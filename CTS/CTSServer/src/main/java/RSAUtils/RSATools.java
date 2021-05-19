package RSAUtils;

import java.math.BigInteger;
import java.util.Base64;
import java.util.Random;

public class RSATools {
    //快速模幂运算
    public static BigInteger quickPowMod(BigInteger baseNum, BigInteger powNum, BigInteger modNum) {
        BigInteger result = BigInteger.ONE;
        while (!powNum.equals(BigInteger.ZERO)) {
            if (powNum.mod(BigInteger.TWO).equals(BigInteger.ONE)) {
                result = result.multiply(baseNum).mod(modNum);
            }
            baseNum = baseNum.multiply(baseNum).mod(modNum);
            powNum = powNum.divide(BigInteger.TWO);
        }
        return result;
    }

    //快速模积运算
    public static BigInteger quickMulMod(BigInteger num1, BigInteger num2, BigInteger modNum) {
        BigInteger result = BigInteger.ZERO;
        while (!num2.equals(BigInteger.ZERO)) {
            if (num2.mod(BigInteger.TWO).equals(BigInteger.ONE)) {
                result = result.add(num1).mod(modNum);
            }
            num1 = num1.multiply(BigInteger.TWO).mod(modNum);
            num2 = num2.divide(BigInteger.TWO);
        }
        return result;
    }

    //快速模逆运算
    public static BigInteger quickInvMod(BigInteger num,BigInteger modNum) {
        if (num.compareTo(modNum) >= 0)
            num = num.mod(modNum);
        return exGCD(num, modNum)[1].compareTo(BigInteger.ZERO) <= 0
                ?
                exGCD(num, modNum)[1].add(modNum) : exGCD(num, modNum)[1];
    }

    //拓展欧几里得算法
    private static BigInteger[] exGCD(BigInteger a,BigInteger b) {
        if (a.compareTo(b) < 0) {
            BigInteger temp = new BigInteger(String.valueOf(a));
            a = b;
            b = temp;
        }
        BigInteger[] result = new BigInteger[3];
        if (b.equals(BigInteger.ZERO)) {
            result[0] = BigInteger.ONE;
            result[1] = BigInteger.ZERO;
            result[2] = new BigInteger(String.valueOf(a));
            return result;
        }
        BigInteger[] temp = exGCD(b, a.mod(b));
        result[0] = temp[1];
        result[1] = temp[0].subtract(a.divide(b).multiply(temp[1]));
        result[2] = temp[2];
        return result;
    }

    //快速判断素数
    public static boolean quickJudgePrime(BigInteger number) {
        //0、1、2直接判断
        if (number.equals(BigInteger.ZERO) || number.equals(BigInteger.ONE)) {
            return false;
        }
        if (number.equals(BigInteger.TWO)) {
            return true;
        }
        //偶数直接判断
        if (number.mod(BigInteger.TWO).equals(BigInteger.ZERO)) {
            return false;
        }
        BigInteger numMinusOne = number.subtract(BigInteger.ONE);//获得number-1
        //若number-1为偶数则向右移n位直到奇数并记录移位数
        int shiftLeftBits = 0;
        while (numMinusOne.mod(BigInteger.TWO).equals(BigInteger.ZERO)) {
            shiftLeftBits++;
            numMinusOne = numMinusOne.divide(BigInteger.valueOf(2));
        }
        Random random = new Random();//随机数
        //进行5次测试
        for (int i = 0; i < 5; i++) {
            //在[2,n)之间取随机数
            BigInteger ranNum = new BigInteger(100, random)
                    .mod(number.subtract(BigInteger.valueOf(2)))
                    .add(BigInteger.valueOf(2));
            //ranNum与number不互素直接跳过
            if (ranNum.mod(number).equals(BigInteger.ZERO))
                continue;
            //计算ranNum^(number-1)%number的值
            ranNum = quickPowMod(ranNum, numMinusOne, number);
            //二次探测
            BigInteger preRanNum = new BigInteger(String.valueOf(ranNum));
            for (int j = 0; j < shiftLeftBits; j++) {
                //补回移位的值
                ranNum = quickMulMod(ranNum, ranNum, number);
                //二次探测定理的判断
                if (ranNum.equals(BigInteger.ONE)
                        && !preRanNum.equals(BigInteger.ONE)
                        && !preRanNum.equals(number.subtract(BigInteger.ONE)))
                    return false;
                preRanNum = new BigInteger(String.valueOf(ranNum));
            }
            //费马小定理的判断
            if (!ranNum.equals(BigInteger.ONE))
                return false;
        }
        return true;
    }

    //快速判断互素
    public static boolean quickJudgeGCD(BigInteger num1, BigInteger num2) {
        BigInteger temp;
        while (!num2.equals(BigInteger.ZERO)) {
            temp = num1.mod(num2);
            num1 = num2;
            num2 = temp;
        }
        if (num1.equals(BigInteger.ONE))
            return true;
        return false;
    }
    //数字码串转换Base64码串
    public static String codeToBase64(String codeStr) {
        String base64Str = null;
        Base64.Encoder encoder = Base64.getEncoder();//Base64编码器
        int num = codeStr.length() / 2;
        int mod = codeStr.length() % 2;
        byte[] codeByte;
        if(mod == 0) {
            codeByte = new byte[num];//byte数组储存编码前的数据
            for (int i = 0; i < num; i++) {
                codeByte[i] = (byte) Integer.parseInt(codeStr.substring(i * 2, (i + 1) * 2));
            }
        } else {
            codeByte = new byte[num + 1];
            for (int i = 0; i < num + 1; i++) {
                if (i == num) {
                    int temp = Integer.parseInt(codeStr.substring(i * 2));
                    if (temp < 10) {
                        temp *= 10;
                    }
                    codeByte[i] = (byte) temp;
                } else {
                    codeByte[i] = (byte) Integer.parseInt(codeStr.substring(i * 2, (i + 1) * 2));
                }
            }
        }
        base64Str = encoder.encodeToString(codeByte);//编码
        return base64Str;
    }
    //Base64码串转换数字码串
    public static String base64ToCode(String base64Str) {
        StringBuilder codeStr = new StringBuilder();
        Base64.Decoder decoder = Base64.getDecoder();//Base64译码器
        byte[] codeByte = decoder.decode(base64Str);//译码
        //byte到二进制码串的转换
        for (int i = 0; i < codeByte.length; i++) {
            StringBuilder codeChar = new StringBuilder(Integer.toString((char)codeByte[i]));
            while (codeChar.length() < 2) {
                codeChar.insert(0, 0);
            }
            codeStr.append(codeChar);
        }
        return codeStr.toString();
    }
}
