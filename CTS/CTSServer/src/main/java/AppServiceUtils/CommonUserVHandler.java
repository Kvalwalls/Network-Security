package AppServiceUtils;

import DataBaseUtils.DBCommand;
import DataUtils.*;
import DataUtils.Record;
import EnumUtils.*;
import TransmissionUtils.*;
import org.w3c.dom.Document;
import org.w3c.dom.Element;
import org.w3c.dom.Node;
import org.w3c.dom.NodeList;


import java.net.Socket;
import java.util.ArrayList;
import java.util.List;

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
                    case EnumCUV.GetOnMovies -> getOnMovies(message);
                    case EnumCUV.GetSeats -> getSeats(message);
                    case EnumCUV.GetTheater -> getTheater(message);
                    case EnumCUV.SelectSeat -> selectSeat(message);
                    case EnumCUV.PayMoney -> payMoney(message);
                    case EnumCUV.PayTimeout -> payTimeout(message);
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
        stateElement.setTextContent(String.valueOf(DBCommand.updateUName(u_idStr, u_nameStr)));
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
        for (Movie temp : movies) {
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
        for (Movie temp : movies) {
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
        Document document = XMLPhaser.stringToDoc(transMessage.getContents());
        Element recvRoot = document.getDocumentElement();
        NodeList nodeList = recvRoot.getChildNodes();
        String u_idStr = null;
        for (int i = 0; i < nodeList.getLength(); i++) {
            Node childNode = nodeList.item(i);
            switch (childNode.getNodeName()) {
                case "u_id" -> u_idStr = childNode.getTextContent();
            }
        }
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

    private void getOnMovies(TransMessage transMessage) throws Exception {
        Document document = XMLPhaser.stringToDoc(transMessage.getContents());
        Element recvRoot = document.getDocumentElement();
        NodeList nodeList = recvRoot.getChildNodes();
        String m_idStr = null;
        for (int i = 0; i < nodeList.getLength(); i++) {
            Node childNode = nodeList.item(i);
            switch (childNode.getNodeName()) {
                case "m_id" -> m_idStr = childNode.getTextContent();
            }
        }
        ArrayList<OnMovie> onMovies = DBCommand.getOnMoviesByMId(m_idStr);
        document = XMLBuilder.buildXMLDoc();
        Element getElement = document.createElement("get_onmovies");

        for (OnMovie temp : onMovies) {
            Element onMovieElement = document.createElement("onmovie");
            Element o_idElement = document.createElement("o_id");
            o_idElement.setTextContent(temp.getOId());
            Element m_idElement = document.createElement("m_id");
            m_idElement.setTextContent(temp.getMId());
            Element t_idElement = document.createElement("t_id");
            t_idElement.setTextContent(temp.getTId());
            Element o_begin_timeElement = document.createElement("o_begin_time");
            o_begin_timeElement.setTextContent(DatePhaser.dateToDateStr(temp.getOBegin()));
            Element o_end_timeElement = document.createElement("o_end_time");
            o_end_timeElement.setTextContent(DatePhaser.dateToDateStr(temp.getOEnd()));
            Element o_priceElement = document.createElement("o_price");
            o_priceElement.setTextContent(String.valueOf(temp.getOPrice()));
            onMovieElement.appendChild(o_idElement);
            onMovieElement.appendChild(m_idElement);
            onMovieElement.appendChild(t_idElement);
            onMovieElement.appendChild(o_begin_timeElement);
            onMovieElement.appendChild(o_end_timeElement);
            onMovieElement.appendChild(o_priceElement);
            getElement.appendChild(onMovieElement);
        }
        document.appendChild(getElement);
        //报文初始化
        TransMessage message = new TransMessage();
        message.setToAddress(toAddr);
        message.setFromAddress(fromAddr);
        message.setServiceType(EnumServiceType.CUV);
        message.setSpecificType(EnumCUV.GetOnMovies);
        message.setContents(XMLPhaser.docToString(document));
        message.enPackage(MySKeyFile, sessionKey);
        transceiver.sendMessage(message);
    }

    private void getSeats(TransMessage transMessage) throws Exception {
        Document document = XMLPhaser.stringToDoc(transMessage.getContents());
        Element recvRoot = document.getDocumentElement();
        NodeList nodeList = recvRoot.getChildNodes();
        String o_idStr = null;
        for (int i = 0; i < nodeList.getLength(); i++) {
            Node childNode = nodeList.item(i);
            switch (childNode.getNodeName()) {
                case "o_id" -> o_idStr = childNode.getTextContent();
            }
        }
        ArrayList<Seat> seats = DBCommand.getSeatsByOid(o_idStr);
        document = XMLBuilder.buildXMLDoc();
        Element getElement = document.createElement("get_seats");
        for (Seat temp : seats) {
            Element seatElement = document.createElement("seat");
            Element o_idElement = document.createElement("o_id");
            o_idElement.setTextContent(temp.getOId());
            Element s_idElement = document.createElement("s_id");
            s_idElement.setTextContent(temp.getSId());
            Element s_statusElement = document.createElement("s_status");
            s_statusElement.setTextContent(String.valueOf(temp.getSStatus()));

            seatElement.appendChild(o_idElement);
            seatElement.appendChild(s_idElement);
            seatElement.appendChild(s_statusElement);

            getElement.appendChild(seatElement);
        }
        document.appendChild(getElement);
        //报文初始化
        TransMessage message = new TransMessage();
        message.setToAddress(toAddr);
        message.setFromAddress(fromAddr);
        message.setServiceType(EnumServiceType.CUV);
        message.setSpecificType(EnumCUV.GetSeats);
        message.setContents(XMLPhaser.docToString(document));
        message.enPackage(MySKeyFile, sessionKey);
        transceiver.sendMessage(message);
    }

    private void getTheater(TransMessage transMessage) throws Exception {
        Document document = XMLPhaser.stringToDoc(transMessage.getContents());
        Element recvRoot = document.getDocumentElement();
        NodeList nodeList = recvRoot.getChildNodes();
        String t_idStr = null;
        for (int i = 0; i < nodeList.getLength(); i++) {
            Node childNode = nodeList.item(i);
            switch (childNode.getNodeName()) {
                case "t_id" -> t_idStr = childNode.getTextContent();
            }
        }
        Theater theater = DBCommand.getTheaterById(t_idStr);
        document = XMLBuilder.buildXMLDoc();
        Element getElement = document.createElement("get_theater");
        Element t_idElement = document.createElement("t_id");
        t_idElement.setTextContent(theater.getTId());
        Element t_typeElement = document.createElement("t_type");
        t_typeElement.setTextContent(String.valueOf(theater.getTType()));
        Element t_sizeElement = document.createElement("t_size");
        t_sizeElement.setTextContent(String.valueOf(theater.getTSize()));
        getElement.appendChild(t_idElement);
        getElement.appendChild(t_typeElement);
        getElement.appendChild(t_sizeElement);
        document.appendChild(getElement);
        //报文初始化
        TransMessage message = new TransMessage();
        message.setToAddress(toAddr);
        message.setFromAddress(fromAddr);
        message.setServiceType(EnumServiceType.CUV);
        message.setSpecificType(EnumCUV.GetTheater);
        message.setContents(XMLPhaser.docToString(document));
        message.enPackage(MySKeyFile, sessionKey);
        transceiver.sendMessage(message);
    }

    private void selectSeat(TransMessage transMessage) throws Exception {
        Document document = XMLPhaser.stringToDoc(transMessage.getContents());
        Element recvRoot = document.getDocumentElement();
        NodeList nodeList = recvRoot.getChildNodes();
        String u_idStr = null, o_idStr = null, r_timeStr = null;
        List<String> s_idList = new ArrayList<>();
        for (int i = 0; i < nodeList.getLength(); i++) {
            Node childNode = nodeList.item(i);
            switch (childNode.getNodeName()) {
                case "u_id" -> u_idStr = childNode.getTextContent();
                case "o_id" -> o_idStr = childNode.getTextContent();
                case "r_time" -> r_timeStr = childNode.getTextContent();
                case "s_id" -> s_idList.add(childNode.getTextContent());
            }
        }
        int selectCount = 0;
        for (String tempS_id : s_idList)
            if (DBCommand.getSeatsByOidSid(o_idStr, tempS_id).getSStatus() != EnumSeatStatus.Unselected)
                break;
            else if (DBCommand.updateSeatStatus(o_idStr, tempS_id, EnumSeatStatus.Selecting))
                selectCount++;
        if (selectCount != s_idList.size()) {
            for (int i = 0; i < selectCount; i++)
                DBCommand.updateSeatStatus(o_idStr, s_idList.get(i), EnumSeatStatus.Unselected);
            document = XMLBuilder.buildXMLDoc();
            Element selectElement = document.createElement("select_seat");
            Element stateElement = document.createElement("state");
            stateElement.setTextContent("false");
            selectElement.appendChild(stateElement);
            document.appendChild(selectElement);
        } else {
            for (String tempS_id : s_idList)
                DBCommand.updateSeatStatus(o_idStr, tempS_id, EnumSeatStatus.Selected);
            Record[] records = new Record[selectCount];
            User user = DBCommand.getUserById(u_idStr);
            float price = DBCommand.getOnMovieByOId(o_idStr).getOPrice() * selectCount;
            if (user.getUAccess() == EnumUserAccess.U_VIP)
                price = price * (float) 0.9;
            else if (user.getUAccess() == EnumUserAccess.U_SVIP)
                price = price * (float) 0.8;
            for (int i = 0; i < selectCount; i++) {
                records[i] = new Record();
                records[i].setUId(u_idStr);
                records[i].setOId(o_idStr);
                records[i].setSId(s_idList.get(i));
                records[i].setStatus(EnumRecordStatus.Waiting);
                records[i].setRTime(DatePhaser.dateStrToDate(r_timeStr));
                records[i].setRPrice(price);
                DBCommand.insertRecord(records[i]);
            }
            document = XMLBuilder.buildXMLDoc();
            Element selectElement = document.createElement("select_seat");
            Element stateElement = document.createElement("state");
            stateElement.setTextContent("true");
            selectElement.appendChild(stateElement);
            Element priceElement = document.createElement("price");
            priceElement.setTextContent(String.valueOf(price));
            selectElement.appendChild(priceElement);
            document.appendChild(selectElement);
        }
        TransMessage message = new TransMessage();
        message.setToAddress(toAddr);
        message.setFromAddress(fromAddr);
        message.setServiceType(EnumServiceType.CUV);
        message.setSpecificType(EnumCUV.SelectSeat);
        message.setContents(XMLPhaser.docToString(document));
        message.enPackage(MySKeyFile, sessionKey);
        transceiver.sendMessage(message);
    }

    private void payMoney(TransMessage transMessage) throws Exception {
        Document document = XMLPhaser.stringToDoc(transMessage.getContents());
        Element recvRoot = document.getDocumentElement();
        NodeList nodeList = recvRoot.getChildNodes();
        float money = 0;
        String u_idStr = null, o_idStr = null;
        List<String> s_idList = new ArrayList<>();
        for (int i = 0; i < nodeList.getLength(); i++) {
            Node childNode = nodeList.item(i);
            switch (childNode.getNodeName()) {
                case "u_id" -> u_idStr = childNode.getTextContent();
                case "o_id" -> o_idStr = childNode.getTextContent();
                case "money" -> money = Float.parseFloat(childNode.getTextContent());
                case "s_id" -> s_idList.add(childNode.getTextContent());
            }
        }
        document = XMLBuilder.buildXMLDoc();
        Element payElement = document.createElement("pay_money");
        Element stateElement = document.createElement("state");
        if (DBCommand.payMoney(u_idStr, money)) {
            for (String tempS_id : s_idList) {
                DBCommand.updateSeatStatus(o_idStr, tempS_id, EnumSeatStatus.Unselected);
                DBCommand.updateRecordStatus(u_idStr, tempS_id, o_idStr, EnumRecordStatus.Success);
            }
            stateElement.setTextContent("true");
        } else {
            for (String tempS_id : s_idList)
                DBCommand.updateRecordStatus(u_idStr, tempS_id, o_idStr, EnumRecordStatus.Failed);
            stateElement.setTextContent("false");
        }
        payElement.appendChild(stateElement);
        document.appendChild(payElement);
        TransMessage message = new TransMessage();
        message.setToAddress(toAddr);
        message.setFromAddress(fromAddr);
        message.setServiceType(EnumServiceType.CUV);
        message.setSpecificType(EnumCUV.PayMoney);
        message.setContents(XMLPhaser.docToString(document));
        message.enPackage(MySKeyFile, sessionKey);
        transceiver.sendMessage(message);
    }

    public void payTimeout(TransMessage transMessage) throws Exception {
        Document document = XMLPhaser.stringToDoc(transMessage.getContents());
        Element recvRoot = document.getDocumentElement();
        NodeList nodeList = recvRoot.getChildNodes();
        String u_idStr = null, o_idStr = null;
        List<String> s_idList = new ArrayList<>();
        for (int i = 0; i < nodeList.getLength(); i++) {
            Node childNode = nodeList.item(i);
            switch (childNode.getNodeName()) {
                case "u_id" -> u_idStr = childNode.getTextContent();
                case "o_id" -> o_idStr = childNode.getTextContent();
                case "s_id" -> s_idList.add(childNode.getTextContent());
            }
        }
        document = XMLBuilder.buildXMLDoc();
        Element payElement = document.createElement("pay_timeout");
        Element stateElement = document.createElement("state");
        for (String tempS_id : s_idList) {
            DBCommand.updateSeatStatus(o_idStr, tempS_id, EnumSeatStatus.Unselected);
            DBCommand.updateRecordStatus(u_idStr, tempS_id, o_idStr, EnumRecordStatus.Failed);
        }
        stateElement.setTextContent("true");
        payElement.appendChild(stateElement);
        document.appendChild(payElement);
        TransMessage message = new TransMessage();
        message.setToAddress(toAddr);
        message.setFromAddress(fromAddr);
        message.setServiceType(EnumServiceType.CUV);
        message.setSpecificType(EnumCUV.PayTimeout);
        message.setContents(XMLPhaser.docToString(document));
        message.enPackage(MySKeyFile, sessionKey);
        transceiver.sendMessage(message);
    }
}