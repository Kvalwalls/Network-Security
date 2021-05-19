package DESUtils;

public class DESHandler {
    //密钥处理器
    private DESKey desKey;
    //轮次数
    private static final int ROUND_NUM = 16;
    //文本长度
    private static final int TEXT_LENGTH = 64;
    //轮密钥长度
    private static final int SubKEY_LENGTH = 48;
    //选择代换表
    private static final int[][] S_Table = {
            //S1
            {
                    14, 4, 13, 1, 2, 15, 11, 8, 3, 10, 6, 12, 5, 9, 0, 7,
                    0, 15, 7, 4, 14, 2, 13, 1, 10, 6, 12, 11, 9, 5, 3, 8,
                    4, 1, 14, 8, 13, 6, 2, 11, 15, 12, 9, 7, 3, 10, 5, 0,
                    15, 12, 8, 2, 4, 9, 1, 7, 5, 11, 3, 14, 10, 0, 6, 13
            },
            //S2
            {
                    15, 1, 8, 14, 6, 11, 3, 4, 9, 7, 2, 13, 12, 0, 5, 10,
                    3, 13, 4, 7, 15, 2, 8, 14, 12, 0, 1, 10, 6, 9, 11, 5,
                    0, 14, 7, 11, 10, 4, 13, 1, 5, 8, 12, 6, 9, 3, 2, 15,
                    13, 8, 10, 1, 3, 15, 4, 2, 11, 6, 7, 12, 0, 5, 14, 9
            },
            //S3
            {
                    10, 0, 9, 14, 6, 3, 15, 5, 1, 13, 12, 7, 11, 4, 2, 8,
                    13, 7, 0, 9, 3, 4, 6, 10, 2, 8, 5, 14, 12, 11, 15, 1,
                    13, 6, 4, 9, 8, 15, 3, 0, 11, 1, 2, 12, 5, 10, 14, 7,
                    1, 10, 13, 0, 6, 9, 8, 7, 4, 15, 14, 3, 11, 5, 2, 12
            },
            //S4
            {
                    7, 13, 14, 3, 0, 6, 9, 10, 1, 2, 8, 5, 11, 12, 4, 15,
                    13, 8, 11, 5, 6, 15, 0, 3, 4, 7, 2, 12, 1, 10, 14, 9,
                    10, 6, 9, 0, 12, 11, 7, 13, 15, 1, 3, 14, 5, 2, 8, 4,
                    3, 15, 0, 6, 10, 1, 13, 8, 9, 4, 5, 11, 12, 7, 2, 14
            },
            //S5
            {
                    2, 12, 4, 1, 7, 10, 11, 6, 8, 5, 3, 15, 13, 0, 14, 9,
                    14, 11, 2, 12, 4, 7, 13, 1, 5, 0, 15, 10, 3, 9, 8, 6,
                    4, 2, 1, 11, 10, 13, 7, 8, 15, 9, 12, 5, 6, 3, 0, 14,
                    11, 8, 12, 7, 1, 14, 2, 13, 6, 15, 0, 9, 10, 4, 5, 3
            },
            //S6
            {
                    12, 1, 10, 15, 9, 2, 6, 8, 0, 13, 3, 4, 14, 7, 5, 11,
                    10, 15, 4, 2, 7, 12, 9, 5, 6, 1, 13, 14, 0, 11, 3, 8,
                    9, 14, 15, 5, 2, 8, 12, 3, 7, 0, 4, 10, 1, 13, 11, 6,
                    4, 3, 2, 12, 9, 5, 15, 10, 11, 14, 1, 7, 6, 0, 8, 13
            },
            //S7
            {
                    4, 11, 2, 14, 15, 0, 8, 13, 3, 12, 9, 7, 5, 10, 6, 1,
                    13, 0, 11, 7, 4, 9, 1, 10, 14, 3, 5, 12, 2, 15, 8, 6,
                    1, 4, 11, 13, 12, 3, 7, 14, 10, 15, 6, 8, 0, 5, 9, 2,
                    6, 11, 13, 8, 1, 4, 10, 7, 9, 5, 0, 15, 14, 2, 3, 12
            },
            //S8
            {
                    13, 2, 8, 4, 6, 15, 11, 1, 10, 9, 3, 14, 5, 0, 12, 7,
                    1, 15, 13, 8, 10, 3, 7, 4, 12, 5, 6, 11, 0, 14, 9, 2,
                    7, 11, 4, 1, 9, 12, 14, 2, 0, 6, 10, 13, 15, 3, 5, 8,
                    2, 1, 14, 7, 4, 10, 8, 13, 15, 12, 9, 0, 3, 5, 6, 11
            }
    };
    //IP置换表
    private static final int[] IP_Table = {
            58, 50, 42, 34, 26, 18, 10, 2,
            60, 52, 44, 36, 28, 20, 12, 4,
            62, 54, 46, 38, 30, 22, 14, 6,
            64, 56, 48, 40, 32, 24, 16, 8,
            57, 49, 41, 33, 25, 17, 9, 1,
            59, 51, 43, 35, 27, 19, 11, 3,
            61, 53, 45, 37, 29, 21, 13, 5,
            63, 55, 47, 39, 31, 23, 15, 7
    };
    //IP逆置换表
    private static final int[] NIP_Table = {
            40, 8, 48, 16, 56, 24, 64, 32,
            39, 7, 47, 15, 55, 23, 63, 31,
            38, 6, 46, 14, 54, 22, 62, 30,
            37, 5, 45, 13, 53, 21, 61, 29,
            36, 4, 44, 12, 52, 20, 60, 28,
            35, 3, 43, 11, 51, 19, 59, 27,
            34, 2, 42, 10, 50, 18, 58, 26,
            33, 1, 41, 9, 49, 17, 57, 25
    };
    //扩充置换表
    private static final int[] E_Table = {
            32, 1, 2, 3, 4, 5,
            4, 5, 6, 7, 8, 9,
            8, 9, 10, 11, 12, 13,
            12, 13, 14, 15, 16, 17,
            16, 17, 18, 19, 20, 21,
            20, 21, 22, 23, 24, 25,
            24, 25, 26, 27, 28, 29,
            28, 29, 30, 31, 32, 1
    };
    //置换表
    private static final int[] P_Table = {
            16, 7, 20, 21,
            29, 12, 28, 17,
            1, 15, 23, 26,
            5, 18, 31, 10,
            2, 8, 24, 14,
            32, 27, 3, 9,
            19, 13, 30, 6,
            22, 11, 4, 25
    };

