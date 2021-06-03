package AppServiceUtils;

import DataBaseUtils.DBCommand;
import DataUtils.*;
import DataUtils.Record;
import EnumUtils.EnumKerberos;
import EnumUtils.EnumServiceType;

import PropertiesUtils.PropertiesHandler;
import TransmissionUtils.*;
import org.w3c.dom.Document;
import org.w3c.dom.Element;


import java.net.Socket;
import java.nio.file.Path;
import java.util.ArrayList;


public class AdminUserVHandler extends VHandler implements Runnable {
    private final Transceiver transceiver;
    private final Socket socket;
    private String DesKey=null;

    @Override
    public void run() {

    }
    public AdminUserVHandler(Socket socket) {
        this.transceiver = new Transceiver(socket);
        this.socket=socket;
    }

    public AdminUserVHandler(Socket socket,String desKey) {
        this.transceiver = new Transceiver(socket);
        this.socket=socket;
        this.DesKey=desKey;
    }

    private byte[] readPicture(Path picName) {
        return null;
    }

    private TransMessage setMessage(byte[] toAddr,String key_c,TransMessage message,Document document) throws Exception {
        message.setToAddress(toAddr);
        message.setFromAddress(AddressPhaser.stringToBytes(
                PropertiesHandler.getElement("My_IPAddress")));
        message.setServiceType(EnumServiceType.AUV);
        message.setSpecificType(EnumKerberos.Reply);
        message.setContents(XMLPhaser.docToString(document));
        message.enPackage("src\\resourcesAS\\KeyFiles\\AS.sk", key_c);
        return message;
    }

    private String getContent(TransMessage transMessage){
        String rsaPKFile = "src\\resourcesAS\\KeyFiles\\"
                + PropertiesHandler.getElement(AddressPhaser.bytesToString(transMessage.getFromAddress()))
                + ".pk";
        transMessage.dePackage(rsaPKFile, null);
        return transMessage.getContents();
    }

    private TransMessage DelOrAddReply(byte[] toAddr, String contents,String key_c) throws Exception{
        //构建报文回复内容
        Document document = XMLBuilder.buildXMLDoc();
        //根
        Element root = document.createElement("addUserResult");
        Element status=document.createElement("Status");
        status.setTextContent(contents);
        document.appendChild(root);
        root.appendChild(status);
        //构建报文
        TransMessage message = new TransMessage();
        setMessage(toAddr,key_c,message,document);
        return message;
    }

    private void logIn(TransMessage transMessage) {
        try {
            String content=getContent(transMessage);
            Document document= XMLPhaser.stringToDoc(content);
            //获取用户名和密码
            Element root=document.getDocumentElement();
            String userid=root.getElementsByTagName("User").item(0).getTextContent();
            String password=root.getElementsByTagName("Password").item(0).getTextContent();
            User user=DBCommand.getUserById(userid);
            String []contents={"","",""};
            //登录失败
            if(user==null){
                contents[0]="用户不存在";
            }
            //登陆成功
            else if(user.getUPassword().equals(password)){
                contents[0]="登录成功";
                contents[1]= String.valueOf(user.getUAccess());
                contents[2]= String.valueOf(user.getUMoney());
            }
            else {
                contents[0]="密码错误";
            }
            transceiver.sendMessage(LoginReply(transMessage.getFromAddress(),contents,DesKey));
        }
        catch (Exception e) {
            e.printStackTrace();
        }

    }

