package TransmissionUtils;

import java.net.InetAddress;
import java.net.ServerSocket;

public class Connection {
    //等待队列长度
    private static final int BACKLOG = 10;

    /**
     * 绑定服务器方法
     *
     * @param IPBytes byte格式的IP地址
     * @param port    端口号
     * @return ServerSocket
     * @throws Exception 绑定异常
     */
    public static ServerSocket bindServer(byte[] IPBytes, int port) throws Exception {
        ServerSocket serverSocket = new ServerSocket(port, BACKLOG, InetAddress.getByAddress(IPBytes));
        serverSocket.setReuseAddress(true);
        return serverSocket;
    }

    /**
     * 绑定服务器方法
     *
     * @param IPStr 字符串格式的IP地址
     * @param port  端口号
     * @return ServerSocket
     * @throws Exception 绑定异常
     */
    public static ServerSocket bindServer(String IPStr, int port) throws Exception {
        ServerSocket serverSocket = new ServerSocket(port, BACKLOG, InetAddress.getByName(IPStr));
        serverSocket.setReuseAddress(true);
        return serverSocket;
    }
}