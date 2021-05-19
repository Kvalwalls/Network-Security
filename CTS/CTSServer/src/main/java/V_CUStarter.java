import AppServiceUtils.CommonUserVHandler;
import KerberosUtils.TGSHandler;

import java.io.IOException;
import java.net.ServerSocket;
import java.net.Socket;

public class V_CUStarter {
    private static final int PORT = 8084;

    public static void main(String[] args) {
        ServerSocket serverSocket = null;
        try {
            serverSocket = new ServerSocket(PORT);
            System.out.println("CUV服务器开始监听:");
            int counter = 1;
            while (true) {
                Socket socket = serverSocket.accept();
                System.out.println("建立第 " + (counter++) + " 个连接!");
                Thread t = new Thread(new CommonUserVHandler(socket));
                t.start();
            }
        } catch (IOException e) {
            e.printStackTrace();
        }
    }
}