package RSAUtils;

import java.math.BigInteger;

public class RSAHandler {
    private RSAKeyPair rsaKeyPair;

    public RSAHandler() {
        rsaKeyPair = new RSAKeyPair();
    }

    public RSAHandler(String pkStr, String skStr) {
        rsaKeyPair = new RSAKeyPair(pkStr, skStr);
    }

    public RSAKeyPair getMyKeyPair() {
        return rsaKeyPair;
    }

    private BigInteger powModByPublicKey(BigInteger M) {
        return RSATools.quickPowMod(M,
                rsaKeyPair.getPublicKey().getE(), rsaKeyPair.getPublicKey().getN());
    }

    private BigInteger powModByPrivateKey(BigInteger C) {
        return RSATools.quickPowMod(C,
                rsaKeyPair.getPrivateKey().getD(), rsaKeyPair.getPrivateKey().getN());
    }

    public String RSAEncrypt(String plaintext) {
        StringBuilder ciphertext = new StringBuilder();
        for (int i = 0; i < plaintext.length(); i++) {
            BigInteger tempBI = powModByPublicKey(BigInteger.valueOf(plaintext.charAt(i)));
            StringBuilder tempSB = new StringBuilder(String.valueOf(tempBI));
            while (tempSB.toString().length() < rsaKeyPair.getLengthOfN()) {
                tempSB.insert(0, 0);
            }
            ciphertext.append(tempSB);
        }
        return ciphertext.toString();
    }

    public String RSADecrypt(String ciphertext) {
        StringBuilder plaintext = new StringBuilder();
        for (int i = 0; i < ciphertext.length() / rsaKeyPair.getLengthOfN(); i++) {
            BigInteger tempBI = powModByPrivateKey(new BigInteger(ciphertext.substring(i * rsaKeyPair.getLengthOfN(), (i + 1) * rsaKeyPair.getLengthOfN())));
            plaintext.append((char) tempBI.intValue());
        }
        return plaintext.toString();
    }

    public String RSAEnSign(String text) {
        StringBuilder sign = new StringBuilder();
        for (int i = 0; i < text.length(); i++) {
            BigInteger tempBI = powModByPrivateKey(BigInteger.valueOf(text.charAt(i)));
            StringBuilder tempSB = new StringBuilder(String.valueOf(tempBI));
            while (tempSB.toString().length() < rsaKeyPair.getLengthOfN()) {
                tempSB.insert(0, 0);
            }
            sign.append(tempSB);
        }
        return sign.toString();
    }

    public String RSADeSign(String sign) {
        StringBuilder text = new StringBuilder();
        for (int i = 0; i < sign.length() / rsaKeyPair.getLengthOfN(); i++) {
            BigInteger tempBI = powModByPublicKey(new BigInteger(sign.substring(i * rsaKeyPair.getLengthOfN(), (i + 1) * rsaKeyPair.getLengthOfN())));
            text.append((char) tempBI.intValue());
        }
        return text.toString();
    }
}