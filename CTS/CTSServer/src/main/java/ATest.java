import TransmissionUtils.Connection;
import TransmissionUtils.TransMessage;
import TransmissionUtils.Transceiver;
import TransmissionUtils.XMLPhaser;
import org.w3c.dom.Document;

import java.net.ServerSocket;
import java.net.Socket;

public class ATest {
    public static void main(String[] args) {
        try {
            ServerSocket serverSocket = Connection.bindServer("127.0.0.1",7000);
            Socket socket = serverSocket.accept();
            Transceiver transceiver = new Transceiver(socket);
            TransMessage message = transceiver.receiveMessage();
            message.dePackage("C:\\Users\\19705\\Desktop\\test.pk","00000001");
            Document document = XMLPhaser.StringToXml(message.getContents());
            System.out.println(document.getFirstChild().getFirstChild().getTextContent());
        } catch (Exception exception) {
            exception.printStackTrace();
        }
    }
}
/*TransMessage transMessage = new TransMessage();
            transMessage.setFromAddress(new byte[]{127,0,0,1});
            transMessage.setToAddress(new byte[]{127,0,0,2});
            transMessage.setCryptCode((byte) 1);
            transMessage.setServiceType((byte) 0);
            transMessage.setSpecificType((byte) 0);
            transMessage.setContents("汪帮传是天才1汪帮传是天才2汪帮传是天才3汪帮传是天才4汪帮传是天才5汪帮传是天才6汪帮传是天才7汪帮传是天才8汪帮传是天才9汪帮传是天才10汪帮传是天才11汪帮传是天才12汪帮传是天才13汪帮传是天才14汪帮传是天才15汪帮传是天才17汪帮传是天才18汪帮传是天才19汪帮传是天才20汪帮传是天才21汪帮传是天才22汪帮传是天才23汪帮传是天才24汪帮传是天才25");
            transMessage.enPackage("C:\\Users\\19705\\Desktop\\test.sk","00000000");
            transceiver.sendMessage(transMessage);*/
