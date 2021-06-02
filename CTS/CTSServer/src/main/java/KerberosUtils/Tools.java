package KerberosUtils;

import java.util.Base64;
import java.util.Random;

public class Tools {
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
}
