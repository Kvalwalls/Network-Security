package AppServiceUtils;

import DataBaseUtils.DBCommand;
import DataUtils.User;
import EnumUtils.EnumCUV;
import EnumUtils.EnumErrorCode;
import EnumUtils.EnumServiceType;
import EnumUtils.EnumUserAccess;
import PropertiesUtils.PropertiesHandler;
import TransmissionUtils.*;
import org.w3c.dom.Document;
import org.w3c.dom.Element;
import org.w3c.dom.Node;
import org.w3c.dom.NodeList;


import java.net.Socket;

public class CommonUserVHandler extends VHandler implements Runnable {
    @Override
    public void run() {
        try {
            while (true)
                if (VCertification())
                    break;
            while (true) {
                TransMessage message = transceiver.receiveMessage();
                message.dePackage(clientPKeyFile, sessionKey);
                if (message.getErrorCode() == EnumErrorCode.Error)
                    throw new Exception("报文错误！");
                switch (message.getSpecificType()) {
                    case EnumCUV.Login -> login(message);
                    case EnumCUV.Register -> register(message);
                    case EnumCUV.Refind -> refind(message);
                    case EnumCUV.ModifyName -> modifyName(message);
                    case EnumCUV.ModifyPassword -> modifyPassword(message);

                }
            }

        } catch (Exception exception) {
            exception.printStackTrace();
        }
    }

    /**
     * 构造方法
     */
    public CommonUserVHandler(Socket socket) {
        this.transceiver = new Transceiver(socket);
    }

    private void login(TransMessage transMessage) throws Exception {
        Document document = XMLPhaser.stringToDoc(transMessage.getContents());
        Element recvRoot = document.getDocumentElement();
        NodeList nodeList = recvRoot.getChildNodes();
        String u_idStr = null, u_passwordStr = null;
        for (int i = 0; i < nodeList.getLength(); i++) {
            Node childNode = nodeList.item(i);
            switch (childNode.getNodeName()) {
                case "u_id" -> u_idStr = childNode.getTextContent();
                case "u_password" -> u_passwordStr = childNode.getTextContent();
            }
        }
        User user = DBCommand.getUserById(u_idStr);
        document = XMLBuilder.buildXMLDoc();
        Element replyRoot = document.createElement("login");
        Element stateElement = document.createElement("state");
        if (user.getUPassword().equals(u_passwordStr) && user.getUAccess() >= EnumUserAccess.U_Comm) {
            stateElement.setTextContent("true");
            Element userElement = document.createElement("user");
            Element u_idElement = document.createElement("u_id");
            u_idElement.setTextContent(user.getUId());
            Element u_nameElement = document.createElement("u_name");
            u_nameElement.setTextContent(user.getUName());
            Element u_passwordElement = document.createElement("u_password");
            u_passwordElement.setTextContent(user.getUPassword());
            Element u_accessElement = document.createElement("u_access");
            u_accessElement.setTextContent(String.valueOf(user.getUAccess()));
            Element u_moneyElement = document.createElement("u_money");
            u_moneyElement.setTextContent(String.valueOf(user.getUMoney()));
            userElement.appendChild(u_idElement);
            userElement.appendChild(u_nameElement);
            userElement.appendChild(u_passwordElement);
            userElement.appendChild(u_accessElement);
            userElement.appendChild(u_moneyElement);
            replyRoot.appendChild(stateElement);
            replyRoot.appendChild(userElement);
        } else {
            stateElement.setTextContent("false");
            replyRoot.appendChild(stateElement);
        }
        document.appendChild(replyRoot);
        //报文初始化
        TransMessage message = new TransMessage();
        message.setToAddress(toAddr);
        message.setFromAddress(fromAddr);
        message.setServiceType(EnumServiceType.CUV);
        message.setSpecificType(EnumCUV.Login);
        message.setContents(XMLPhaser.docToString(document));
        message.enPackage(MySKeyFile, sessionKey);
        transceiver.sendMessage(message);
    }

    private void register(TransMessage transMessage) throws Exception {
        Document document = XMLPhaser.stringToDoc(transMessage.getContents());
        Element recvRoot = document.getDocumentElement();
        NodeList nodeList = recvRoot.getChildNodes();
        User user = new User();
        for (int i = 0; i < nodeList.getLength(); i++) {
            Node childNode = nodeList.item(i);
            switch (childNode.getNodeName()) {
                case "u_id" -> user.setUId(childNode.getTextContent());
                case "u_name" -> user.setUName(childNode.getTextContent());
                case "u_password" -> user.setUPassword(childNode.getTextContent());
                case "u_access" -> user.setUAccess(Byte.parseByte(childNode.getTextContent()));
                case "u_money" -> user.setUMoney(Float.parseFloat(childNode.getTextContent()));
            }
        }
        document = XMLBuilder.buildXMLDoc();
        Element registerElement = document.createElement("register");
        Element stateElement = document.createElement("state");
        stateElement.setTextContent(String.valueOf(DBCommand.insertUser(user)));
        registerElement.appendChild(stateElement);
        document.appendChild(registerElement);
        //报文初始化
        TransMessage message = new TransMessage();
        message.setToAddress(toAddr);
        message.setFromAddress(fromAddr);
        message.setServiceType(EnumServiceType.CUV);
        message.setSpecificType(EnumCUV.Register);
        message.setContents(XMLPhaser.docToString(document));
        message.enPackage(MySKeyFile, sessionKey);
        transceiver.sendMessage(message);
    }

