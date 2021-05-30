namespace CommonUser.Transmission
{
    class AddressPhaser
    {
        /// <summary>
        /// 字符串形式转换字节流形式
        /// </summary>
        /// <param name="addrStr">字符串形式的IP地址</param>
        /// <returns>字节流形式的IP地址</returns>
        public static byte[] StringToBytes(string addrStr)
        {
            string[] subAddrStrs = addrStr.Replace("\\s", "").Split('.');
            byte[] subAddrBytes = new byte[4];
            for (int i = 0; i < 4; i++)
                subAddrBytes[i] = byte.Parse(subAddrStrs[i]);
            return subAddrBytes;
        }

        /// <summary>
        /// 字节流形式转换字符串形式
        /// </summary>
        /// <param name="addrBytes">字节流形式的IP地址</param>
        /// <returns>字符串形式的IP地址</returns>
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
