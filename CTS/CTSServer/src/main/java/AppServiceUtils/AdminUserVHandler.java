package AppServiceUtils;

import DataBaseUtils.DBCommand;
import DataUtils.*;
import DataUtils.Record;
import EnumUtils.*;

import PropertiesUtils.PropertiesHandler;
import TransmissionUtils.*;
import org.w3c.dom.Document;
import org.w3c.dom.Element;
import org.w3c.dom.Node;
import org.w3c.dom.NodeList;


import java.net.Socket;
import java.nio.file.Path;
import java.util.ArrayList;


public class AdminUserVHandler extends VHandler implements Runnable {
    private final String sessionKey = "11111111";
    private static String clientPKeyFile;
    private static byte[] fromAddr;
    private static byte[] toAddr;
    private final Transceiver transceiver;
    private final Socket socket;
    //private String DesKey=null;

    @Override
    public void run() {
        try {
            //while (true)
            //    if (VCertification())
            //        break;
            while (true) {
                TransMessage message = transceiver.receiveMessage();
                toAddr = message.getFromAddress();
                fromAddr = message.getToAddress();
                clientPKeyFile = "src\\resourcesV\\KeyFiles\\"
                        + PropertiesHandler.getElement(AddressPhaser.bytesToString(message.getFromAddress()))
                        + ".pk";
                //message.dePackage(clientPKeyFile, sessionKey);
                if (message.getErrorCode() == EnumErrorCode.Error)
                    throw new Exception("报文错误！");
                switch (message.getSpecificType()) {
                    case EnumAUV.Login: {
                        logIn(message);
                        break;
                    }
                    case EnumAUV.AddUser: {
                        addUser(message);
                        break;
                    }
                    case EnumAUV.DelUser: {
                        deleteUser(message);
                        break;
                    }
                    case EnumAUV.GetUser: {
                        getUser(message);
                        break;
                    }
                    case EnumAUV.AddMovie: {
                        addMovie(message);
                        break;
                    }
                    case EnumAUV.DelMovie: {
                        deleteMovie(message);
                        break;
                    }
                    case EnumAUV.GetMovie: {
                        getMovie(message);
                        break;
                    }
                    case EnumAUV.AddTheater: {
                        addTheater(message);
                        break;
                    }
                    case EnumAUV.DelTheater: {
                        deleteTheater(message);
                        break;
                    }
                    case EnumAUV.GetTheater: {
                        getTheater(message);
                        break;
                    }
                    case EnumAUV.AddOnMovie: {
                        addOnMovie(message);
                        break;
                    }
                    case EnumAUV.DelOnMovie: {
                        deleteOnMovie(message);
                        break;
                    }
                    case EnumAUV.GetOnMovie: {
                        getOnMovie(message);
                        break;
                    }
                    case EnumAUV.GetRecord: {
                        getTicket(message);
                        break;
                    }
                    case EnumAUV.GetMoviePic: {
                        getMoviePictures(message);
                        break;
                    }
                    case EnumAUV.SRMoviePic: {
                        recMoviePicture(message);
                        break;
                    }
                }
            }
        } catch (Exception exception) {
            exception.printStackTrace();
        }
    }

    public AdminUserVHandler(Socket socket) {
        this.transceiver = new Transceiver(socket);
        this.socket = socket;
    }

    public AdminUserVHandler(Socket socket, String desKey) {
        this.transceiver = new Transceiver(socket);
        this.socket = socket;
        //this.DesKey=desKey;
    }

    private byte[] readPicture(Path picName) {
        return null;
    }

    private TransMessage setMessage(TransMessage message, Document document, byte type) throws Exception {
        message.setToAddress(toAddr);
        message.setFromAddress(fromAddr);
        message.setServiceType(type);
        message.setSpecificType(EnumKerberos.Reply);
        message.setContents(XMLPhaser.docToString(document));
        message.enPackage(MyAUVSKeyFile, sessionKey);
        return message;
    }

