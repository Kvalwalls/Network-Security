import java.net.ServerSocket;
import java.net.Socket;

public class ATest {
    public static void main(String[] args) {
        int port = 7000;
        ServerSocket serverSocket = null;
        try {
            serverSocket = new ServerSocket(port);
            Socket socket = serverSocket.accept();

        } catch (Exception exception) {
            exception.printStackTrace();
        }
    }
}