    private TransMessage LoginReply(byte[] toAddr, String [] contents,String key_c) throws Exception{
        //构建报文回复内容
        Document document = XMLBuilder.buildXMLDoc();
        //根
        Element root = document.createElement("LoginResult");
        Element status=document.createElement("Status");
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
        setMessage(toAddr,key_c,message,document);
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
        String con=getContent(transMessage);
        Document document= XMLPhaser.stringToDoc(con);
        Element root=document.getDocumentElement();
        String id=root.getElementsByTagName("Id").item(0).getTextContent();
        String name=root.getElementsByTagName("Name").item(0).getTextContent();
        String password=root.getElementsByTagName("Password").item(0).getTextContent();
        String access=root.getElementsByTagName("Access").item(0).getTextContent();
        String money=root.getElementsByTagName("Money").item(0).getTextContent();
        User user=new User(id,name,password,Byte.parseByte(access),Float.parseFloat(money));
        String status;
        //添加成功
        if(DBCommand.insertUser(user)){
            status="添加成功";
        }
        //添加失败
        else {
            status="添加失败";
        }
            transceiver.sendMessage(addUserReply(transMessage.getFromAddress(),status,DesKey));
    }

    private TransMessage addUserReply(byte[] toAddr, String contents,String key_c) throws Exception {
        return DelOrAddReply(toAddr,contents,key_c);
    }

    private void deleteUser(TransMessage transMessage) throws Exception {
        String con=getContent(transMessage);
        Document document= XMLPhaser.stringToDoc(con);
        Element root=document.getDocumentElement();
        String id=root.getElementsByTagName("Id").item(0).getTextContent();
        String status;
        //删除成功
        if(DBCommand.deleteUser(id)){
            status="删除成功";
        }
        //删除失败
        else {
            status="删除失败";
        }
        transceiver.sendMessage(deleteUserReply(transMessage.getFromAddress(),status,DesKey));
    }

    private TransMessage deleteUserReply(byte[] toAddr, String contents,String key_c) throws Exception {
        return DelOrAddReply(toAddr,contents,key_c);
    }

    private void getUser(TransMessage transMessage) throws Exception {
        ArrayList<User> users;
        users=DBCommand.getAllUsers();
        transceiver.sendMessage(getAllUserReply(transMessage.getFromAddress(),users,DesKey));
    }

    private TransMessage getAllUserReply(byte[] toAddr, ArrayList<User> users,String key_c) throws Exception{
        int userNumb=users.size();
        Document document = XMLBuilder.buildXMLDoc();
        Element root=document.createElement("GetAllUser");
        document.appendChild(root);
        Element[] elements=new Element[userNumb];
        for(int i=0;i<userNumb;i++){
            elements[i]=document.createElement("User");
            setUserElement(users.get(i),document,elements[i]);
            root.appendChild(elements[i]);
        }
        TransMessage message = new TransMessage();
        setMessage(toAddr,key_c,message,document);
        return message;
    }


    private static void setUserElement(User user,Document document,Element userElement){
        Element Id=document.createElement("Id");
        Id.setTextContent(user.getUId());
        Element Name=document.createElement("Name");
        Name.setTextContent(user.getUName());
        Element Password=document.createElement("Password");
        Password.setTextContent(user.getUPassword());
        Element Access=document.createElement("Access");
        Access.setTextContent(String.valueOf(user.getUAccess()));
        userElement.appendChild(Id);
        userElement.appendChild(Name);
        userElement.appendChild(Password);
        userElement.appendChild(Access);
    }

    private void addMovie(TransMessage transMessage) throws Exception {
        String con=getContent(transMessage);
        Document document= XMLPhaser.stringToDoc(con);
        Element root=document.getDocumentElement();
        String id=root.getElementsByTagName("Id").item(0).getTextContent();
        String name=root.getElementsByTagName("Name").item(0).getTextContent();
        String type=root.getElementsByTagName("Type").item(0).getTextContent();
        String time=root.getElementsByTagName("Time").item(0).getTextContent();
        String comment=root.getElementsByTagName("Comment").item(0).getTextContent();
        String description=root.getElementsByTagName("Description").item(0).getTextContent();
        Movie movie=new Movie(id,name,type,Integer.parseInt(time),Float.parseFloat(comment),description);
        String status;
        //添加成功
        if(DBCommand.insertMovie(movie)){
            status="添加成功";
        }
        //添加失败
        else {
            status="添加失败";
        }
        transceiver.sendMessage(addMovieReply(transMessage.getFromAddress(),status,DesKey));
    }

