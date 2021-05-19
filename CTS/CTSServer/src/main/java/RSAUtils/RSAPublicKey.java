package RSAUtils;

import java.math.BigInteger;

public class RSAPublicKey {
    private BigInteger n;//p*q
    private BigInteger e;//gcd(f,e)=1

    public RSAPublicKey(BigInteger n, BigInteger e) {
        this.n = n;
        this.e = e;
    }

    public RSAPublicKey(String Base64Str) {
        String[] strings = Base64Str.split("\n");
        this.n = new BigInteger(RSATools.base64ToCode(strings[1]));
        this.e = new BigInteger(RSATools.base64ToCode(strings[2]));
    }

    public BigInteger getN() {
        return this.n;
    }

    public BigInteger getE() {
        return this.e;
    }

    @Override
    public String toString() {
        StringBuilder pkStr = new StringBuilder();
        pkStr.append(RSATools.codeToBase64(n.toString()));
        pkStr.append("\n");
        pkStr.append(RSATools.codeToBase64(e.toString()));
        pkStr.insert(0, "-----BEGIN PUBLIC KEY-----\n");
        pkStr.append("\n-----END PUBLIC KEY-----\n");
        return pkStr.toString();

    }
}
