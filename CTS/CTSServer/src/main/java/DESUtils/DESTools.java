package DESUtils;

import java.util.Base64;

public class DESTools {
    //位异或操作
    public static StringBuilder XOR(StringBuilder binStr1, StringBuilder binStr2) {
        StringBuilder resultStr = new StringBuilder();
        //异或操作
        for (int i = 0; i < binStr1.length(); i++) {
            //相同为0 不同为1
            if (binStr1.charAt(i) == binStr2.charAt(i)) {
                resultStr.append("0");
            } else {
                resultStr.append("1");
            }
        }
        return resultStr;
    }

    //普通字符串转换二进制码串
    public static StringBuilder stringToBinStr(StringBuilder str) {
        StringBuilder binStr = new StringBuilder();
        //逐个字符进行转换
        for (int i = 0; i < str.length(); i++) {
            StringBuilder binChar = new StringBuilder(Integer.toBinaryString(str.charAt(i)));//每个字符转换为二进制码字符串
            //不满16位的二进制码串补0
            while (binChar.length() < 16) {
                binChar.insert(0, 0);
            }
            binStr.append(binChar);
        }
        return binStr;
    }

    //二进制码串转换普通字符串
    public static StringBuilder binStrToString(StringBuilder binStr) {
        StringBuilder str = new StringBuilder();
        //每16位的二进制码串进行转换
        for (int i = 0; i < binStr.length(); i += 16) {
            int data = Integer.parseInt(binStr.substring(i, i + 16), 2);//每16位二进制码转换为字符
            str.append((char) data);
        }
        return str;
    }

    //二进制码转换Base64码串
    public static StringBuilder binToBase64(StringBuilder binStr) {
        String base64Str = null;
        Base64.Encoder base64 = Base64.getEncoder();//Base64编码器
        int num = binStr.length() / 8;
        byte[] binByte = new byte[num];//byte数组储存编码前的数据
        //二进制码串到byte的转换
        for (int i = 0; i < num; i++) {
            char temp = (char) Integer.parseInt(binStr.substring(i * 8, (i + 1) * 8), 2);
            binByte[i] = (byte) temp;
        }
        base64Str = base64.encodeToString(binByte);//编码
        return new StringBuilder(base64Str);
    }

    //Base64码串转换二进制码串
    public static StringBuilder base64ToBin(StringBuilder base64Str) {
        StringBuilder binStr = new StringBuilder();
        Base64.Decoder base64 = Base64.getDecoder();//Base64译码器
        byte[] binByte = base64.decode(base64Str.toString());//译码
        //byte到二进制码串的转换
        for (int i = 0; i < binByte.length; i++) {
            char temp = (char) binByte[i];
            StringBuilder binChar = new StringBuilder(Integer.toBinaryString((temp & 0xFF) + 0x100).substring(1));
            while (binChar.length() < 8) {
                binChar.insert(0, 0);
            }
            binStr.append(binChar);
        }
        return binStr;
    }

    //取二进制码串的左右半部
    public static StringBuilder[] splitLR(StringBuilder binStr) {
        StringBuilder[] temp = new StringBuilder[2];
        temp[0] = new StringBuilder(binStr.substring(0, binStr.length() / 2));//左部
        temp[1] = new StringBuilder(binStr.substring(binStr.length() / 2, binStr.length()));//右部
        return temp;
    }

    //交换二进制码串的左右半部
    public static StringBuilder swapLR(StringBuilder binStr) {
        StringBuilder temp = new StringBuilder();
        temp.append(binStr.substring(binStr.length() / 2, binStr.length()));
        temp.append(binStr.substring(0, binStr.length() / 2));
        return temp;
    }

    //填充二进制码串
    public static StringBuilder fillBinStr(StringBuilder binStr) {
        StringBuilder temp = new StringBuilder(binStr);
        //不满64位的二进制码串补0
        while (temp.length() < 64) {
            temp.insert(temp.length(), 0);
        }
        return temp;
    }

    //分割二进制码串
    public static StringBuilder[] splitBinStr(StringBuilder str, int specLength) {
        //用于判断分割后的子串数量
        int num = str.length() / specLength;
        int mod = str.length() % specLength;
        StringBuilder[] temp;
        if (mod == 0) {
            //整除的情况
            temp = new StringBuilder[num];
            for (int i = 0; i < num; i++) {
                StringBuilder splitOne = new StringBuilder();
                splitOne.append(str.substring(i * specLength, (i + 1) * specLength));
                temp[i] = splitOne;
            }
        } else {
            //非整除的情况
            temp = new StringBuilder[num + 1];
            for (int i = 0; i < num + 1; i++) {
                StringBuilder splitOne = new StringBuilder();
                if (i == num) {
                    splitOne.append(str.substring(i * specLength, str.length()));//最后一个子串长度不足specLength
                } else {
                    splitOne.append(str.substring(i * specLength, (i + 1) * specLength));
                }
                temp[i] = splitOne;
            }
        }
        return temp;
    }

    //判断是否为二进制字符串
    public static boolean isBinStr(StringBuilder str) {
        for (int i = 0; i < str.length(); i++) {
            if (str.charAt(i) != '0' && str.charAt(i) != '1') {
                return false;
            }
        }
        return true;
    }
}