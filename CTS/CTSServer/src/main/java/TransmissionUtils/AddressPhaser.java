package TransmissionUtils;

public class AddressPhaser {
    /**
     * 字符串地址转换字节流地址方法
     *
     * @param addrStr 字符串形式的地址
     * @return 字节流形式的地址
     */
    public static byte[] StringToBytes(String addrStr) {
        String[] subAddrStrings = addrStr.replace("\\s", "").split("\\.");
        byte[] subAddrBytes = new byte[4];
        for (int i = 0; i < 4; i++)
            subAddrBytes[i] = Byte.parseByte(subAddrStrings[i]);
        return subAddrBytes;
    }

    /**
     * 字节流地址转换字符串地址方法
     *
     * @param addrBytes 字节流形式的地址
     * @return 字符串形式的地址
     */
    public static String BytesToString(byte[] addrBytes) {
        StringBuilder addrStr = new StringBuilder();
        for (int i = 0; i < 4; i++) {
            addrStr.append(addrBytes[i]);
            if (i != 3)
                addrStr.append(".");
        }
        return addrStr.toString();
    }
}