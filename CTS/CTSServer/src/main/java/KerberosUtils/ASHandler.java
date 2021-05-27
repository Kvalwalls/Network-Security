package KerberosUtils;

import TransmissionUtils.TransMessage;
import TransmissionUtils.Transceiver;

import javax.swing.text.Document;
import java.net.Socket;

public class ASHandler implements Runnable {
    private static final int LIFE_TIME = 100000;
    private final Transceiver transceiver;
    private String encryptKey;

    /**
     * 构造方法
     * @param socket socket用于初始化transceiver
     */
    public ASHandler(Socket socket) {
        this.transceiver = new Transceiver(socket);
    }

    @Override
    public void run() {
        try {
            TransMessage transMessage = transceiver.receiveMessage();
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

    private String getIDKey(String ID) {
        return null;
    }

    private String generateSessionKey() {
        return null;
    }

    private long generateTimestamp() {
        return 0;
    }
}