package TransmissionUtils;

public class IntBytesPhaser {
    /**
     * int转换byte数组方法
     *
     * @param intNum 数字的int形式
     * @return 数字的byte数组形式
     */
    public static byte[] intToBytes(int intNum) {
        byte[] bytesNum = new byte[4];
        for (int i = 0; i < 4; i++)
            bytesNum[i] = (byte) ((intNum >> i * 8) & 0xff);
        return bytesNum;
    }

    /**
     * byte数组转换int方法
     *
     * @param bytesNum 数字的byte数组形式
     * @return 数字的int形式
     */
    public static int bytesToInt(byte[] bytesNum) {
        int intNum = 0;
        for (int i = 0; i < 4; i++)
            intNum += (bytesNum[i] & 0xff) << (i * 8);
        return intNum;
    }
}