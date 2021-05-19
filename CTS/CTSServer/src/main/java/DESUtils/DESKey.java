package DESUtils;

import java.util.Random;

public class DESKey {
    //密钥
    private StringBuilder key;
    //轮密钥
    private StringBuilder[] subKeys;
    //轮次数
    private static final int ROUND_NUM = 16;
    //密钥长度
    private static final int KEY_LENGTH = 56;
    //轮密钥长度
    private static final int SubKEY_LENGTH = 48;
    //密钥紧缩置换表
    private final static int[] C_Table = {
            14, 17, 11, 24, 1, 5,
            3, 28, 15, 6, 21, 10,
            23, 19, 12, 4, 26, 8,
            16, 7, 27, 20, 13, 2,
            41, 52, 31, 37, 47, 55,
            30, 40, 51, 45, 33, 48,
            44, 49, 39, 56, 34, 53,
            46, 42, 50, 36, 29, 32
    };
    //密钥移位数表
    private static final int[] KS_Table = {
            1, 1, 2, 2, 2, 2, 2, 2,
            1, 2, 2, 2, 2, 2, 2, 1
    };

    //构造器方法
    public DESKey() {
        this.key = new StringBuilder();
        //产生随机密钥
        Random r = new Random();
        for (int i = 0; i < KEY_LENGTH; i++) {
            int ranInt = r.nextInt(2);
            this.key.append(Integer.toString(ranInt));
        }
        generateSubKeys();//生成轮密钥
    }

    //构造器方法
    public DESKey(StringBuilder k) {
        if (!DESTools.isBinStr(k)) {
            this.key = DESTools.stringToBinStr(k);//非二进制码串转换二进制码串
        } else {
            this.key = k;//二进制码串直接赋值
        }
        if (this.key.length() > KEY_LENGTH) {
            this.key.delete(KEY_LENGTH, key.length());//过长的二进制码串截断
        } else {
            //过短的二进制码串填充
            while (key.length() < KEY_LENGTH) {
                key.append(0);
            }
        }
        generateSubKeys();//生成轮密钥
    }

    //获取密钥
    public StringBuilder getKey() {
        return this.key;
    }

    //获取轮密钥
    public StringBuilder[] getSubKeys() {
        return this.subKeys;
    }

    //密钥移位
    private StringBuilder shiftKey(int step, StringBuilder oldKey) {
        StringBuilder newKey = new StringBuilder(oldKey);
        //默认step为1
        if (step <= 0) {
            step = 1;
        }
        //前28位密钥移位
        for (int i = 0; i < KEY_LENGTH / 2; i++) {
            //前半部分转置
            if (i < step / 2) {
                char temp = newKey.charAt(i);
                newKey.setCharAt(i, newKey.charAt(step - 1 - i));
                newKey.setCharAt(step - 1 - i, temp);
            }
            //后半部分转置
            if (i < (KEY_LENGTH / 2 - step) / 2) {
                char temp = newKey.charAt(i + step);
                newKey.setCharAt(i + step, newKey.charAt(KEY_LENGTH / 2 - 1 - i));
                newKey.setCharAt(KEY_LENGTH / 2 - 1 - i, temp);
            }
            //整个转置
            if (i < KEY_LENGTH / 4) {
                char temp = newKey.charAt(i);
                newKey.setCharAt(i, newKey.charAt(KEY_LENGTH / 2 - 1 - i));
                newKey.setCharAt(KEY_LENGTH / 2 - 1 - i, temp);
            }
        }
        //后28位密钥移位
        for (int i = 0; i < KEY_LENGTH / 2; i++) {
            //前半部分转置
            if (i < step / 2) {
                char temp = newKey.charAt(i + KEY_LENGTH / 2);
                newKey.setCharAt(i + KEY_LENGTH / 2, newKey.charAt(step - 1 - i + KEY_LENGTH / 2));
                newKey.setCharAt(step - 1 - i + KEY_LENGTH / 2, temp);
            }
            //后半部分转置
            if (i < (KEY_LENGTH / 2 - step) / 2) {
                char temp = newKey.charAt(i + step + KEY_LENGTH / 2);
                newKey.setCharAt(i + step + KEY_LENGTH / 2, newKey.charAt(KEY_LENGTH - 1 - i));
                newKey.setCharAt(KEY_LENGTH - 1 - i, temp);
            }
            //整个转置
            if (i < KEY_LENGTH / 4) {
                char temp = newKey.charAt(i + KEY_LENGTH / 2);
                newKey.setCharAt(i + KEY_LENGTH / 2, newKey.charAt(KEY_LENGTH - 1 - i));
                newKey.setCharAt(KEY_LENGTH - 1 - i, temp);
            }
        }
        return newKey;
    }

    //密钥压缩置换
    private StringBuilder compressPermuteKey(StringBuilder oldKey) {
        StringBuilder newKey = new StringBuilder();
        //轮密钥的产生
        for (int i = 0; i < SubKEY_LENGTH; i++) {
            newKey.append(oldKey.charAt(C_Table[i] - 1));
        }
        return newKey;
    }

    //生成轮密钥
    private void generateSubKeys() {
        this.subKeys = new StringBuilder[ROUND_NUM];
        StringBuilder temp = new StringBuilder(this.key);
        //生成ROUND_NUM个轮密钥
        for (int i = 0; i < ROUND_NUM; i++) {
            temp = shiftKey(KS_Table[i], temp);
            subKeys[i] = compressPermuteKey(temp);
        }
    }
}