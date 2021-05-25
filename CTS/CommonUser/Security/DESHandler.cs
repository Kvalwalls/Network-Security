using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace CommonUser.Security
{
    class DESHandler
    {
        public static string Encrypt(string key, string plainText)
        {
            if (key.Length < 8)
                while (key.Length < 8)
                    key += "\0";
            else if (key.Length > 8)
                key = key.Substring(0, 8);
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            des.Mode = CipherMode.ECB;
            des.Key = Encoding.UTF8.GetBytes(key);
            des.Padding = PaddingMode.PKCS7;
            byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
            cs.Write(plainBytes, 0, plainBytes.Length);
            cs.FlushFinalBlock();
            return Convert.ToBase64String(ms.ToArray());
        }

        public static string Decrypt(string key, string cipherText)
        {
            if (key.Length < 8)
                while (key.Length < 8)
                    key += "\0";
            else if (key.Length > 8)
                key = key.Substring(0, 8);
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            des.Mode = CipherMode.ECB;
            des.Key = Encoding.UTF8.GetBytes(key);
            des.Padding = PaddingMode.PKCS7;
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
            cs.Write(cipherBytes, 0, cipherBytes.Length);
            cs.FlushFinalBlock();
            return Encoding.UTF8.GetString(ms.ToArray());
        }
    }
}
