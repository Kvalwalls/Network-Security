namespace AdminUser.Transmission
{
    class IntBytesPhaser
    {
        /// <summary>
        /// int转换byte数组方法
        /// </summary>
        /// <param name="intNum">数字的int形式</param>
        /// <returns>数字的byte数组形式</returns>
        public static byte[] IntToBytes(int intNum)
        {
            byte[] bytesNum = new byte[4];
            for (int i = 0; i < 4; i++)
                bytesNum[i] = (byte)((intNum >> i * 8) & 0xff);
            return bytesNum;
        }

        /// <summary>
        /// byte数组转换int方法
        /// </summary>
        /// <param name="bytesNum">数字的byte数组形式</param>
        /// <returns>数字的int形式</returns>
        public static int BytesToInt(byte[] bytesNum)
        {
            int intNum = 0;
            for (int i = 0; i < 4; i++)
                intNum += (bytesNum[i] & 0xff) << (i * 8);
            return intNum;
        }
    }
}
