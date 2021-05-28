using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace CommonUser.Security
{
    class DESHandler
    {
        /// <summary>
        /// DES加密函数
        /// </summary>
        /// <param name="key">密钥</param>
        /// <param name="plainText">明文</param>
        /// <returns>Base格式的密文</returns>
        public static string Encrypt(string key, string plainText)
        {
            //key的扩充与缩减
            if (key.Length < 8)
                while (key.Length < 8)
                    key += "0";
            else if (key.Length > 8)
                key = key.Substring(0, 8);
            //DES加密对象初始化
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            des.Mode = CipherMode.CBC;//CBC模式
            des.Padding = PaddingMode.PKCS7;//PKCS7填充模式
            des.Key = des.IV = Encoding.UTF8.GetBytes(key);//设置密钥
            //明文格式字符串转换byte
            byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);
            //DES加密过程
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
            cs.Write(plainBytes, 0, plainBytes.Length);
            cs.FlushFinalBlock();
            return Convert.ToBase64String(ms.ToArray());//转换为Base格式
        }

        /// <summary>
        /// DES解密函数
        /// </summary>
        /// <param name="key">密钥</param>
        /// <param name="cipherText">Base格式的密文</param>
        /// <returns>明文</returns>
        public static string Decrypt(string key, string cipherText)
        {
            //key的扩充与缩减
            if (key.Length < 8)
                while (key.Length < 8)
                    key += "0";
            else if (key.Length > 8)
                key = key.Substring(0, 8);
            //DES解密对象初始化
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            des.Mode = CipherMode.CBC;//CBC模式
            des.Padding = PaddingMode.PKCS7;//PKCS7填充模式
            des.Key = des.IV = Encoding.UTF8.GetBytes(key);//设置密钥
            //Base64格式字符串转换byte
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            //DES解密过程
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
            cs.Write(cipherBytes, 0, cipherBytes.Length);
            cs.FlushFinalBlock();
            return Encoding.UTF8.GetString(ms.ToArray());//转换为明文格式
        }
    }
}