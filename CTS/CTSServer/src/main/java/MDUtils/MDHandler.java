package MDUtils;

import java.security.MessageDigest;
import java.security.NoSuchAlgorithmException;
import java.util.Base64;

public class MDHandler {
    public static String generateMD(String text) throws NoSuchAlgorithmException {
        MessageDigest md5 = MessageDigest.getInstance("MD5");
        md5.update(text.getBytes());
        byte[] md5Bytes = md5.digest();
        Base64.Encoder encoder = Base64.getEncoder();
        return encoder.encodeToString(md5Bytes);
    }

    public static boolean verifyMD(String mdStr, String text) throws NoSuchAlgorithmException {
        MessageDigest md5 = MessageDigest.getInstance("MD5");
        md5.update(text.getBytes());
        byte[] md5Bytes = md5.digest();
        Base64.Encoder encoder = Base64.getEncoder();
        String tmpStr = encoder.encodeToString(md5Bytes);
        return mdStr.equals(tmpStr);
    }
}
