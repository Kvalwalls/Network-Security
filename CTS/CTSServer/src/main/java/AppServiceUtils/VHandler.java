package AppServiceUtils;

import DataUtils.Authenticator;
import DataUtils.Ticket;
import EnumUtils.EnumErrorCode;
import EnumUtils.EnumKerberos;
import EnumUtils.EnumServiceType;
import KerberosUtils.ToolsKerberos;
import PropertiesUtils.PropertiesHandler;
import TransmissionUtils.*;
import org.w3c.dom.Document;
import org.w3c.dom.Element;
import org.w3c.dom.Node;
import org.w3c.dom.NodeList;

import java.awt.*;

public abstract class VHandler {
    protected String sessionKey;

    protected String rsaPKFile;

    protected byte[] fromAddr;

    protected byte[] toAddr;

    protected Transceiver transceiver;

    protected boolean VCertification() throws Exception {
        while (true) {
            TransMessage message = transceiver.receiveMessage();
            toAddr = message.getFromAddress();
            fromAddr = message.getToAddress();
            rsaPKFile = "src\\resourcesV\\KeyFiles\\"
                    + PropertiesHandler.getElement(AddressPhaser.bytesToString(message.getFromAddress()))
                    + ".pk";
            message.dePackage(rsaPKFile, null);
            if(message.getSpecificType() == EnumKerberos.Request) {
                TransMessage replyMessage = null;
                //报文有错误
                if (message.getErrorCode() == EnumErrorCode.Error) {
                    replyMessage = generateErrorReply(message.getServiceType());
                    transceiver.sendMessage(replyMessage);
                    continue;
                }
                //解析XMLDocument
                String ticket_vStr = null, authenticator_cStr = null;
                Document document = XMLPhaser.stringToDoc(message.getContents());
                Element certification = document.getDocumentElement();
                NodeList nodeList = certification.getChildNodes();
                for (int i = 0; i < nodeList.getLength(); i++) {
                    Node childNode = nodeList.item(i);
                    switch (childNode.getNodeName()) {
                        case "ticket_v" -> ticket_vStr = childNode.getTextContent();
                        case "authenticator_c" -> authenticator_cStr = childNode.getTextContent();
                    }
                }
                Ticket vTicket = null;
                if (message.getServiceType() == EnumServiceType.AUV)
                    vTicket = new Ticket(ticket_vStr, PropertiesHandler.getElement("AUV_Key"));
                else
                    vTicket = new Ticket(ticket_vStr, PropertiesHandler.getElement("CUV_Key"));
                Authenticator authenticator = new Authenticator(authenticator_cStr, vTicket.getKey());
                if (!ToolsKerberos.verifyTicketAndAuthenticator(vTicket, authenticator)) {
                    replyMessage = generateErrorReply(message.getServiceType());
                    transceiver.sendMessage(replyMessage);
                    continue;
                }
                sessionKey = vTicket.getKey();
                replyMessage = generateNormalReply(vTicket.getTimestamp() + 1,
                        vTicket.getKey(),
                        message.getServiceType());
                transceiver.sendMessage(replyMessage);
                return true;
            } else {
                return false;
            }
        }
    }

    private TransMessage generateNormalReply(long ts5plus,String key_c_v,byte type) throws Exception {
        //创建XMLDocument
        Document document = XMLBuilder.buildXMLDoc();
        //根节点
        Element root = document.createElement("v_reply");
        //子节点
        Element ts5plusElement = document.createElement("ts5plus");
        ts5plusElement.setTextContent(String.valueOf(ts5plus));
        root.appendChild(ts5plusElement);
        document.appendChild(root);
        //报文初始化
        TransMessage message = new TransMessage();
        message.setToAddress(toAddr);
        message.setFromAddress(fromAddr);
        message.setServiceType(type);
        message.setSpecificType(EnumKerberos.Reply);
        message.setContents(XMLPhaser.docToString(document));
        if (type == EnumServiceType.AUV)
            message.enPackage(PropertiesHandler.getElement("AUV_SKeyFile"), key_c_v);
        else
            message.enPackage(PropertiesHandler.getElement("CUV_SKeyFile"), key_c_v);
        return message;
    }

    private TransMessage generateErrorReply(byte type) throws Exception {
        //创建XMLDocument
        Document document = XMLBuilder.buildXMLDoc();
        //根节点
        Element root = document.createElement("v_error");
        document.appendChild(root);
        //报文初始化
        TransMessage message = new TransMessage();
        message.setToAddress(toAddr);
        message.setFromAddress(fromAddr);
        message.setServiceType(type);
        message.setSpecificType(EnumKerberos.Error);
        message.setContents(XMLPhaser.docToString(document));
        if (type == EnumServiceType.AUV)
            message.enPackage(PropertiesHandler.getElement("AUV_SKeyFile"), null);
        else
            message.enPackage(PropertiesHandler.getElement("CUV_SKeyFile"), null);
        return message;
    }
}