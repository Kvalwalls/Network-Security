import KerberosUtils.ASHandler;

import java.io.IOException;
import java.net.ServerSocket;
import java.net.Socket;

public class K_ASStarter {
    private static final int PORT = 8081;

    public static void main(String[] args) {
        ServerSocket serverSocket = null;
        try {
            serverSocket = new ServerSocket(PORT);
            System.out.println("AS服务器开始监听:");
            int counter = 1;
            while (true) {
                Socket socket = serverSocket.accept();
                System.out.println("建立第 " + (counter++) + " 个连接!");
                Thread t = new Thread(new ASHandler(socket));
                t.start();
            }
        } catch (IOException e) {
            e.printStackTrace();
        }
    }
}
