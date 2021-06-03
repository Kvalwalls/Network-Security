package KerberosUtils;

import DataUtils.Authenticator;
import DataUtils.Ticket;

import java.util.Base64;
import java.util.Random;

public class ToolsKerberos {
    public static final int LIFE_TIME = 60;

    public static long generateTS() {
        return (System.currentTimeMillis() / 1000);
    }

    public static boolean verifyTS(long ts, long lifetime) {
        return (generateTS() - ts < lifetime);
    }

    public static String generateSessionK() {
        Random random = new Random();
        byte[] randomBytes = new byte[6];
        random.nextBytes(randomBytes);
        Base64.Encoder base64Encoder = Base64.getEncoder();
        return new String(base64Encoder.encode(randomBytes));
    }

    public static boolean verifyTicketAndAuthenticator(Ticket ticket, Authenticator authenticator) {
        if (!ToolsKerberos.verifyTS(ticket.getTimestamp(), ticket.getLifetime()))
            return false;
        if (!ToolsKerberos.verifyTS(authenticator.getTimestamp(), LIFE_TIME))
            return false;
        if (!ticket.getAD_c().equals(authenticator.getAD_c()))
            return false;
        if (!ticket.getID_c().equals(authenticator.getID_c()))
            return false;
        return true;
    }
}