    //构造器方法
    public DESHandler() {
        this.desKey = new DESKey();
    }

    //构造器方法
    public DESHandler(StringBuilder k) {
        this.desKey = new DESKey(k);
    }

    //获取密钥
    public StringBuilder getKey() {
        return desKey.getKey();
    }

    //IP置换
    private StringBuilder permuteIP(StringBuilder str) {
        StringBuilder reStr = new StringBuilder();
        for (int i = 0; i < TEXT_LENGTH; i++) {
            reStr.append(str.charAt(IP_Table[i] - 1));
        }
        return reStr;
    }

    //IP逆置换
    private StringBuilder permuteNIP(StringBuilder str) {
        StringBuilder reStr = new StringBuilder();
        for (int i = 0; i < TEXT_LENGTH; i++) {
            reStr.append(str.charAt(NIP_Table[i] - 1));
        }
        return reStr;
    }

    //数据扩充置换
    private StringBuilder extendPermute(StringBuilder str) {
        StringBuilder reStr = new StringBuilder();
        for (int i = 0; i < SubKEY_LENGTH; i++) {
            reStr.append(str.charAt(E_Table[i] - 1));
        }
        return reStr;
    }

    //数据选择置换
    private StringBuilder selectPermute(StringBuilder str) {
        StringBuilder reStr = new StringBuilder();
        for (int i = 0; i < 8; i++) {
            String SBoxInput = str.substring(i * 6, (i + 1) * 6);//每6位进行选择置换
            //计算行列号
            int row = Integer.parseInt(Character.toString(SBoxInput.charAt(0)) + SBoxInput.charAt(5), 2);
            int column = Integer.parseInt(SBoxInput.substring(1, 5), 2);
            StringBuilder SBoxOutput = new StringBuilder(Integer.toBinaryString(S_Table[i][row * 16 + column]));//计算输出的二进制码串
            //不足4位的进行补0
            while (SBoxOutput.length() < 4) {
                SBoxOutput.insert(0, 0);
            }
            reStr.append(SBoxOutput);
        }
        return reStr;
    }

    //数据置换
    private StringBuilder permute(StringBuilder str) {
        StringBuilder reStr = new StringBuilder();
        for (int i = 0; i < TEXT_LENGTH / 2; i++) {
            reStr.append(str.charAt(P_Table[i] - 1));
        }
        return reStr;
    }

    //轮函数
    private StringBuilder FeiRound(StringBuilder str, StringBuilder subKey) {
        StringBuilder[] LR = DESTools.splitLR(str);//划分左右半部
        StringBuilder newR = extendPermute(LR[1]);//扩充置换
        newR = DESTools.XOR(newR, subKey);//异或subKey
        newR = selectPermute(newR);//选择置换
        newR = permute(newR);//置换
        newR = DESTools.XOR(newR, LR[0]);//异或左半部
        LR[1].append(newR);//右半部后拼接新右半部
        return LR[1];
    }

    //加密
    public String DESEncrypt(String text) {
        StringBuilder ciphertext = new StringBuilder();
        StringBuilder plaintext = new StringBuilder(text);
        StringBuilder texts[] = DESTools.splitBinStr(plaintext, TEXT_LENGTH);
        for (int i = 0; i < texts.length; i++) {
            //不满64位的二进制码串进行填充
            if (texts[i].length() < TEXT_LENGTH) {
                texts[i] = DESTools.fillBinStr(texts[i]);
            }
            //IP置换
            texts[i] = permuteIP(texts[i]);
            for (int j = 0; j < ROUND_NUM; j++) {
                texts[i] = FeiRound(texts[i], desKey.getSubKeys()[j]);//第j轮使用第j个轮密钥
            }
            //交换左右半部
            texts[i] = DESTools.swapLR(texts[i]);
            //IP逆置换
            texts[i] = permuteNIP(texts[i]);
            ciphertext.append(texts[i]);
        }
        return ciphertext.toString();
    }

    //解密
    public String DESDecrypt(String text) {
        StringBuilder plaintext = new StringBuilder();
        StringBuilder ciphertext = new StringBuilder(text);
        StringBuilder texts[] = DESTools.splitBinStr(ciphertext, TEXT_LENGTH);
        for (int i = 0; i < texts.length; i++) {
            //不满64位的二进制码串进行填充
            if (texts[i].length() < TEXT_LENGTH) {
                texts[i] = DESTools.fillBinStr(texts[i]);
            }
            //IP置换
            texts[i] = permuteIP(texts[i]);
            for (int j = 0; j < ROUND_NUM; j++) {
                texts[i] = FeiRound(texts[i], desKey.getSubKeys()[ROUND_NUM - 1 - j]);//第j轮使用第ROUND_NUM - 1 - j个轮密钥
            }
            //交换左右半部
            texts[i] = DESTools.swapLR(texts[i]);
            //IP逆置换
            texts[i] = permuteNIP(texts[i]);
            plaintext.append(texts[i]);
        }
        return plaintext.toString();
    }
}