    private TransMessage addMovieReply(byte[] toAddr, String contents,String key_c) throws Exception{
        return DelOrAddReply(toAddr,contents,key_c);
    }

    private void deleteMovie(TransMessage transMessage) throws Exception {
        String con=getContent(transMessage);
        Document document= XMLPhaser.stringToDoc(con);
        Element root=document.getDocumentElement();
        String id=root.getElementsByTagName("Id").item(0).getTextContent();
        String status;
        //删除成功
        if(DBCommand.deleteMovie(id)){
            status="删除成功";
        }
        //删除失败
        else {
            status="删除失败";
        }
        transceiver.sendMessage(delMovieReply(transMessage.getFromAddress(),status,DesKey));
    }

    private TransMessage delMovieReply(byte[] toAddr, String contents,String key_c) throws Exception{
        return DelOrAddReply(toAddr,contents,key_c);
    }

    private void getMovie(TransMessage transMessage) throws Exception {
        ArrayList<Movie> movies;
        movies=DBCommand.getAllMovies();
        assert movies != null;
        transceiver.sendMessage(getAllMovieReply(transMessage.getFromAddress(),movies,DesKey));
    }

    private TransMessage getAllMovieReply(byte[] toAddr, ArrayList<Movie> movies, String key_c) throws Exception{
        int movieNumb=movies.size();
        Document document = XMLBuilder.buildXMLDoc();
        Element root=document.createElement("GetAllMovie");
        document.appendChild(root);
        Element[] elements=new Element[movieNumb];
        for(int i=0;i<movieNumb;i++){
            elements[i]=document.createElement("Movie");
            setMovieElement(movies.get(i),document,elements[i]);
            root.appendChild(elements[i]);
        }
        TransMessage message = new TransMessage();
        setMessage(toAddr,key_c,message,document);
        return message;
    }
    private static void setMovieElement(Movie movie,Document document,Element movieElement){
        Element Id=document.createElement("Id");
        Id.setTextContent(movie.getMId());
        Element Name=document.createElement("Name");
        Name.setTextContent(movie.getMName());
        Element Type=document.createElement("Type");
        Type.setTextContent(movie.getMType());
        Element Time=document.createElement("Time");
        Time.setTextContent(String.valueOf(movie.getMTime()));
        Element Comment=document.createElement("Comment");
        Comment.setTextContent(String.valueOf(movie.getMComment()));
        Element Description=document.createElement("Description");
        Description.setTextContent(movie.getDescription());
        movieElement.appendChild(Id);
        movieElement.appendChild(Name);
        movieElement.appendChild(Type);
        movieElement.appendChild(Time);
        movieElement.appendChild(Comment);
        movieElement.appendChild(Description);
    }


    private void addTheater(TransMessage transMessage) throws Exception {
        String con=getContent(transMessage);
        Document document= XMLPhaser.stringToDoc(con);
        Element root=document.getDocumentElement();
        String id=root.getElementsByTagName("Id").item(0).getTextContent();
        String type=root.getElementsByTagName("Type").item(0).getTextContent();
        String size=root.getElementsByTagName("Size").item(0).getTextContent();
        Theater theater=new Theater(id,Byte.parseByte(type),Integer.parseInt(size));
        String status;
        //添加成功
        if(DBCommand.insertTheater(theater)){
            status="添加成功";
        }
        //添加失败
        else {
            status="添加失败";
        }
        transceiver.sendMessage(addTheaterReply(transMessage.getFromAddress(),status,DesKey));
    }

    private TransMessage addTheaterReply(byte[] toAddr, String contents,String key_c) throws Exception{
        return DelOrAddReply(toAddr,contents,key_c);
    }

