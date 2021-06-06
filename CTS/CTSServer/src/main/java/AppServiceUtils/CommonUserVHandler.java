package AppServiceUtils;

import DataBaseUtils.DBCommand;
import DataUtils.Movie;
import DataUtils.Record;
import DataUtils.User;
import EnumUtils.EnumCUV;
import EnumUtils.EnumErrorCode;
import EnumUtils.EnumServiceType;
import EnumUtils.EnumUserAccess;
import TransmissionUtils.*;
import org.w3c.dom.Document;
import org.w3c.dom.Element;
import org.w3c.dom.Node;
import org.w3c.dom.NodeList;


import java.net.Socket;
import java.util.ArrayList;

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
                    case EnumCUV.GetUser -> getUser(message);
                    case EnumCUV.ModifyName -> modifyName(message);
                    case EnumCUV.ModifyPassword -> modifyPassword(message);
                    case EnumCUV.UpgradeAccess -> upgradeAccess(message);
                    case EnumCUV.Recharge -> recharge(message);
                    case EnumCUV.GetMovies -> getMovies(message);
                    case EnumCUV.GetMoviePictures -> getMoviePictures(message);
                    case EnumCUV.GetRecords -> getRecords(message);
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
        if (user.getUPassword().equals(u_passwordStr) && user.getUAccess() >= EnumUserAccess.U_Comm)
            stateElement.setTextContent("true");
        else
            stateElement.setTextContent("false");
        replyRoot.appendChild(stateElement);
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

    private void getUser(TransMessage transMessage) throws Exception {
        Document document = XMLPhaser.stringToDoc(transMessage.getContents());
        Element recvRoot = document.getDocumentElement();
        NodeList nodeList = recvRoot.getChildNodes();
        String u_idStr = null;
        for (int i = 0; i < nodeList.getLength(); i++) {
            Node childNode = nodeList.item(i);
            if ("u_id".equals(childNode.getNodeName())) {
                u_idStr = childNode.getTextContent();
            }
        }
        User user = DBCommand.getUserById(u_idStr);
        document = XMLBuilder.buildXMLDoc();
        Element replyRoot = document.createElement("get_user");
        Element stateElement = document.createElement("state");
        if (user != null) {
            stateElement.setTextContent("true");
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
            replyRoot.appendChild(stateElement);
            replyRoot.appendChild(u_idElement);
            replyRoot.appendChild(u_nameElement);
            replyRoot.appendChild(u_passwordElement);
            replyRoot.appendChild(u_accessElement);
            replyRoot.appendChild(u_moneyElement);
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
        message.setSpecificType(EnumCUV.GetUser);
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

    private void upgradeAccess(TransMessage transMessage) throws Exception {
        Document document = XMLPhaser.stringToDoc(transMessage.getContents());
        Element recvRoot = document.getDocumentElement();
        NodeList nodeList = recvRoot.getChildNodes();
        String u_idStr = null;
        byte u_accessByte = EnumUserAccess.U_Comm;
        for (int i = 0; i < nodeList.getLength(); i++) {
            Node childNode = nodeList.item(i);
            switch (childNode.getNodeName()) {
                case "u_id" -> u_idStr = childNode.getTextContent();
                case "u_access" -> u_accessByte = Byte.parseByte(childNode.getTextContent());
            }
        }
        document = XMLBuilder.buildXMLDoc();
        Element modifyElement = document.createElement("upgrade_access");
        Element stateElement = document.createElement("state");
        stateElement.setTextContent(String.valueOf(DBCommand.updateAccess(u_idStr, u_accessByte)));
        modifyElement.appendChild(stateElement);
        document.appendChild(modifyElement);
        //报文初始化
        TransMessage message = new TransMessage();
        message.setToAddress(toAddr);
        message.setFromAddress(fromAddr);
        message.setServiceType(EnumServiceType.CUV);
        message.setSpecificType(EnumCUV.UpgradeAccess);
        message.setContents(XMLPhaser.docToString(document));
        message.enPackage(MySKeyFile, sessionKey);
        transceiver.sendMessage(message);
    }

    private void recharge(TransMessage transMessage) throws Exception {
        Document document = XMLPhaser.stringToDoc(transMessage.getContents());
        Element recvRoot = document.getDocumentElement();
        NodeList nodeList = recvRoot.getChildNodes();
        String u_idStr = null;
        float u_moneyFloat = 0;
        for (int i = 0; i < nodeList.getLength(); i++) {
            Node childNode = nodeList.item(i);
            switch (childNode.getNodeName()) {
                case "u_id" -> u_idStr = childNode.getTextContent();
                case "u_money" -> u_moneyFloat = Float.parseFloat(childNode.getTextContent());
            }
        }
        document = XMLBuilder.buildXMLDoc();
        Element fundElement = document.createElement("fund_money");
        Element stateElement = document.createElement("state");
        stateElement.setTextContent(String.valueOf(DBCommand.fundMoney(u_idStr, u_moneyFloat)));
        fundElement.appendChild(stateElement);
        document.appendChild(fundElement);
        //报文初始化
        TransMessage message = new TransMessage();
        message.setToAddress(toAddr);
        message.setFromAddress(fromAddr);
        message.setServiceType(EnumServiceType.CUV);
        message.setSpecificType(EnumCUV.Recharge);
        message.setContents(XMLPhaser.docToString(document));
        message.enPackage(MySKeyFile, sessionKey);
        transceiver.sendMessage(message);
    }

    private void getMovies(TransMessage transMessage) throws Exception {
        ArrayList<Movie> movies = DBCommand.getAllMovies();
        Document document = XMLBuilder.buildXMLDoc();
        Element getElement = document.createElement("get_movies");
        for(Movie temp : movies) {
            Element movieElement = document.createElement("movie");
            Element m_idElement = document.createElement("m_id");
            m_idElement.setTextContent(temp.getMId());
            Element m_nameElement = document.createElement("m_name");
            m_nameElement.setTextContent(temp.getMName());
            Element m_typeElement = document.createElement("m_type");
            m_typeElement.setTextContent(temp.getMType());
            Element m_timeElement = document.createElement("m_time");
            m_timeElement.setTextContent(String.valueOf(temp.getMTime()));
            Element m_commentElement = document.createElement("m_comment");
            m_commentElement.setTextContent(String.valueOf(temp.getMComment()));
            Element m_descriptionElement = document.createElement("m_description");
            m_descriptionElement.setTextContent(temp.getDescription());
            movieElement.appendChild(m_idElement);
            movieElement.appendChild(m_nameElement);
            movieElement.appendChild(m_typeElement);
            movieElement.appendChild(m_timeElement);
            movieElement.appendChild(m_commentElement);
            movieElement.appendChild(m_descriptionElement);
            getElement.appendChild(movieElement);
        }
        document.appendChild(getElement);
        //报文初始化
        TransMessage message = new TransMessage();
        message.setToAddress(toAddr);
        message.setFromAddress(fromAddr);
        message.setServiceType(EnumServiceType.CUV);
        message.setSpecificType(EnumCUV.GetMovies);
        message.setContents(XMLPhaser.docToString(document));
        message.enPackage(MySKeyFile, sessionKey);
        transceiver.sendMessage(message);
    }

    private void getMoviePictures(TransMessage transMessage) throws Exception {
        ArrayList<Movie> movies = DBCommand.getAllMovies();
        Document document = XMLBuilder.buildXMLDoc();
        Element getElement = document.createElement("get_movie_pictures");
        Element countElement = document.createElement("count");
        countElement.setTextContent(String.valueOf(movies.size()));
        getElement.appendChild(countElement);
        document.appendChild(getElement);
        //报文初始化
        TransMessage message = new TransMessage();
        message.setToAddress(toAddr);
        message.setFromAddress(fromAddr);
        message.setServiceType(EnumServiceType.CUV);
        message.setSpecificType(EnumCUV.GetMoviePictures);
        message.setContents(XMLPhaser.docToString(document));
        message.enPackage(MySKeyFile, sessionKey);
        transceiver.sendMessage(message);
        for(Movie temp : movies) {
            document = XMLBuilder.buildXMLDoc();
            getElement = document.createElement("get_movie_pictures");
            Element m_idElement = document.createElement("m_id");
            m_idElement.setTextContent(temp.getMId());
            getElement.appendChild(m_idElement);
            String moviePicName = "src\\resourcesV\\MoviePictures\\" + temp.getMId() + ".jpg";
            String moviePicStr = PicturePhaser.pictureToBase64(moviePicName);
            Element m_pictureElement = document.createElement("m_picture");
            m_pictureElement.setTextContent(moviePicStr);
            getElement.appendChild(m_pictureElement);
            document.appendChild(getElement);
            //报文初始化
            message = new TransMessage();
            message.setToAddress(toAddr);
            message.setFromAddress(fromAddr);
            message.setServiceType(EnumServiceType.CUV);
            message.setSpecificType(EnumCUV.GetMovies);
            message.setContents(XMLPhaser.docToString(document));
            message.enPackage(MySKeyFile, sessionKey);
            transceiver.sendMessage(message);
        }
    }

    private void getRecords(TransMessage transMessage) throws Exception {
        Document document = XMLBuilder.buildXMLDoc();
        Element recvRoot = document.getDocumentElement();
        String u_idStr = recvRoot.getFirstChild().getTextContent();
        ArrayList<DataUtils.Record> records = DBCommand.getRecordByUid(u_idStr);
        document = XMLBuilder.buildXMLDoc();
        Element getElement = document.createElement("get_records");
        for (Record temp : records) {
            Element recordElement = document.createElement("record");
            Element u_idElement = document.createElement("u_id");
            u_idElement.setTextContent(temp.getUId());
            Element o_idElement = document.createElement("o_id");
            o_idElement.setTextContent(temp.getOId());
            Element s_idElement = document.createElement("s_id");
            s_idElement.setTextContent(temp.getSId());
            Element r_timeElement = document.createElement("r_time");
            r_timeElement.setTextContent(DatePhaser.dateToDateStr(temp.getRTime()));
            Element r_priceElement = document.createElement("r_price");
            r_priceElement.setTextContent(String.valueOf(temp.getRPrice()));
            Element r_statusElement = document.createElement("r_status");
            r_statusElement.setTextContent(String.valueOf(temp.getStatus()));
            recordElement.appendChild(u_idElement);
            recordElement.appendChild(o_idElement);
            recordElement.appendChild(s_idElement);
            recordElement.appendChild(r_timeElement);
            recordElement.appendChild(r_priceElement);
            recordElement.appendChild(r_statusElement);
            getElement.appendChild(recordElement);
        }
        document.appendChild(getElement);
        //报文初始化
        TransMessage message = new TransMessage();
        message.setToAddress(toAddr);
        message.setFromAddress(fromAddr);
        message.setServiceType(EnumServiceType.CUV);
        message.setSpecificType(EnumCUV.GetRecords);
        message.setContents(XMLPhaser.docToString(document));
        message.enPackage(MySKeyFile, sessionKey);
        transceiver.sendMessage(message);
    }

    private void getOnMovies(TransMessage transMessage) {

    }

    private void refundTicket(TransMessage transMessage) {

    }

    private void modifyPWD(TransMessage transMessage) {

    }

}