import DataBaseUtils.DBCommand;
import DataUtils.Movie;
import DataUtils.OnMovie;
import DataUtils.Record;
import DataUtils.Theater;
import DataUtils.User;
import EnumUtils.EnumRecordStatus;
import EnumUtils.EnumSeatStatus;
import EnumUtils.EnumTheaterType;
import EnumUtils.EnumUserAccess;
import KerberosUtils.Tools;
import SecurityUtils.RSAHandler;
import TransmissionUtils.Connection;
import TransmissionUtils.DatePhaser;
import TransmissionUtils.TransMessage;
import TransmissionUtils.Transceiver;

import java.net.ServerSocket;
import java.net.Socket;
import java.text.SimpleDateFormat;
import java.time.LocalDate;
import java.time.format.DateTimeFormatter;
import java.util.ArrayList;
import java.util.Date;

public class ATest {
    public static void main(String[] args) {
        try {
            ServerSocket serverSocket = Connection.bindServer("127.0.0.1",7000);
            Socket socket = serverSocket.accept();
            Transceiver transceiver = new Transceiver(socket);
            TransMessage message = new TransMessage();
            message.setFromAddress(new byte[]{127,0,0,1});
            message.setToAddress(new byte[]{127,0,0,2});
            message.setServiceType((byte) 0);
            message.setSpecificType((byte) 0);
            message.setCryptCode((byte) 1);
            message.setContents("123");
            message.enPackage("src\\resourcesAS\\KeyFiles\\AS.sk","00000000");
            transceiver.sendMessage(message);
        } catch (Exception exception) {
            exception.printStackTrace();
        }
    }
}
//XML测试
/*ServerSocket serverSocket = Connection.bindServer("127.0.0.1",7000);
            Socket socket = serverSocket.accept();
            Transceiver transceiver = new Transceiver(socket);
            TransMessage message = transceiver.receiveMessage();
            message.dePackage("C:\\Users\\19705\\Desktop\\test.pk","00000001");
            Document document = XMLPhaser.StringToXml(message.getContents());
            System.out.println(document.getFirstChild().getFirstChild().getTextContent());*/
//报文收发测试
/*TransMessage transMessage = new TransMessage();
            transMessage.setFromAddress(new byte[]{127,0,0,1});
            transMessage.setToAddress(new byte[]{127,0,0,2});
            transMessage.setCryptCode((byte) 1);
            transMessage.setServiceType((byte) 0);
            transMessage.setSpecificType((byte) 0);
            transMessage.setContents("汪帮传是天才1汪帮传是天才2汪帮传是天才3汪帮传是天才4汪帮传是天才5汪帮传是天才6汪帮传是天才7汪帮传是天才8汪帮传是天才9汪帮传是天才10汪帮传是天才11汪帮传是天才12汪帮传是天才13汪帮传是天才14汪帮传是天才15汪帮传是天才17汪帮传是天才18汪帮传是天才19汪帮传是天才20汪帮传是天才21汪帮传是天才22汪帮传是天才23汪帮传是天才24汪帮传是天才25");
            transMessage.enPackage("C:\\Users\\19705\\Desktop\\test.sk","00000000");
            transceiver.sendMessage(transMessage);*/
/*ServerSocket serverSocket = Connection.bindServer("127.0.0.1",7000);
            Socket socket = serverSocket.accept();
            Transceiver transceiver = new Transceiver(socket);
            TransMessage message = new TransMessage();
            message.setFromAddress(new byte[]{127,0,0,1});
            message.setToAddress(new byte[]{127,0,0,2});
            message.setServiceType((byte) 0);
            message.setSpecificType((byte) 0);
            message.setCryptCode((byte) 1);
            message.setContents(PicturePhaser.pictureToBase64("C:\\Users\\19705\\Desktop\\M00001.jpg"));
            message.enPackage("src\\resources\\KeyFiles\\Client.sk","00000000");
            transceiver.sendMessage(message);
            String temp = PicturePhaser.pictureToBase64("C:\\Users\\19705\\Desktop\\M00001.jpg");
            PicturePhaser.base64ToPicture(temp,"0001");*/
/*byte[] temp = IntBytesPhaser.intToBytes(123456);
            int num = IntBytesPhaser.bytesToInt(temp);
            System.out.println(num);*/

/*ServerSocket serverSocket = Connection.bindServer("127.0.0.1",7000);
            Socket socket = serverSocket.accept();
            Transceiver transceiver = new Transceiver(socket);
            TransMessage transMessage = new TransMessage();
            transMessage.setFromAddress(new byte[]{127,0,0,1});
            transMessage.setToAddress(new byte[]{127,0,0,2});
            transMessage.setCryptCode((byte) 1);
            transMessage.setServiceType((byte) 0);
            transMessage.setSpecificType((byte) 0);
            transMessage.setContents("");
            transMessage.enPackage("src\\resourcesAS\\KeyFiles\\AS.sk","00000000");
            System.out.println(transMessage.getSignature());
            transceiver.sendMessage(transMessage);*/

/*            User user = new User();

            user.setUId("U00002");
            user.setUName("李正浩");
            user.setUPassword("lzh123456789");
            user.setUAccess(EnumUserAccess.U_VIP);
            user.setUMoney(1000);
            DBCommand.insertUser(user);

            ArrayList<User> temp = DBCommand.getAllUsers();
            temp.forEach((User u) -> System.out.println(u.getUName()));

            System.out.println(DBCommand.getUserById("U00002").getUName());
            System.out.println(DBCommand.findPassword("U00002", "李正浩"));
            DBCommand.updateAccess("U00002", EnumUserAccess.U_SVIP);
            DBCommand.updateUName("U00002", "lzh");
            DBCommand.updatePassword("U00002", "lzh123");
            if (DBCommand.payMoney("U00002", (float) 10001))
                System.out.println("OK");
            System.out.println(DBCommand.fundMoney("U00002", (float) 10000));
            System.out.println(DBCommand.payMoney("U00002", (float) 10001));
            System.out.println(DBCommand.getUserById("U00002").getUName());
            System.out.println(DBCommand.getUserById("U00002").getUMoney());
            System.out.println(DBCommand.getUserById("U00002").getUAccess());
            System.out.println(DBCommand.getUserById("U00002").getUPassword());
            DBCommand.deleteUser("U00002");
            ArrayList<User> da = DBCommand.getAllUsers();
            da.forEach((User u) -> System.out.println(u.getUName()));*/

/*Theater t = new Theater("T00001",EnumTheaterType.Comm,64);
            Movie m = new Movie("M00001","我","WWW",120,10,"123456");
            DBCommand.insertMovie(m);
            DBCommand.insertTheater(t);
            OnMovie o = new OnMovie();
            Date date = DatePhaser.addDateMinutes(new Date(),1000);
            o.setMId("M00001");
            o.setOId("O00000");
            o.setOPrice(35);
            o.setTId("T00001");
            o.setOBegin(date);
            o.setOEnd(DatePhaser.addDateMinutes(date,100));
            System.out.println(DBCommand.insertOnMovie(o));*/