package TransmissionUtils;

public class AddressPhaser {
    public static byte[] StringToBytes(String addrStr) {
        String[] subAddrStrings = addrStr.replace("\\s", "").split("\\.");
        byte[] subAddrBytes = new byte[4];
        for (int i = 0; i < 4; i++)
            subAddrBytes[i] = Byte.parseByte(subAddrStrings[i]);
        return subAddrBytes;
    }

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