    private String getContent(TransMessage transMessage) {
        transMessage.dePackage(clientPKeyFile, sessionKey);
        return transMessage.getContents();
    }

    private TransMessage DelOrAddReply(String contents, byte type) throws Exception {
        //构建报文回复内容
        Document document = XMLBuilder.buildXMLDoc();
        //根
        Element root = document.createElement("DelOrAddResult");
        Element status = document.createElement("Status");
        status.setTextContent(contents);
        document.appendChild(root);
        root.appendChild(status);
        //构建报文
        TransMessage message = new TransMessage();
        setMessage(message, document, type);
        return message;
    }

    private void logIn(TransMessage transMessage) {
        try {
            String content = getContent(transMessage);
            Document document = XMLPhaser.stringToDoc(content);
            //获取用户名和密码
            Element root = document.getDocumentElement();
            NodeList nodeList = root.getChildNodes();
            String userid = null;
            String password = null;
            for (int i = 0; i < nodeList.getLength(); i++) {
                Node childNode = nodeList.item(i);
                switch (childNode.getNodeName()) {
                    case "Id" -> userid = childNode.getTextContent();
                    case "Password" -> password = childNode.getTextContent();
                }
            }
            User user = DBCommand.getUserById(userid);
            String[] contents = {"", "", ""};
            //登录失败
            if (user == null) {
                contents[0] = "用户不存在";
            }
            //登陆成功
            else if (user.getUPassword().equals(password)) {
                contents[0] = "登录成功";
                contents[1] = String.valueOf(user.getUAccess());
                contents[2] = String.valueOf(user.getUMoney());
            } else if (user.getUAccess() != 1) {
                contents[0] = "登录成功";
            } else {
                contents[0] = "密码错误";
            }
            transceiver.sendMessage(LoginReply(contents));
        } catch (Exception e) {
            e.printStackTrace();
        }

    }

    private TransMessage LoginReply(String[] contents) throws Exception {
        //构建报文回复内容
        Document document = XMLBuilder.buildXMLDoc();
        //根
        Element root = document.createElement("LoginResult");
        Element status = document.createElement("Status");
        status.setTextContent(contents[0]);
        Element access = document.createElement("Access");
        access.setTextContent(contents[1]);
        Element money = document.createElement("Money");
        money.setTextContent(contents[2]);
        document.appendChild(root);
        root.appendChild(status);
        root.appendChild(access);
        root.appendChild(money);
        //构建报文
        TransMessage message = new TransMessage();
        setMessage(message, document, EnumAUV.Login);
        return message;
    }

//    private void logOut(TransMessage transMessage) throws Exception {
//        TransMessage message = transceiver.receiveMessage();
//        String rsaPKFile = "src\\resourcesAS\\KeyFiles\\"
//                + PropertiesHandler.getPropertiesElement(AddressPhaser.bytesToString(message.getFromAddress()))
//                + ".pk";
//        message.dePackage(rsaPKFile, null);
//        String content=message.getContents();
//
//    }
//
//    private TransMessage LogOutReply(byte[] toAddr, String [] contents,String key_c) throws Exception{
//
//    }

    private void addUser(TransMessage transMessage) throws Exception {
        String con = getContent(transMessage);
        Document document = XMLPhaser.stringToDoc(con);
        Element root = document.getDocumentElement();
        String id = root.getElementsByTagName("Id").item(0).getTextContent();
        String name = root.getElementsByTagName("Name").item(0).getTextContent();
        String password = root.getElementsByTagName("Password").item(0).getTextContent();
        String access = root.getElementsByTagName("Access").item(0).getTextContent();
        String money = root.getElementsByTagName("Money").item(0).getTextContent();
        User user = new User(id, name, password, Byte.parseByte(access), Float.parseFloat(money));
        String status;
        //添加成功
        if (DBCommand.insertUser(user)) {
            status = "添加成功";
        }
        //添加失败
        else {
            status = "添加失败";
        }
        transceiver.sendMessage(addUserReply(status));
    }

