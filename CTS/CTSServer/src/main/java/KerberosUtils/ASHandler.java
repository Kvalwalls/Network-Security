package KerberosUtils;

import DataBaseUtils.DBCommand;
import DataUtils.Ticket;
import EnumUtils.EnumErrorCode;
import EnumUtils.EnumKerberos;
import EnumUtils.EnumServiceType;
import PropertiesUtils.PropertiesHandler;
import TransmissionUtils.*;
import org.w3c.dom.Document;
import org.w3c.dom.Element;
import org.w3c.dom.Node;
import org.w3c.dom.NodeList;

import java.net.Socket;
import java.util.Arrays;

public class ASHandler implements Runnable {
    private static final int LIFE_TIME = 60;
    private final Transceiver transceiver;
    private final int counter;

    /**
     * 构造方法
     *
     * @param socket socket用于初始化transceiver
     */
    public ASHandler(Socket socket,int counter) {
        this.transceiver = new Transceiver(socket);
        this.counter = counter;
    }

    @Override
    public void run() {
        try {
            while(true) {
                TransMessage message = transceiver.receiveMessage();
                message.dePackage("src\\resourcesAS\\KeyFiles\\Client1.pk",null);
                switch(message.getSpecificType()) {
                    case EnumKerberos.Request: {
                        TransMessage replyMessage = null;
                        //报文有错误
                        if (message.getErrorCode() == EnumErrorCode.Error) {
                            replyMessage = generateErrorReply(AddressPhaser.bytesToString(message.getFromAddress()));
                            transceiver.sendMessage(replyMessage);
                            continue;
                        }
                        //解析XMLDocument
                        String id_c = null, id_tgs = null;
                        long ts1 = 0;
                        Document document = XMLPhaser.stringToDoc(message.getContents());
                        Element certification = document.getDocumentElement();
                        NodeList nodeList = certification.getChildNodes();
                        for (int i = 0; i < nodeList.getLength(); i++) {
                            Node childNode = nodeList.item(i);
                            switch (childNode.getNodeName()) {
                                case "id_c" -> id_c = childNode.getTextContent();
                                case "id_tgs" -> id_tgs = childNode.getTextContent();
                                case "ts1" -> ts1 = Long.parseLong(childNode.getTextContent());
                            }
                        }
                        //报文超过生命周期
                        if (!Tools.verifyTS(ts1, LIFE_TIME)) {
                            replyMessage = generateErrorReply(AddressPhaser.bytesToString(message.getFromAddress()));
                            transceiver.sendMessage(replyMessage);
                            continue;
                        }
                        String key_tgs = DBCommand.getKeyById(id_tgs);
                        String key_c = DBCommand.getKeyById(id_c);
                        String sessionKey = Tools.generateSessionK();
                        long ts2 = Tools.generateTS();
                        Ticket ticket = new Ticket();
                        ticket.setKey(sessionKey);
                        ticket.setID_c(id_c);
                        ticket.setAD_c(AddressPhaser.bytesToString(message.getFromAddress()));
                        ticket.setID_dest(id_tgs);
                        ticket.setTimestamp(ts2);
                        ticket.setLifetime(LIFE_TIME);
                        String strTicket = ticket.generateTicket(key_tgs);
                        String[] contents = new String[] {
                                sessionKey,
                                id_tgs,
                                String.valueOf(ts2),
                                String.valueOf(LIFE_TIME),
                                strTicket
                        };
                        replyMessage = generateNormalReply(message.getFromAddress(),contents,key_c);
                        transceiver.sendMessage(replyMessage);
                    }
                    case EnumKerberos.End: {
                        transceiver.closeTransceiver();
                        System.out.println("断开第 " + counter + " 个连接!");
                        return;
                    }
                    default:
                        throw new Exception("未知服务类型！");
                }
            }
        } catch (Exception exception) {
            exception.printStackTrace();
        }
    }

    private TransMessage generateNormalReply(byte[] toAddr,String[] contents,String key_c) throws Exception {
        //创建XMLDocument
        Document document = XMLBuilder.buildXMLDoc();
        //根节点
        Element root = document.createElement("reply");
        //子节点
        Element keyElement = document.createElement("key");
        keyElement.setTextContent(contents[0]);
        Element id_tgsElement = document.createElement("id_tgs");
        id_tgsElement.setTextContent(contents[1]);
        Element ts2Element = document.createElement("ts2");
        ts2Element.setTextContent(contents[2]);
        Element lifetimeElement = document.createElement("lifetime");
        lifetimeElement.setTextContent(contents[3]);
        Element ticket_tgsElement = document.createElement("ticket_tgs");
        ticket_tgsElement.setTextContent(contents[4]);
        //形成树结构
        root.appendChild(keyElement);
        root.appendChild(id_tgsElement);
        root.appendChild(ts2Element);
        root.appendChild(lifetimeElement);
        root.appendChild(ticket_tgsElement);
        document.appendChild(root);
        //报文初始化
        TransMessage message = new TransMessage();
        message.setToAddress(toAddr);
        message.setFromAddress(AddressPhaser.stringToBytes(
                PropertiesHandler.getPropertiesElement("My_IPAddress")));
        message.setServiceType(EnumServiceType.AS);
        message.setSpecificType(EnumKerberos.Reply);
        message.setContents(XMLPhaser.docToString(document));
        message.enPackage("src\\resourcesAS\\KeyFiles\\AS.sk", key_c);
        return message;
    }

    private TransMessage generateErrorReply(String toAddr) throws Exception {
        //创建XMLDocument
        Document document = XMLBuilder.buildXMLDoc();
        //根节点
        Element root = document.createElement("error");
        document.appendChild(root);
        //报文初始化
        TransMessage message = new TransMessage();
        message.setToAddress(AddressPhaser.stringToBytes(toAddr));
        message.setFromAddress(AddressPhaser.stringToBytes(
                PropertiesHandler.getPropertiesElement("My_IPAddress")));
        message.setServiceType(EnumServiceType.AS);
        message.setSpecificType(EnumKerberos.Error);
        message.setContents(XMLPhaser.docToString(document));
        message.enPackage("src\\resourcesAS\\KeyFiles\\AS.sk", null);
        return message;
    }
}