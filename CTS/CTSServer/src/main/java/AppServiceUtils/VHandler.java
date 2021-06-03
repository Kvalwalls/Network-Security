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

public abstract class VHandler {
    protected String sessionKey = null;

    protected Transceiver transceiver;

    protected boolean VCertification() throws Exception {
        while (true) {
            TransMessage message = transceiver.receiveMessage();
            String rsaPKFile = "src\\resourcesV\\KeyFiles\\"
                    + PropertiesHandler.getElement(AddressPhaser.bytesToString(message.getFromAddress()))
                    + ".pk";
            message.dePackage(rsaPKFile, null);
            System.out.println(message.getContents());
            if(message.getSpecificType() == EnumKerberos.Request) {
                TransMessage replyMessage = null;
                //报文有错误
                if (message.getErrorCode() == EnumErrorCode.Error) {
                    replyMessage = generateErrorReply(message.getFromAddress(), message.getServiceType());
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
                    replyMessage = generateErrorReply(message.getFromAddress(), message.getServiceType());
                    transceiver.sendMessage(replyMessage);
                    continue;
                }
                sessionKey = vTicket.getKey();
                replyMessage = generateNormalReply(message.getFromAddress(),
                        String.valueOf(vTicket.getTimestamp() + 1),
                        vTicket.getKey(),
                        message.getServiceType());
                return true;
            } else {
                return false;
            }
        }
    }

    private TransMessage generateNormalReply(byte[] toAddr,String contents,String key_c_v,byte type) throws Exception {
        //创建XMLDocument
        Document document = XMLBuilder.buildXMLDoc();
        //根节点
        Element root = document.createElement("v_reply");
        //子节点
        Element ts5plusElement = document.createElement("ts5plus");
        ts5plusElement.setTextContent(contents);
        root.appendChild(ts5plusElement);
        document.appendChild(root);
        //报文初始化
        TransMessage message = new TransMessage();
        message.setToAddress(toAddr);
        if (type == EnumServiceType.AUV)
            message.setFromAddress(AddressPhaser.stringToBytes(
                    PropertiesHandler.getElement("AUV_IPAddress")));
        else
            message.setFromAddress(AddressPhaser.stringToBytes(
                    PropertiesHandler.getElement("CUV_IPAddress")));
        message.setServiceType(type);
        message.setSpecificType(EnumKerberos.Error);
        message.setContents(XMLPhaser.docToString(document));
        if (type == EnumServiceType.AUV)
            message.enPackage(PropertiesHandler.getElement("AUV_SKeyFile"), key_c_v);
        else
            message.enPackage(PropertiesHandler.getElement("CUV_SKeyFile"), key_c_v);
        return message;
    }

    private TransMessage generateErrorReply(byte[] toAddr,byte type) throws Exception {
        //创建XMLDocument
        Document document = XMLBuilder.buildXMLDoc();
        //根节点
        Element root = document.createElement("v_error");
        document.appendChild(root);
        //报文初始化
        TransMessage message = new TransMessage();
        message.setToAddress(toAddr);
        if (type == EnumServiceType.AUV)
            message.setFromAddress(AddressPhaser.stringToBytes(
                    PropertiesHandler.getElement("AUV_IPAddress")));
        else
            message.setFromAddress(AddressPhaser.stringToBytes(
                    PropertiesHandler.getElement("CUV_IPAddress")));
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