package TransmissionUtils;

import java.io.*;
import java.net.Socket;

public class Transceiver {
    private final Socket socket;

    public Transceiver(Socket socket) {
        this.socket = socket;
    }

    public void sendMessage(TransMessage message) throws Exception {
        byte[] buffer = message.MessageToBytes();
        DataOutputStream dos = new DataOutputStream(
                new BufferedOutputStream(
                        socket.getOutputStream()));
        dos.write(message.MessageToBytes());
        dos.flush();
    }

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
        int signLen = Integer.parseInt(new String(buffer).trim());
        buffer = new byte[4];
        dis.read(buffer);
        int contentLen = Integer.parseInt(new String(buffer).trim());
        //数字签名字段
        buffer = new byte[signLen];
        dis.read(buffer);
        message.setSignature(new String(buffer));
        //报文内容字段
        buffer = new byte[contentLen];
        dis.read(buffer);
        message.setContents(new String(buffer));
        //图片大小
        buffer = new byte[4];
        dis.read(buffer);
        int imgLen = Integer.parseInt(new String(buffer).trim());
        //图片
        buffer = new byte[imgLen];
        dis.read(buffer);
        message.setImage(buffer);
        return message;
    }
    //还原接收图片
    public static void RePicture(byte[] image, String Mid){

        FileOutputStream fos= null;
        //fos = new FileOutputStream("D:\\1\\2.jpg");
        try {
            fos.write(image,0,image.length);
            fos.flush();
            fos.close();
        } catch (IOException e) {
            e.printStackTrace();
        }

    }
}
