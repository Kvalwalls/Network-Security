package SecurityUtils;

import javax.crypto.Cipher;
import javax.crypto.SecretKeyFactory;
import javax.crypto.spec.DESKeySpec;
import javax.crypto.spec.IvParameterSpec;
import javax.crypto.spec.SecretKeySpec;
import java.security.Key;
import java.security.Security;
import java.security.spec.AlgorithmParameterSpec;
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
                keyBuilder.append("0");
            key = keyBuilder.toString();
        } else if (key.length() > 8)
            key = key.substring(0, 8);
        //获取DES加密对象
        DESKeySpec keySpec = new DESKeySpec(key.getBytes());
        Key desKey = SecretKeyFactory.getInstance("DES").generateSecret(keySpec);
        AlgorithmParameterSpec desIV = new IvParameterSpec(key.getBytes());//CBC初始化向量
        Cipher cipher = Cipher.getInstance("DES/CBC/PKCS5Padding");
        cipher.init(Cipher.ENCRYPT_MODE, desKey, desIV);
        //DES加密过程
        byte[] cipherBytes = cipher.doFinal(plainText.getBytes());
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
                keyBuilder.append("0");
            key = keyBuilder.toString();
        } else if (key.length() > 8)
            key = key.substring(0, 8);
        //获取DES解密对象
        DESKeySpec keySpec = new DESKeySpec(key.getBytes());
        Key desKey = SecretKeyFactory.getInstance("DES").generateSecret(keySpec);
        AlgorithmParameterSpec desIV = new IvParameterSpec(key.getBytes());//CBC初始化向量
        Cipher cipher = Cipher.getInstance("DES/CBC/PKCS5Padding");
        cipher.init(Cipher.DECRYPT_MODE, desKey, desIV);
        //DES解密过程
        Base64.Decoder base64Decoder = Base64.getDecoder();
        byte[] cipherBytes = base64Decoder.decode(cipherText.getBytes());
        byte[] plainBytes = cipher.doFinal(cipherBytes);
        return new String(plainBytes);
    }
}