    private TransMessage addUserReply(String contents) throws Exception {
        return DelOrAddReply(contents, EnumAUV.AddUser);
    }

    private void deleteUser(TransMessage transMessage) throws Exception {
        String con = getContent(transMessage);
        Document document = XMLPhaser.stringToDoc(con);
        Element root = document.getDocumentElement();
        String id = root.getElementsByTagName("Id").item(0).getTextContent();
        String status;
        //删除成功
        if (DBCommand.deleteUser(id)) {
            status = "删除成功";
        }
        //删除失败
        else {
            status = "删除失败";
        }
        transceiver.sendMessage(deleteUserReply(status));
    }

    private TransMessage deleteUserReply(String contents) throws Exception {
        return DelOrAddReply(contents, EnumAUV.DelUser);
    }

    private void getUser(TransMessage transMessage) throws Exception {
        ArrayList<User> users;
        users = DBCommand.getAllUsers();
        transceiver.sendMessage(getAllUserReply(users));
    }

    private TransMessage getAllUserReply(ArrayList<User> users) throws Exception {
        int userNumb = users.size();
        Document document = XMLBuilder.buildXMLDoc();
        Element root = document.createElement("GetAllUser");
        document.appendChild(root);
        Element[] elements = new Element[userNumb];
        for (int i = 0; i < userNumb; i++) {
            elements[i] = document.createElement("User");
            setUserElement(users.get(i), document, elements[i]);
            root.appendChild(elements[i]);
        }
        TransMessage message = new TransMessage();
        setMessage(message, document, EnumAUV.GetUser);
        return message;
    }

    private static void setUserElement(User user, Document document, Element userElement) {
        Element Id = document.createElement("Id");
        Id.setTextContent(user.getUId());
        Element Name = document.createElement("Name");
        Name.setTextContent(user.getUName());
        Element Password = document.createElement("Password");
        Password.setTextContent(user.getUPassword());
        Element Access = document.createElement("Access");
        Access.setTextContent(String.valueOf(user.getUAccess()));
        userElement.appendChild(Id);
        userElement.appendChild(Name);
        userElement.appendChild(Password);
        userElement.appendChild(Access);
    }

    private void addMovie(TransMessage transMessage) throws Exception {
        String con = getContent(transMessage);
        Document document = XMLPhaser.stringToDoc(con);
        Element root = document.getDocumentElement();
        String id = root.getElementsByTagName("Id").item(0).getTextContent();
        String name = root.getElementsByTagName("Name").item(0).getTextContent();
        String type = root.getElementsByTagName("Type").item(0).getTextContent();
        String time = root.getElementsByTagName("Time").item(0).getTextContent();
        String comment = root.getElementsByTagName("Comment").item(0).getTextContent();
        String description = root.getElementsByTagName("Description").item(0).getTextContent();
        Movie movie = new Movie(id, name, type, Integer.parseInt(time), Float.parseFloat(comment), description);
        String status;
        //添加成功
        if (DBCommand.insertMovie(movie)) {
            status = "添加成功";
        }
        //添加失败
        else {
            status = "添加失败";
        }
        transceiver.sendMessage(addMovieReply(status));
    }

    private TransMessage addMovieReply(String contents) throws Exception {
        return DelOrAddReply(contents, EnumAUV.AddMovie);
    }

    private void deleteMovie(TransMessage transMessage) throws Exception {
        String con = getContent(transMessage);
        Document document = XMLPhaser.stringToDoc(con);
        Element root = document.getDocumentElement();
        String id = root.getElementsByTagName("Id").item(0).getTextContent();
        String status;
        //删除成功
        if (DBCommand.deleteMovie(id)) {
            status = "删除成功";
        }
        //删除失败
        else {
            status = "删除失败";
        }
        transceiver.sendMessage(delMovieReply(status));
    }