    private void refind(TransMessage transMessage) throws Exception {
        Document document = XMLPhaser.stringToDoc(transMessage.getContents());
        Element recvRoot = document.getDocumentElement();
        NodeList nodeList = recvRoot.getChildNodes();
        String u_idStr = null, u_nameStr = null;
        for (int i = 0; i < nodeList.getLength(); i++) {
            Node childNode = nodeList.item(i);
            switch (childNode.getNodeName()) {
                case "u_id" -> u_idStr = childNode.getTextContent();
                case "u_name" -> u_nameStr = childNode.getTextContent();
            }
        }
        User user = DBCommand.getUserById(u_idStr);
        document = XMLBuilder.buildXMLDoc();
        Element refindElement = document.createElement("refind");
        Element stateElement = document.createElement("state");
        refindElement.appendChild(stateElement);
        if (user != null) {
            stateElement.setTextContent(String.valueOf(user.getUName().equals(u_nameStr)));
            Element u_passwordElement = document.createElement("u_password");
            u_passwordElement.setTextContent(user.getUPassword());
            refindElement.appendChild(u_passwordElement);
        } else
            stateElement.setTextContent("false");
        document.appendChild(refindElement);
        //报文初始化
        TransMessage message = new TransMessage();
        message.setToAddress(toAddr);
        message.setFromAddress(fromAddr);
        message.setServiceType(EnumServiceType.CUV);
        message.setSpecificType(EnumCUV.Refind);
        message.setContents(XMLPhaser.docToString(document));
        message.enPackage(MySKeyFile, sessionKey);
        transceiver.sendMessage(message);
    }

    private void modifyName(TransMessage transMessage) throws Exception {
        Document document = XMLPhaser.stringToDoc(transMessage.getContents());
        Element recvRoot = document.getDocumentElement();
        NodeList nodeList = recvRoot.getChildNodes();
        String u_idStr = null, u_nameStr = null;
        for (int i = 0; i < nodeList.getLength(); i++) {
            Node childNode = nodeList.item(i);
            switch (childNode.getNodeName()) {
                case "u_id" -> u_idStr = childNode.getTextContent();
                case "u_name" -> u_nameStr = childNode.getTextContent();
            }
        }
        document = XMLBuilder.buildXMLDoc();
        Element modifyElement = document.createElement("modify_name");
        Element stateElement = document.createElement("state");
        stateElement.setTextContent(String.valueOf(DBCommand.updateUName(u_idStr,u_nameStr)));
        modifyElement.appendChild(stateElement);
        document.appendChild(modifyElement);
        //报文初始化
        TransMessage message = new TransMessage();
        message.setToAddress(toAddr);
        message.setFromAddress(fromAddr);
        message.setServiceType(EnumServiceType.CUV);
        message.setSpecificType(EnumCUV.ModifyName);
        message.setContents(XMLPhaser.docToString(document));
        message.enPackage(MySKeyFile, sessionKey);
        transceiver.sendMessage(message);
    }

    private void modifyPassword(TransMessage transMessage) throws Exception {
        Document document = XMLPhaser.stringToDoc(transMessage.getContents());
        Element recvRoot = document.getDocumentElement();
        NodeList nodeList = recvRoot.getChildNodes();
        String u_idStr = null, u_passwordStr = null;
        for (int i = 0; i < nodeList.getLength(); i++) {
            Node childNode = nodeList.item(i);
            switch (childNode.getNodeName()) {
                case "u_id" -> u_idStr = childNode.getTextContent();
                case "u_password" -> u_passwordStr = childNode.getTextContent();
            }
        }
        document = XMLBuilder.buildXMLDoc();
        Element modifyElement = document.createElement("modify_password");
        Element stateElement = document.createElement("state");
        stateElement.setTextContent(String.valueOf(DBCommand.updatePassword(u_idStr, u_passwordStr)));
        modifyElement.appendChild(stateElement);
        document.appendChild(modifyElement);
        //报文初始化
        TransMessage message = new TransMessage();
        message.setToAddress(toAddr);
        message.setFromAddress(fromAddr);
        message.setServiceType(EnumServiceType.CUV);
        message.setSpecificType(EnumCUV.ModifyPassword);
        message.setContents(XMLPhaser.docToString(document));
        message.enPackage(MySKeyFile, sessionKey);
        transceiver.sendMessage(message);
    }

    private void getRecords(TransMessage transMessage) {

    }

    private void getOnMovie(TransMessage transMessage) {

    }

    private void buyTicket(TransMessage transMessage) {

    }

    private void refundTicket(TransMessage transMessage) {

    }

    private void modifyPWD(TransMessage transMessage) {

    }

    private void recharge(TransMessage transMessage) {

    }
}