    private void deleteTheater(TransMessage transMessage) throws Exception {
        String con=getContent(transMessage);
        Document document= XMLPhaser.stringToDoc(con);
        Element root=document.getDocumentElement();
        String id=root.getElementsByTagName("Id").item(0).getTextContent();
        String status;
        //删除成功
        if(DBCommand.deleteTheater(id)){
            status="删除成功";
        }
        //删除失败
        else {
            status="删除失败";
        }
        transceiver.sendMessage(delTheaterReply(transMessage.getFromAddress(),status,DesKey));
    }

    private TransMessage delTheaterReply(byte[] toAddr, String contents,String key_c) throws Exception{
        return DelOrAddReply(toAddr,contents,key_c);
    }

    private void getTheater(TransMessage transMessage) throws Exception {
        ArrayList<Theater> theaters;
        theaters=DBCommand.getAllTheaters();
        assert theaters != null;
        transceiver.sendMessage(getAllTheaterReply(transMessage.getFromAddress(),theaters,DesKey));
    }

    private TransMessage getAllTheaterReply(byte[] toAddr, ArrayList<Theater> theaters, String key_c) throws Exception{
        int theaterNumb=theaters.size();
        Document document = XMLBuilder.buildXMLDoc();
        Element root=document.createElement("GetAllTheater");
        document.appendChild(root);
        Element[] elements=new Element[theaterNumb];
        for(int i=0;i<theaterNumb;i++){
            elements[i]=document.createElement("Theater");
            setTheaterElement(theaters.get(i),document,elements[i]);
            root.appendChild(elements[i]);
        }
        TransMessage message = new TransMessage();
        setMessage(toAddr,key_c,message,document);
        return message;
    }

    private static void setTheaterElement(Theater theater,Document document,Element theaterElement){
        Element Id=document.createElement("Id");
        Id.setTextContent(theater.getTId());
        Element Type=document.createElement("Type");
        Type.setTextContent(String.valueOf(theater.getTType()));
        Element Size=document.createElement("Size");
        Size.setTextContent(String.valueOf(theater.getTSize()));
        theaterElement.appendChild(Id);
        theaterElement.appendChild(Type);
        theaterElement.appendChild(Size);
    }

    private void addOnMovie(TransMessage transMessage) throws Exception {
        String con=getContent(transMessage);
        Document document= XMLPhaser.stringToDoc(con);
        Element root=document.getDocumentElement();
        
        String oid=root.getElementsByTagName("Oid").item(0).getTextContent();
        String mid=root.getElementsByTagName("Mid").item(0).getTextContent();
        String tid=root.getElementsByTagName("Tid").item(0).getTextContent();
        String startTime=root.getElementsByTagName("Obegin").item(0).getTextContent();
        String endTime=root.getElementsByTagName("Oend").item(0).getTextContent();
        String price =root.getElementsByTagName("Oprice").item(0).getTextContent();
        
        OnMovie onMovie=new OnMovie(oid,mid,tid,DatePhaser.dateStrToDate(startTime),DatePhaser.dateStrToDate(endTime),Float.parseFloat(price));
        String status;
        //添加成功
        if(DBCommand.insertOnMovie(onMovie)){
            status="添加成功";
        }
        //添加失败
        else {
            status="添加失败";
        }
        transceiver.sendMessage(addTheaterReply(transMessage.getFromAddress(),status,DesKey));
    }

    private TransMessage addOnMovieReply(byte[] toAddr, String contents,String key_c) throws Exception{
        return DelOrAddReply(toAddr,contents,key_c);
    }

    private void deleteOnMovie(TransMessage transMessage) throws Exception {
        String con=getContent(transMessage);
        Document document= XMLPhaser.stringToDoc(con);
        Element root=document.getDocumentElement();
        String id=root.getElementsByTagName("Id").item(0).getTextContent();
        String status;
        //删除成功
        if(DBCommand.deleteOnMovie(id)){
            status="删除成功";
        }
        //删除失败
        else {
            status="删除失败";
        }
        transceiver.sendMessage(delOnMovieReply(transMessage.getFromAddress(),status,DesKey));
    }