    private TransMessage delMovieReply(String contents) throws Exception {
        return DelOrAddReply(contents, EnumAUV.DelMovie);
    }

    private void getMovie(TransMessage transMessage) throws Exception {
        ArrayList<Movie> movies;
        movies = DBCommand.getAllMovies();
        assert movies != null;
        transceiver.sendMessage(getAllMovieReply(movies));
    }

    private TransMessage getAllMovieReply(ArrayList<Movie> movies) throws Exception {
        int movieNumb = movies.size();
        Document document = XMLBuilder.buildXMLDoc();
        Element root = document.createElement("GetAllMovie");
        document.appendChild(root);
        Element[] elements = new Element[movieNumb];
        for (int i = 0; i < movieNumb; i++) {
            elements[i] = document.createElement("Movie");
            setMovieElement(movies.get(i), document, elements[i]);
            root.appendChild(elements[i]);
        }
        TransMessage message = new TransMessage();
        setMessage(message, document, EnumAUV.GetMovie);
        return message;
    }

    private static void setMovieElement(Movie movie, Document document, Element movieElement) {
        Element Id = document.createElement("Id");
        Id.setTextContent(movie.getMId());
        Element Name = document.createElement("Name");
        Name.setTextContent(movie.getMName());
        Element Type = document.createElement("Type");
        Type.setTextContent(movie.getMType());
        Element Time = document.createElement("Time");
        Time.setTextContent(String.valueOf(movie.getMTime()));
        Element Comment = document.createElement("Comment");
        Comment.setTextContent(String.valueOf(movie.getMComment()));
        Element Description = document.createElement("Description");
        Description.setTextContent(movie.getDescription());
        movieElement.appendChild(Id);
        movieElement.appendChild(Name);
        movieElement.appendChild(Type);
        movieElement.appendChild(Time);
        movieElement.appendChild(Comment);
        movieElement.appendChild(Description);
    }

    private void addTheater(TransMessage transMessage) throws Exception {
        String con = getContent(transMessage);
        Document document = XMLPhaser.stringToDoc(con);
        Element root = document.getDocumentElement();
        String id = root.getElementsByTagName("Id").item(0).getTextContent();
        String type = root.getElementsByTagName("Type").item(0).getTextContent();
        String size = root.getElementsByTagName("Size").item(0).getTextContent();
        Theater theater = new Theater(id, Byte.parseByte(type), Integer.parseInt(size));
        String status;
        //添加成功
        if (DBCommand.insertTheater(theater)) {
            status = "添加成功";
        }
        //添加失败
        else {
            status = "添加失败";
        }
        transceiver.sendMessage(addTheaterReply(status));
    }

    private TransMessage addTheaterReply(String contents) throws Exception {
        return DelOrAddReply(contents, EnumAUV.AddTheater);
    }

    private void deleteTheater(TransMessage transMessage) throws Exception {
        String con = getContent(transMessage);
        Document document = XMLPhaser.stringToDoc(con);
        Element root = document.getDocumentElement();
        String id = root.getElementsByTagName("Id").item(0).getTextContent();
        String status;
        //删除成功
        if (DBCommand.deleteTheater(id)) {
            status = "删除成功";
        }
        //删除失败
        else {
            status = "删除失败";
        }
        transceiver.sendMessage(delTheaterReply(status));
    }

    private TransMessage delTheaterReply(String contents) throws Exception {
        return DelOrAddReply(contents, EnumAUV.DelTheater);
    }

    private void getTheater(TransMessage transMessage) throws Exception {
        ArrayList<Theater> theaters;
        theaters = DBCommand.getAllTheaters();
        assert theaters != null;
        transceiver.sendMessage(getAllTheaterReply(theaters));
    }

