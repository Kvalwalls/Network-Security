package TransmissionUtils;

public class AddressPhaser {
    /**
     * 字符串地址转换字节流地址方法
     *
     * @param addrStr 字符串形式的地址
     * @return 字节流形式的地址
     */
    public static byte[] stringToBytes(String addrString) {
        String[] subAddrStrings = addrString.replace("\\s", "").split("\\.");
        byte[] addrBytes = new byte[4];
        for (int i = 0; i < 4; i++) {
            int temp = Integer.parseInt(subAddrStrings[i]);
            temp = temp > 127 ? (temp - 256) : temp;
            addrBytes[i] = (byte) temp;
        }
        return addrBytes;
    }

    /**
     * 字节流地址转换字符串地址方法
     *
     * @param addrBytes 字节流形式的地址
     * @return 字符串形式的地址
     */
    public static String bytesToString(byte[] addrBytes) {
        StringBuilder addrString = new StringBuilder();
        for (int i = 0; i < 4; i++) {
            int temp = Integer.parseInt(String.valueOf(addrBytes[i]));
            temp = temp < 0 ? (temp + 256) : temp;
            addrString.append(temp);
            if (i != 3)
                addrString.append(".");
        }
        return addrString.toString();
    }
}