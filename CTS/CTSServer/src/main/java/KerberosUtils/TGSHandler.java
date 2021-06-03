package KerberosUtils;

import DataBaseUtils.DBCommand;
import DataUtils.Authenticator;
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

public class TGSHandler implements Runnable {
    //数据收发器
    private final Transceiver transceiver;
    //连接计数
    private final int counter;

    /**
     * 构造方法
     * @param socket socket用于初始化transceiver
     * @param counter 连接计数
     */
    public TGSHandler(Socket socket,int counter) {
        this.transceiver = new Transceiver(socket);
        this.counter = counter;
    }

    @Override
    public void run() {
        try {
            while (true) {
                TransMessage message = transceiver.receiveMessage();
                String rsaPKFile = "src\\resourcesTGS\\KeyFiles\\"
                        + PropertiesHandler.getElement(AddressPhaser.bytesToString(message.getFromAddress()))
                        + ".pk";
                message.dePackage(rsaPKFile, null);
                System.out.println(message.getContents());
                switch (message.getSpecificType()) {
                    case EnumKerberos.Request: {
                        TransMessage replyMessage = null;
                        //报文有错误
                        if (message.getErrorCode() == EnumErrorCode.Error) {
                            replyMessage = generateErrorReply(message.getFromAddress());
                            transceiver.sendMessage(replyMessage);
                            continue;
                        }
                        //解析XMLDocument
                        String id_vStr = null, ticket_tgsStr = null, authenticator_cStr = null;
                        Document document = XMLPhaser.stringToDoc(message.getContents());
                        Element certification = document.getDocumentElement();
                        NodeList nodeList = certification.getChildNodes();
                        for (int i = 0; i < nodeList.getLength(); i++) {
                            Node childNode = nodeList.item(i);
                            switch (childNode.getNodeName()) {
                                case "id_v" -> id_vStr = childNode.getTextContent();
                                case "ticket_tgs" -> ticket_tgsStr = childNode.getTextContent();
                                case "authenticator_c" -> authenticator_cStr = childNode.getTextContent();
                            }
                        }
                        Ticket tgsTicket = new Ticket(ticket_tgsStr, PropertiesHandler.getElement("TGS_Key"));
                        Authenticator authenticator = new Authenticator(authenticator_cStr, tgsTicket.getKey());
                        if (!ToolsKerberos.verifyTicketAndAuthenticator(tgsTicket, authenticator)) {
                            replyMessage = generateErrorReply(message.getFromAddress());
                            transceiver.sendMessage(replyMessage);
                            continue;
                        }
                        String key_v = DBCommand.getKeyById(id_vStr);
                        String sessionKey = ToolsKerberos.generateSessionK();
                        long ts4 = ToolsKerberos.generateTS();
                        Ticket vTicket = new Ticket();
                        vTicket.setKey(sessionKey);
                        vTicket.setID_c(authenticator.getID_c());
                        vTicket.setAD_c(AddressPhaser.bytesToString(message.getFromAddress()));
                        vTicket.setID_dest(id_vStr);
                        vTicket.setTimestamp(ts4);
                        vTicket.setLifetime(ToolsKerberos.LIFE_TIME);
                        String vTicketStr = vTicket.generateTicket(key_v);
                        String[] contents = new String[]{
                                sessionKey,
                                id_vStr,
                                String.valueOf(ts4),
                                vTicketStr
                        };
                        replyMessage = generateNormalReply(message.getFromAddress(), contents, tgsTicket.getKey());
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

    private TransMessage generateNormalReply(byte[] toAddr,String[] contents,String key_c_tgs) throws Exception {
        //创建XMLDocument
        Document document = XMLBuilder.buildXMLDoc();
        //根节点
        Element root = document.createElement("tgs_reply");
        //子节点
        Element keyElement = document.createElement("key");
        keyElement.setTextContent(contents[0]);
        Element id_vElement = document.createElement("id_v");
        id_vElement.setTextContent(contents[1]);
        Element ts4Element = document.createElement("ts4");
        ts4Element.setTextContent(contents[2]);
        Element ticket_vElement = document.createElement("ticket_v");
        ticket_vElement.setTextContent(contents[3]);
        //形成树结构
        root.appendChild(keyElement);
        root.appendChild(id_vElement);
        root.appendChild(ts4Element);
        root.appendChild(ticket_vElement);
        document.appendChild(root);
        //报文初始化
        TransMessage message = new TransMessage();
        message.setToAddress(toAddr);
        message.setFromAddress(AddressPhaser.stringToBytes(
                PropertiesHandler.getElement("TGS_IPAddress")));
        message.setServiceType(EnumServiceType.AS);
        message.setSpecificType(EnumKerberos.Reply);
        message.setContents(XMLPhaser.docToString(document));
        System.out.println(message.getContents());
        message.enPackage(PropertiesHandler.getElement("TGS_SKeyFile"), key_c_tgs);
        return message;
    }

    private TransMessage generateErrorReply(byte[] toAddr) throws Exception {
        //创建XMLDocument
        Document document = XMLBuilder.buildXMLDoc();
        //根节点
        Element root = document.createElement("tgs_error");
        document.appendChild(root);
        //报文初始化
        TransMessage message = new TransMessage();
        message.setToAddress(toAddr);
        message.setFromAddress(AddressPhaser.stringToBytes(
                PropertiesHandler.getElement("AS_IPAddress")));
        message.setServiceType(EnumServiceType.AS);
        message.setSpecificType(EnumKerberos.Error);
        message.setContents(XMLPhaser.docToString(document));
        message.enPackage(PropertiesHandler.getElement("TGS_SKeyFile"), null);
        return message;
    }
}