    private TransMessage getAllTheaterReply(ArrayList<Theater> theaters) throws Exception {
        int theaterNumb = theaters.size();
        Document document = XMLBuilder.buildXMLDoc();
        Element root = document.createElement("GetAllTheater");
        document.appendChild(root);
        Element[] elements = new Element[theaterNumb];
        for (int i = 0; i < theaterNumb; i++) {
            elements[i] = document.createElement("Theater");
            setTheaterElement(theaters.get(i), document, elements[i]);
            root.appendChild(elements[i]);
        }
        TransMessage message = new TransMessage();
        setMessage(message, document, EnumAUV.GetTheater);
        return message;
    }

    private static void setTheaterElement(Theater theater, Document document, Element theaterElement) {
        Element Id = document.createElement("Id");
        Id.setTextContent(theater.getTId());
        Element Type = document.createElement("Type");
        Type.setTextContent(String.valueOf(theater.getTType()));
        Element Size = document.createElement("Size");
        Size.setTextContent(String.valueOf(theater.getTSize()));
        theaterElement.appendChild(Id);
        theaterElement.appendChild(Type);
        theaterElement.appendChild(Size);
    }

    private void addOnMovie(TransMessage transMessage) throws Exception {
        String con = getContent(transMessage);
        Document document = XMLPhaser.stringToDoc(con);
        Element root = document.getDocumentElement();

        String oid = root.getElementsByTagName("Oid").item(0).getTextContent();
        String mid = root.getElementsByTagName("Mid").item(0).getTextContent();
        String tid = root.getElementsByTagName("Tid").item(0).getTextContent();
        String startTime = root.getElementsByTagName("Obegin").item(0).getTextContent();
        String endTime = root.getElementsByTagName("Oend").item(0).getTextContent();
        String price = root.getElementsByTagName("Oprice").item(0).getTextContent();

        OnMovie onMovie = new OnMovie(oid, mid, tid, DatePhaser.dateStrToDate(startTime), DatePhaser.dateStrToDate(endTime), Float.parseFloat(price));
        String status;
        //添加成功
        if (DBCommand.insertOnMovie(onMovie)) {
            status = "添加成功";
        }
        //添加失败
        else {
            status = "添加失败";
        }
        transceiver.sendMessage(addTheaterReply(status));
    }

    private TransMessage addOnMovieReply(String contents) throws Exception {
        return DelOrAddReply(contents, EnumAUV.AddOnMovie);
    }

    private void deleteOnMovie(TransMessage transMessage) throws Exception {
        String con = getContent(transMessage);
        Document document = XMLPhaser.stringToDoc(con);
        Element root = document.getDocumentElement();
        String id = root.getElementsByTagName("Oid").item(0).getTextContent();
        String status;
        //删除成功
        if (DBCommand.deleteOnMovie(id)) {
            status = "删除成功";
        }
        //删除失败
        else {
            status = "删除失败";
        }
        transceiver.sendMessage(delOnMovieReply(status));
    }

    private TransMessage delOnMovieReply(String contents) throws Exception {
        return DelOrAddReply(contents, EnumAUV.DelOnMovie);
    }

    private void getOnMovie(TransMessage transMessage) throws Exception {
        ArrayList<OnMovie> onMovies;
        onMovies = DBCommand.getAllOnMovies();
        assert onMovies != null;
        transceiver.sendMessage(getAllOnMovieReply(onMovies));
    }

    private TransMessage getAllOnMovieReply(ArrayList<OnMovie> onMovies) throws Exception {
        int onMovieNumb = onMovies.size();
        Document document = XMLBuilder.buildXMLDoc();
        Element root = document.createElement("GetAllOnMovie");
        document.appendChild(root);
        Element[] elements = new Element[onMovieNumb];
        for (int i = 0; i < onMovieNumb; i++) {
            elements[i] = document.createElement("OnMovie");
            setOnMovieElement(onMovies.get(i), document, elements[i]);
            root.appendChild(elements[i]);
        }
        TransMessage message = new TransMessage();
        setMessage(message, document, EnumAUV.GetOnMovie);
        return message;
    }

