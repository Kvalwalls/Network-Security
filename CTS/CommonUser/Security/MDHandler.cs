using System;
using System.Security.Cryptography;
using System.Text;

namespace CommonUser.Security
{
    class MDHandler
    {
        public static string GenerateMD(string plainText)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] md5Bytes = md5.ComputeHash(Encoding.UTF8.GetBytes(plainText));
            return Convert.ToBase64String(md5Bytes);
        }

        public static bool VerifyMD(string mdText, string plainText)
        {
            return mdText.Equals(GenerateMD(plainText));
        }
    }
}
