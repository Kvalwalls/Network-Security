import KerberosUtils.TGSHandler;
import TransmissionUtils.Connection;

import java.io.IOException;
import java.net.ServerSocket;
import java.net.Socket;

public class K_TGSStarter {
    private static final int PORT = 8082;

    public static void main(String[] args) {
        ServerSocket serverSocket = null;
        try {
            serverSocket = Connection.bindServer("127.0.0.1",PORT);
            System.out.println("TGS服务器开始监听:");
            int counter = 1;
            while (true) {
                Socket socket = serverSocket.accept();
                Thread t = new Thread(new TGSHandler(socket,counter));
                System.out.println("建立第 " + (counter++) + " 个连接!");
                t.start();
            }
        } catch (Exception e) {
            e.printStackTrace();
        }
    }
}