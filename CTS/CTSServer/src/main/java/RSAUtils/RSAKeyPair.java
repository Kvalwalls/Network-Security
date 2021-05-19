package RSAUtils;

import java.math.BigInteger;
import java.util.Random;

public class RSAKeyPair {
    private static final int PQ_LENGTH = 512;//素数p、q的位长度

    private final RSAPrivateKey privateKey;//私钥
    private final RSAPublicKey publicKey;//公钥
    private int lengthOfN = 0;

    public RSAKeyPair() {
        BigInteger p = null, q = null;
        do {
            p = BigInteger.probablePrime(PQ_LENGTH, new Random());//素数p
            q = BigInteger.probablePrime(PQ_LENGTH, new Random());//素数q
        } while (RSATools.quickJudgePrime(p) && RSATools.quickJudgePrime(q));
        BigInteger n = p.multiply(q);//n=p*q
        BigInteger f = p.subtract(BigInteger.ONE).multiply(q.subtract(BigInteger.ONE));///f=(p-1)*(q-1)
        //随机产生e，满足：e＜f且e和f互素
        BigInteger e = null;
        do {
            e = new BigInteger(new Random().nextInt(f.bitLength() - 1) + 1, new Random());//产生随机数
        } while ((e.compareTo(f) >= 0) || (e.equals(BigInteger.ONE)) || (!RSATools.quickJudgeGCD(e, f)));//判断是否满足条件
        BigInteger d = RSATools.quickInvMod(e, f);//d=e^(-1)(mod f)
        //实例化公钥和私钥
        publicKey = new RSAPublicKey(n, e);
        privateKey = new RSAPrivateKey(n, d);
        lengthOfN = n.toString().length();
    }

    public RSAKeyPair(String pkStr, String skStr) {
        publicKey = new RSAPublicKey(pkStr);
        privateKey = new RSAPrivateKey(skStr);
        lengthOfN = publicKey.getN().toString().length();
    }

    public int getLengthOfN() {
        return lengthOfN;
    }

    public RSAPublicKey getPublicKey() {
        return publicKey;
    }

    public RSAPrivateKey getPrivateKey() {
        return privateKey;
    }
}