    private TransMessage delOnMovieReply(byte[] toAddr, String contents,String key_c) throws Exception{
        return DelOrAddReply(toAddr,contents,key_c);
    }

    private void getOnMovie(TransMessage transMessage) throws Exception {
        ArrayList<OnMovie> onMovies;
        onMovies=DBCommand.getAllOnMovies();
        assert onMovies != null;
        transceiver.sendMessage(getAllOnMovieReply(transMessage.getFromAddress(),onMovies,DesKey));
    }

    private TransMessage getAllOnMovieReply(byte[] toAddr, ArrayList<OnMovie> onMovies, String key_c) throws Exception{
        int onMovieNumb=onMovies.size();
        Document document = XMLBuilder.buildXMLDoc();
        Element root=document.createElement("GetAllOnMovie");
        document.appendChild(root);
        Element[] elements=new Element[onMovieNumb];
        for(int i=0;i<onMovieNumb;i++){
            elements[i]=document.createElement("OnMovie");
            setOnMovieElement(onMovies.get(i),document,elements[i]);
            root.appendChild(elements[i]);
        }
        TransMessage message = new TransMessage();
        setMessage(toAddr,key_c,message,document);
        return message;
    }

    private static void setOnMovieElement(OnMovie onMovie,Document document,Element onMovieElement){
        Element OId=document.createElement("Oid");
        OId.setTextContent(onMovie.getOId());
        Element MId=document.createElement("Mid");
        MId.setTextContent(onMovie.getMId());
        Element TId=document.createElement("Tid");
        TId.setTextContent(onMovie.getTId());
        Element startTime=document.createElement("Obegin");
        startTime.setTextContent(DatePhaser.dateToDateStr(onMovie.getOBegin()));
        Element endTime=document.createElement("Oend");
        endTime.setTextContent(DatePhaser.dateToDateStr(onMovie.getOEnd()));
        Element Price=document.createElement("Oprice");
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
        record=DBCommand.getAllRecord();
        transceiver.sendMessage(getAllTicketReply(transMessage.getFromAddress(),record,DesKey));
    }
    private TransMessage getAllTicketReply(byte[] toAddr, ArrayList<Record> records, String key_c) throws Exception{
        int recordNumb=records.size();
        Document document = XMLBuilder.buildXMLDoc();
        Element root=document.createElement("GetAllTicket");
        document.appendChild(root);
        Element[] elements=new Element[recordNumb];
        for(int i=0;i<recordNumb;i++){
            elements[i]=document.createElement("Ticket");
            setTicketElement(records.get(i),document,elements[i]);
            root.appendChild(elements[i]);
        }
        TransMessage message = new TransMessage();
        setMessage(toAddr,key_c,message,document);
        return message;
    }

    private static void setTicketElement(Record record,Document document,Element recordElement){
        Element Uid=document.createElement("Uid");
        Uid.setTextContent(record.getUId());
        Element Sid=document.createElement("Sid");
        Sid.setTextContent(record.getSId());
        Element Oid=document.createElement("Oid");
        Oid.setTextContent(record.getOId());
        Element Rtime=document.createElement("Rtime");
        Rtime.setTextContent(DatePhaser.dateToDateStr(record.getRTime()));
        Element Rprice=document.createElement("Rprice");
        Rprice.setTextContent(String.valueOf(record.getRPrice()));
        Element Rstatus=document.createElement("Rstatus");
        Rstatus.setTextContent(String.valueOf(record.getStatus()));
        //构建树形结构
        recordElement.appendChild(Uid);
        recordElement.appendChild(Sid);
        recordElement.appendChild(Oid);
        recordElement.appendChild(Rtime);
        recordElement.appendChild(Rprice);
        recordElement.appendChild(Rstatus);
    }
}