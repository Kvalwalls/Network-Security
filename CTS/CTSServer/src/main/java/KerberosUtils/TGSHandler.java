package KerberosUtils;

import DataUtils.Authenticator;
import DataUtils.Ticket;
import TransmissionUtils.TransMessage;
import TransmissionUtils.Transceiver;

import java.net.Socket;

public class TGSHandler implements Runnable {
    private static final int LIFE_TIME = 100000;
    private static final String ID_TGS = "";
    private final Transceiver transceiver;
    private String encryptKey;

    /**
     * 构造方法
     * @param socket socket用于初始化transceiver
     */
    public TGSHandler(Socket socket) {
        this.transceiver = new Transceiver(socket);
    }

    @Override
    public void run() {

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