    private static void setOnMovieElement(OnMovie onMovie, Document document, Element onMovieElement) {
        Element OId = document.createElement("Oid");
        OId.setTextContent(onMovie.getOId());
        Element MId = document.createElement("Mid");
        MId.setTextContent(onMovie.getMId());
        Element TId = document.createElement("Tid");
        TId.setTextContent(onMovie.getTId());
        Element startTime = document.createElement("Obegin");
        startTime.setTextContent(DatePhaser.dateToDateStr(onMovie.getOBegin()));
        Element endTime = document.createElement("Oend");
        endTime.setTextContent(DatePhaser.dateToDateStr(onMovie.getOEnd()));
        Element Price = document.createElement("Oprice");
        Price.setTextContent(String.valueOf(onMovie.getOPrice()));
        //构建树形结构
        onMovieElement.appendChild(OId);
        onMovieElement.appendChild(MId);
        onMovieElement.appendChild(TId);
        onMovieElement.appendChild(startTime);
        onMovieElement.appendChild(endTime);
        onMovieElement.appendChild(Price);
    }

    private void getTicket(TransMessage transMessage) throws Exception {
        ArrayList<Record> record;
        record = DBCommand.getAllRecord();
        assert record != null;
        transceiver.sendMessage(getAllTicketReply(record));
    }

    private TransMessage getAllTicketReply(ArrayList<Record> records) throws Exception {
        int recordNumb = records.size();
        Document document = XMLBuilder.buildXMLDoc();
        Element root = document.createElement("GetAllTicket");
        document.appendChild(root);
        Element[] elements = new Element[recordNumb];
        for (int i = 0; i < recordNumb; i++) {
            elements[i] = document.createElement("Ticket");
            setTicketElement(records.get(i), document, elements[i]);
            root.appendChild(elements[i]);
        }
        TransMessage message = new TransMessage();
        setMessage(message, document, EnumAUV.GetRecord);
        return message;
    }

    private static void setTicketElement(Record record, Document document, Element recordElement) {
        Element Uid = document.createElement("Uid");
        Uid.setTextContent(record.getUId());
        Element Sid = document.createElement("Sid");
        Sid.setTextContent(record.getSId());
        Element Oid = document.createElement("Oid");
        Oid.setTextContent(record.getOId());
        Element Rtime = document.createElement("Rtime");
        Rtime.setTextContent(DatePhaser.dateToDateStr(record.getRTime()));
        Element Rprice = document.createElement("Rprice");
        Rprice.setTextContent(String.valueOf(record.getRPrice()));
        Element Rstatus = document.createElement("Rstatus");
        Rstatus.setTextContent(String.valueOf(record.getStatus()));
        //构建树形结构
        recordElement.appendChild(Uid);
        recordElement.appendChild(Sid);
        recordElement.appendChild(Oid);
        recordElement.appendChild(Rtime);
        recordElement.appendChild(Rprice);
        recordElement.appendChild(Rstatus);
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
        message.setServiceType(EnumServiceType.AUV);
        message.setSpecificType(EnumAUV.GetMoviePic);
        message.setContents(XMLPhaser.docToString(document));
        message.enPackage(MyAUVSKeyFile, sessionKey);
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

    private void recMoviePicture(TransMessage transMessage) throws Exception {
        String con = getContent(transMessage);
        Document document = XMLPhaser.stringToDoc(con);
        Element root = document.getDocumentElement();
        String mid = root.getElementsByTagName("Mid").item(0).getTextContent();
        String data = root.getElementsByTagName("Picture").item(0).getTextContent();
        String moviePicName = "src\\resourcesV\\MoviePictures\\" + mid + ".jpg";
        PicturePhaser.base64ToPicture(data,moviePicName);
    }
}