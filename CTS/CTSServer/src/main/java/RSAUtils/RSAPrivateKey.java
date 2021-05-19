package RSAUtils;

import java.math.BigInteger;

public class RSAPrivateKey {
    private BigInteger n;//p*q
    private BigInteger d;//d*e=1(mod n)

    public RSAPrivateKey(BigInteger n, BigInteger d) {
        this.n = n;
        this.d = d;
    }

    public RSAPrivateKey(String Base64Str) {
        String[] strings = Base64Str.split("\n");
        this.n = new BigInteger(RSATools.base64ToCode(strings[1]));
        this.d = new BigInteger(RSATools.base64ToCode(strings[2]));
    }

    public BigInteger getN() {
        return this.n;
    }

    public BigInteger getD() {
        return this.d;
    }

    @Override
    public String toString() {
        StringBuilder skStr = new StringBuilder();
        skStr.append(RSATools.codeToBase64(n.toString()));
        skStr.append("\n");
        skStr.append(RSATools.codeToBase64(d.toString()));
        skStr.insert(0, "-----BEGIN PRIVATE KEY-----\n");
        skStr.append("\n-----END PRIVATE KEY-----\n");
        return skStr.toString();
    }
}
