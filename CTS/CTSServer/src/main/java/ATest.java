import TransmissionUtils.*;
import org.w3c.dom.Document;

import java.net.ServerSocket;
import java.net.Socket;

public class ATest {
    public static void main(String[] args) {
        try {

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