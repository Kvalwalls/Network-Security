using System.Text;

namespace AdminUser.Transmission
{
    class AddressPhaser
    {
        /// <summary>
        /// 字符串形式转换字节流形式
        /// </summary>
        /// <param name="addrStr">字符串形式的IP地址</param>
        /// <returns>字节流形式的IP地址</returns>
        public static byte[] StringToBytes(string addrString)
        {
            string[] subAddrStrings = addrString.Replace("\\s", "").Split('.');
            byte[] addrBytes = new byte[4];
            for (int i = 0; i < 4; i++)
                addrBytes[i] = byte.Parse(subAddrStrings[i]);
            return addrBytes;
        }

        /// <summary>
        /// 字节流形式转换字符串形式
        /// </summary>
        /// <param name="addrBytes">字节流形式的IP地址</param>
        /// <returns>字符串形式的IP地址</returns>
        public static string BytesToString(byte[] addrBytes)
        {
            StringBuilder addrString = new StringBuilder();
            for (int i = 0; i < 4; i++)
            {
                addrString.Append(addrBytes[i].ToString());
                if (i != 3)
                    addrString.Append(".");
            }
            return addrString.ToString();
        }
    }
}