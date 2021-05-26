package SecurityUtils;

import javax.crypto.Cipher;
import java.io.ByteArrayOutputStream;
import java.nio.file.Files;
import java.nio.file.Path;
import java.security.KeyFactory;
import java.security.PrivateKey;
import java.security.PublicKey;
import java.security.Signature;
import java.security.spec.PKCS8EncodedKeySpec;
import java.security.spec.X509EncodedKeySpec;
import java.util.Base64;

public class RSAHandler {
    private static final int PUBLIC_BUFFERSIZE = 117;//公钥加密的字符串长度
    private static final int PRIVATE_BUFFERSIZE = 128;//私钥加密的字符串长度

    /**
     * RSA加密方法
     *
     * @param pKeyFile  公钥文件名
     * @param plainText 明文
     * @return Base64编码的密文
     * @throws Exception RSA异常
     */
    public static String encrypt(String pKeyFile, String plainText) throws Exception {
        String pKeyStr = Files.readString(Path.of(pKeyFile));//读取公钥文件
        PublicKey pKey = getRSAPublicKey(pKeyStr);//获取公钥
        //初始化RSA加密对象
        Cipher cipher = Cipher.getInstance("RSA");
        cipher.init(Cipher.ENCRYPT_MODE, pKey);
        //分块加密
        ByteArrayOutputStream out = new ByteArrayOutputStream();
        int offSet = 0;
        int bufferSize = PUBLIC_BUFFERSIZE;
        while (plainText.length() - offSet > 0) {
            byte[] buffer = null;
            if (plainText.length() - offSet > bufferSize) {
                buffer = cipher.doFinal(plainText.getBytes(), offSet, bufferSize);
            } else {
                buffer = cipher.doFinal(plainText.getBytes(), offSet, plainText.length() - offSet);
            }
            out.write(buffer, 0, buffer.length);
            offSet += bufferSize;
        }
        byte[] cipherBytes = out.toByteArray();
        //转换Base64格式
        Base64.Encoder base64Encoder = Base64.getEncoder();
        return base64Encoder.encodeToString(cipherBytes);
    }

    /**
     * RSA解密方法
     *
     * @param sKeyFile   私钥文件名
     * @param cipherText Base64编码的密文
     * @return 明文
     * @throws Exception RSA异常
     */
    public static String decrypt(String sKeyFile, String cipherText) throws Exception {
        String sKeyStr = Files.readString(Path.of(sKeyFile));//读取私钥文件
        PrivateKey sKey = getRSAPrivateKey(sKeyStr);//获取私钥
        //初始化RSA解密对象
        Cipher cipher = Cipher.getInstance("RSA");
        cipher.init(Cipher.DECRYPT_MODE, sKey);
        Base64.Decoder base64Decoder = Base64.getDecoder();
        //分块解密
        byte[] cipherBytes = base64Decoder.decode(cipherText.getBytes());
        ByteArrayOutputStream out = new ByteArrayOutputStream();
        int offSet = 0;
        int bufferSize = PRIVATE_BUFFERSIZE;
        while (cipherBytes.length - offSet > 0) {
            byte[] buffer = null;
            if (cipherBytes.length - offSet > bufferSize) {
                buffer = cipher.doFinal(cipherBytes, offSet, bufferSize);
            } else {
                buffer = cipher.doFinal(cipherBytes, offSet, cipherBytes.length - offSet);
            }
            out.write(buffer, 0, buffer.length);
            offSet += bufferSize;
        }
        byte[] plainBytes = out.toByteArray();
        return new String(plainBytes);//转换明文
    }

    /**
     * 生成数字签名方法
     *
     * @param sKeyFile 私钥文件名
     * @param text     明文
     * @return 数字签名
     * @throws Exception RSA异常、MD5异常
     */
    public static String generateSign(String sKeyFile, String text) throws Exception {
        String sKeyStr = Files.readString(Path.of(sKeyFile));//读取私钥文件
        PrivateKey sKey = getRSAPrivateKey(sKeyStr);//获取私钥
        //初始化数字签名对象
        Signature signature = Signature.getInstance("MD5withRSA");
        signature.initSign(sKey);
        signature.update(text.getBytes());
        byte[] signBytes = signature.sign();
        //转化Base64格式
        Base64.Encoder base64Encoder = Base64.getEncoder();
        return base64Encoder.encodeToString(signBytes);
    }

    /**
     * 验证数字签名方法
     *
     * @param pKeyFile 公钥文件名
     * @param sign     数字签名
     * @param text     明文
     * @return 验证结果
     * @throws Exception RSA异常、MD5异常
     */
    public static boolean verifySign(String pKeyFile, String sign, String text) throws Exception {
        String pKeyStr = Files.readString(Path.of(pKeyFile));//读取公钥对象
        PublicKey pKey = getRSAPublicKey(pKeyStr);//获取公钥
        //初始化数字签名对象
        Signature signature = Signature.getInstance("MD5withRSA");
        signature.initVerify(pKey);
        signature.update(text.getBytes());
        //验证数字签名
        Base64.Decoder base64Decoder = Base64.getDecoder();
        return signature.verify(base64Decoder.decode(sign));
    }

    /**
     * 获取公钥方法
     *
     * @param publicKeyStr PEM格式的公钥
     * @return 标准公钥对象
     * @throws Exception RSA异常
     */
    private static PublicKey getRSAPublicKey(String publicKeyStr) throws Exception {
        //替换无关字符
        publicKeyStr = publicKeyStr.replace("-----BEGIN PUBLIC KEY-----", "");
        publicKeyStr = publicKeyStr.replace("-----END PUBLIC KEY-----", "");
        publicKeyStr = publicKeyStr.replace("\r", "");
        publicKeyStr = publicKeyStr.replace("\n", "");
        //Base64格式译码
        Base64.Decoder base64Decoder = Base64.getDecoder();
        byte[] decodedBytes = base64Decoder.decode(publicKeyStr.getBytes());
        //初始化标准公钥对象
        X509EncodedKeySpec spec = new X509EncodedKeySpec(decodedBytes);
        KeyFactory keyFactory = KeyFactory.getInstance("RSA");
        return keyFactory.generatePublic(spec);
    }

    /**
     * 获取私钥方法
     *
     * @param privateKeyStr PEM格式的私钥
     * @return 标准私钥对象
     * @throws Exception RSA异常
     */
    private static PrivateKey getRSAPrivateKey(String privateKeyStr) throws Exception {
        //替换无关字符
        privateKeyStr = privateKeyStr.replace("-----BEGIN PRIVATE KEY-----", "");
        privateKeyStr = privateKeyStr.replace("-----END PRIVATE KEY-----", "");
        privateKeyStr = privateKeyStr.replace("\r", "");
        privateKeyStr = privateKeyStr.replace("\n", "");
        //Base64格式译码
        Base64.Decoder base64Decoder = Base64.getDecoder();
        byte[] decodedBytes = base64Decoder.decode(privateKeyStr.getBytes());
        //初始化标准公钥对象
        PKCS8EncodedKeySpec spec = new PKCS8EncodedKeySpec(decodedBytes);
        KeyFactory keyFactory = KeyFactory.getInstance("RSA");
        return keyFactory.generatePrivate(spec);
    }
}