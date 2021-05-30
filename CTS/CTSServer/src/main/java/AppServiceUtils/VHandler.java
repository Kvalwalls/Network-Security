package AppServiceUtils;

import DataUtils.Authenticator;
import DataUtils.Ticket;
import TransmissionUtils.TransMessage;

public abstract class VHandler {
    protected static final int LIFE_TIME = 100000;
    protected String encryptKey;

    protected boolean verifyMessage(TransMessage transMessage) {
        return false;
    }

    protected boolean verifyLive(String timeStamp, int lifetime) {
        return false;
    }

    protected boolean verifyTicket(Ticket ticket, Authenticator authenticatorC) {
        return false;
    }
}