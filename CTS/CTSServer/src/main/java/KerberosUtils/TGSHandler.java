package KerberosUtils;

import DataUtils.Authenticator;
import DataUtils.Ticket;
import EnumUtils.EnumKerberos;
import PropertiesUtils.PropertiesHandler;
import TransmissionUtils.AddressPhaser;
import TransmissionUtils.TransMessage;
import TransmissionUtils.Transceiver;

import java.net.Socket;
import java.util.Arrays;

public class TGSHandler implements Runnable {
    //生命周期
    private static final int LIFE_TIME = 60;
    //数据收发器
    private final Transceiver transceiver;
    //连接计数
    private final int counter;

    /**
     * 构造方法
     * @param socket socket用于初始化transceiver
     * @param counter 连接计数
     */
    public TGSHandler(Socket socket,int counter) {
        this.transceiver = new Transceiver(socket);
        this.counter = counter;
    }

    @Override
    public void run() {
        try {
            while (true) {
                TransMessage message = transceiver.receiveMessage();
                String rsaPKFile = "src\\resourcesTGS\\KeyFiles\\"
                        + PropertiesHandler.getPropertiesElement(AddressPhaser.bytesToString(message.getFromAddress()))
                        + ".pk";
                message.dePackage(rsaPKFile, null);
                switch (message.getSpecificType()) {
                    case EnumKerberos.Request: {
                        System.out.println(Arrays.toString(message.getFromAddress()));
                        System.out.println(message.getContents());
                    }
                    case EnumKerberos.End: {
                        transceiver.closeTransceiver();
                        System.out.println("断开第 " + counter + " 个连接!");
                        return;
                    }
                    default:
                        throw new Exception("未知服务类型！");
                }
            }
        } catch (Exception exception) {
            exception.printStackTrace();
        }
    }

    private boolean verifyMessage(TransMessage transMessage) {
        return false;
    }

    private boolean verifyLive(long timestamp, int lifetime) {
        return false;
    }

    private boolean verifyTicket(Ticket ticket, Authenticator authenticatorC) {
        return false;
    }

    private String getIDKey(String ID) {
        return null;
    }

    private String generateSessionKey() {
        return null;
    }

    private long generateTimeStamp() {
        return 0;
    }
}