package SecurityUtils;

import javax.crypto.Cipher;
import javax.crypto.spec.SecretKeySpec;
import java.security.Security;
import java.util.Base64;

public class DESHandler {
    /**
     * DES加密方法
     *
     * @param key       密钥
     * @param plainText 明文
     * @return Base64编码的密文
     * @throws Exception DES异常
     */
    public static String encrypt(String key, String plainText) throws Exception {
        //key的扩充与缩减
        if (key.length() < 8) {
            StringBuilder keyBuilder = new StringBuilder(key);
            while (keyBuilder.length() < 8)
                keyBuilder.append("\0");
            key = keyBuilder.toString();
        } else if (key.length() > 8)
            key = key.substring(0, 8);
        SecretKeySpec desKey = new SecretKeySpec(key.getBytes(), "DES");//密钥
        Cipher cipher = Cipher.getInstance("DES/ECB/PKCS5Padding");//ECB模式、PKCS5填充模式的DES
        //加密过程
        cipher.init(Cipher.ENCRYPT_MODE, desKey);
        byte[] cipherBytes = cipher.doFinal(plainText.getBytes());
        //Base64编码
        Base64.Encoder base64Encoder = Base64.getEncoder();
        return base64Encoder.encodeToString(cipherBytes);
    }

    /**
     * DES解密方法
     *
     * @param key        密钥
     * @param cipherText Base64编码的密文
     * @return 明文
     * @throws Exception DES异常
     */
    public static String decrypt(String key, String cipherText) throws Exception {
        //key的扩充与缩减
        if (key.length() < 8) {
            StringBuilder keyBuilder = new StringBuilder(key);
            while (keyBuilder.length() < 8)
                keyBuilder.append("\0");
            key = keyBuilder.toString();
        } else if (key.length() > 8)
            key = key.substring(0, 8);
        SecretKeySpec desKey = new SecretKeySpec(key.getBytes(), "DES");//密钥
        Cipher cipher = Cipher.getInstance("DES/ECB/PKCS5Padding");//ECB模式、PKCS5填充模式的DES
        //Base64解码
        Base64.Decoder base64Decoder = Base64.getDecoder();
        byte[] cipherBytes = base64Decoder.decode(cipherText.getBytes());
        //解密过程
        cipher.init(Cipher.DECRYPT_MODE, desKey);
        byte[] plainBytes = cipher.doFinal(cipherBytes);
        return new String(plainBytes);
    }
}