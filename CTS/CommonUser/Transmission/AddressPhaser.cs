namespace CommonUser.Transmission
{
    class AddressPhaser
    {
        public static byte[] StringToBytes(string addrStr)
        {
            string[] subAddrStrs = addrStr.Replace("\\s", "").Split('.');
            byte[] subAddrBytes = new byte[4];
            for (int i = 0; i < 4; i++)
                subAddrBytes[i] = byte.Parse(subAddrStrs[i]);
            return subAddrBytes;
        }

        public static string BytesToString(byte[] addrBytes)
        {
            string addrStr = "";
            for (int i = 0; i < 4; i++)
            {
                addrStr += addrBytes[i].ToString();
                if (i != 3)
                    addrStr += ".";
            }
            return addrStr;
        }
    }
}
