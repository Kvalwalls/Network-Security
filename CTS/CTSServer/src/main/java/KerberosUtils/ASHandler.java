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

public class ASHandler implements Runnable {
    //数据收发器
    private final Transceiver transceiver;
    //连接计数
    private final int counter;

    /**
     * 构造方法
     *
     * @param socket  socket用于初始化transceiver
     * @param counter 连接计数
     */
    public ASHandler(Socket socket, int counter) {
        this.transceiver = new Transceiver(socket);
        this.counter = counter;
    }

    /**
     * 线程执行方法
     */
    @Override
    public void run() {
        try {
            while (true) {
                TransMessage message = transceiver.receiveMessage();
                String rsaPKFile = "src\\resourcesAS\\KeyFiles\\"
                        + PropertiesHandler.getElement(AddressPhaser.bytesToString(message.getFromAddress()))
                        + ".pk";
                message.dePackage(rsaPKFile, null);
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
                        String id_cStr = null, id_tgsStr = null;
                        long ts1Long = 0;
                        Document document = XMLPhaser.stringToDoc(message.getContents());
                        Element certificationElement = document.getDocumentElement();
                        NodeList nodeList = certificationElement.getChildNodes();
                        for (int i = 0; i < nodeList.getLength(); i++) {
                            Node childNode = nodeList.item(i);
                            switch (childNode.getNodeName()) {
                                case "id_c" -> id_cStr = childNode.getTextContent();
                                case "id_tgs" -> id_tgsStr = childNode.getTextContent();
                                case "ts1" -> ts1Long = Long.parseLong(childNode.getTextContent());
                            }
                        }
                        //报文超过生命周期
                        if (!ToolsKerberos.verifyTS(ts1Long, ToolsKerberos.LIFE_TIME)) {
                            replyMessage = generateErrorReply(message.getFromAddress());
                            transceiver.sendMessage(replyMessage);
                            continue;
                        }
                        String key_tgs = DBCommand.getKeyById(id_tgsStr);
                        String key_c = DBCommand.getKeyById(id_cStr);
                        String sessionKey = ToolsKerberos.generateSessionK();
                        long ts2 = ToolsKerberos.generateTS();
                        Ticket tgsTicket = new Ticket();
                        tgsTicket.setKey(sessionKey);
                        tgsTicket.setID_c(id_cStr);
                        tgsTicket.setAD_c(AddressPhaser.bytesToString(message.getFromAddress()));
                        tgsTicket.setID_dest(id_tgsStr);
                        tgsTicket.setTimestamp(ts2);
                        tgsTicket.setLifetime(ToolsKerberos.LIFE_TIME);
                        String tgsTicketStr = tgsTicket.generateTicket(key_tgs);
                        String[] contents = new String[]{
                                sessionKey,
                                id_tgsStr,
                                String.valueOf(ts2),
                                String.valueOf(ToolsKerberos.LIFE_TIME),
                                tgsTicketStr
                        };
                        replyMessage = generateNormalReply(message.getFromAddress(), contents, key_c);
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

    /**
     * 生成正常回复报文
     *
     * @param toAddr   目的IP地址
     * @param contents 内容
     * @param key_c    id_c对应密钥
     * @return 报文对象
     * @throws Exception 异常
     */
    private TransMessage generateNormalReply(byte[] toAddr, String[] contents, String key_c) throws Exception {
        //创建XMLDocument
        Document document = XMLBuilder.buildXMLDoc();
        //根节点
        Element root = document.createElement("as_reply");
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
                PropertiesHandler.getElement("AS_IPAddress")));
        message.setServiceType(EnumServiceType.AS);
        message.setSpecificType(EnumKerberos.Reply);
        message.setContents(XMLPhaser.docToString(document));
        message.enPackage(PropertiesHandler.getElement("AS_SKeyFile"), key_c);
        return message;
    }

    /**
     * 生成错误回复报文
     *
     * @param toAddr 目的IP地址
     * @return 报文对象
     * @throws Exception 异常
     */
    private TransMessage generateErrorReply(byte[] toAddr) throws Exception {
        //创建XMLDocument
        Document document = XMLBuilder.buildXMLDoc();
        //根节点
        Element root = document.createElement("as_error");
        document.appendChild(root);
        //报文初始化
        TransMessage message = new TransMessage();
        message.setToAddress(toAddr);
        message.setFromAddress(AddressPhaser.stringToBytes(
                PropertiesHandler.getElement("AS_IPAddress")));
        message.setServiceType(EnumServiceType.AS);
        message.setSpecificType(EnumKerberos.Error);
        message.setContents(XMLPhaser.docToString(document));
        message.enPackage(PropertiesHandler.getElement("AS_SKeyFile"), null);
        return message;
    }
}