package TransmissionUtils;

import java.io.BufferedInputStream;
import java.io.BufferedOutputStream;
import java.io.DataInputStream;
import java.io.DataOutputStream;
import java.net.Socket;

public class Transceiver {
    private final Socket socket;

    /**
     * 带参数的构造方法
     */
    public Transceiver(Socket socket) {
        this.socket = socket;
    }

    /**
     * 报文发送方法
     *
     * @param message 发送的报文对象
     * @throws Exception 发送异常
     */
    public void sendMessage(TransMessage message) throws Exception {
        DataOutputStream dos = new DataOutputStream(
                new BufferedOutputStream(
                        socket.getOutputStream()));
        dos.write(message.MessageToBytes());
        dos.flush();
    }

    /**
     * 报文接收方法
     *
     * @return 接收的报文对象
     * @throws Exception 接收异常
     */
    public TransMessage receiveMessage() throws Exception {
        byte[] buffer = null;
        TransMessage message = new TransMessage();
        DataInputStream dis = new DataInputStream(
                new BufferedInputStream(socket.getInputStream()));
        //源、目的IP地址字段
        buffer = new byte[4];
        dis.read(buffer);
        message.setToAddress(buffer);
        dis.read(buffer);
        message.setFromAddress(buffer);
        //其他控制字段
        buffer = new byte[1];
        dis.read(buffer);
        message.setServiceType(buffer[0]);
        dis.read(buffer);
        message.setSpecificType(buffer[0]);
        dis.read(buffer);
        message.setErrorCode(buffer[0]);
        dis.read(buffer);
        message.setCryptCode(buffer[0]);
        //长度字段
        buffer = new byte[4];
        dis.read(buffer);
        int signLen = IntBytesPhaser.bytesToInt(buffer);
        buffer = new byte[4];
        dis.read(buffer);
        int contentLen = IntBytesPhaser.bytesToInt(buffer);
        //数字签名字段
        buffer = new byte[signLen];
        dis.read(buffer);
        message.setSignature(new String(buffer));
        //报文内容字段
        buffer = new byte[contentLen];
        dis.read(buffer);
        message.setContents(new String(buffer));
        return message;
    }

    /**
     * 关闭数据收发器方法
     *
     * @throws Exception 关闭异常
     */
    public void closeTransceiver() throws Exception {
        socket.shutdownInput();
        socket.shutdownOutput();
        socket.close();
    }
}