using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Test
{
    class RSAHandler
    {
        /// <summary>
        /// RSA加密函数
        /// </summary>
        /// <param name="pKeyFile">公钥文件名</param>
        /// <param name="plainText">明文</param>
        /// <returns>Base64格式的密文</returns>
        public static string Encrypt(string pKeyFile, string plainText)
        {
            //RSA加密对象初始化
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            string publicKeyStr = File.ReadAllText(pKeyFile);
            rsa.FromXmlString(getRSAPublicKey(publicKeyStr));
            int bufferSize = (rsa.KeySize / 8) - 11;
            byte[] buffer = new byte[bufferSize];
            byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);
            //分块加密
            using (MemoryStream inputStream = new MemoryStream(plainBytes), outputStream = new MemoryStream())
            {
                while (true)
                {
                    int readSize = inputStream.Read(buffer, 0, bufferSize);
                    if (readSize <= 0)
                        break;
                    byte[] temp = new byte[readSize];
                    Array.Copy(buffer, 0, temp, 0, readSize);
                    byte[] cipherBytes = rsa.Encrypt(temp, false);
                    outputStream.Write(cipherBytes, 0, cipherBytes.Length);
                }
                return Convert.ToBase64String(outputStream.ToArray());
            }
        }

        /// <summary>
        /// RSA解密函数
        /// </summary>
        /// <param name="sKeyFile">私钥文件名</param>
        /// <param name="cipherText">Base64格式的密文</param>
        /// <returns>明文</returns>
        public static string Decrypt(string sKeyFile, string cipherText)
        {
            //RSA解密对象初始阿虎
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            string privateKeyStr = File.ReadAllText(sKeyFile);
            rsa.FromXmlString(getRSAPrivateKey(privateKeyStr));
            int bufferSize = rsa.KeySize / 8;
            byte[] buffer = new byte[bufferSize];
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            //分块解密
            using (MemoryStream inputStream = new MemoryStream(cipherBytes), outputStream = new MemoryStream())
            {
                while (true)
                {
                    int readSize = inputStream.Read(buffer, 0, bufferSize);
                    if (readSize <= 0)
                        break;
                    byte[] temp = new byte[readSize];
                    Array.Copy(buffer, 0, temp, 0, readSize);
                    byte[] plainBytes = rsa.Decrypt(temp, false);
                    outputStream.Write(plainBytes, 0, plainBytes.Length);
                }
                return Encoding.UTF8.GetString(outputStream.ToArray());
            }
        }

        /// <summary>
        /// 生成数字签名函数
        /// </summary>
        /// <param name="sKeyFile">私钥文件名</param>
        /// <param name="text">报文</param>
        /// <returns>Base64格式的数字签名</returns>
        public static string GenerateSign(string sKeyFile, string text)
        {
            //RSA私钥初始化
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            string privateKeyStr = File.ReadAllText(sKeyFile);
            rsa.FromXmlString(getRSAPrivateKey(privateKeyStr));
            //MD5生成信息摘要
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] hashBytes = md5.ComputeHash(Encoding.UTF8.GetBytes(text));
            //信息摘要生成数字签名
            RSAPKCS1SignatureFormatter formatter = new RSAPKCS1SignatureFormatter(rsa);
            formatter.SetHashAlgorithm("MD5");
            return Convert.ToBase64String(formatter.CreateSignature(hashBytes));
        }

        /// <summary>
        /// 验证数字签名函数
        /// </summary>
        /// <param name="pKeyFile">公钥文件名</param>
        /// <param name="sign">Base64格式的数字签名</param>
        /// <param name="text">报文</param>
        /// <returns>验证结果</returns>
        public static bool VerifySign(string pKeyFile, string sign, string text)
        {
            //RSA公钥初始化
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            string publicKeyStr = File.ReadAllText(pKeyFile);
            rsa.FromXmlString(getRSAPublicKey(publicKeyStr));
            //MD5生成信息摘要
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] hashBytes = md5.ComputeHash(Encoding.UTF8.GetBytes(text));
            //信息摘要验证数字签名
            RSAPKCS1SignatureDeformatter deformatter = new RSAPKCS1SignatureDeformatter(rsa);
            deformatter.SetHashAlgorithm("MD5");
            return deformatter.VerifySignature(hashBytes, Convert.FromBase64String(sign));
        }

        /// <summary>
        /// 获取公钥函数
        /// </summary>
        /// <param name="publicKeyStr">PEM格式的公钥</param>
        /// <returns>标准公钥对象</returns>
        private static string getRSAPublicKey(string publicKeyStr)
        {
            //替换无关字符
            publicKeyStr = publicKeyStr.Replace("-----BEGIN PUBLIC KEY-----", "");
            publicKeyStr = publicKeyStr.Replace("-----END PUBLIC KEY-----", "");
            publicKeyStr = publicKeyStr.Replace("\r", "");
            publicKeyStr = publicKeyStr.Replace("\n", "");
            //初始化标准公钥对象
            RsaKeyParameters publicKeyParam = (RsaKeyParameters)PublicKeyFactory.CreateKey(
                Convert.FromBase64String(publicKeyStr));
            return string.Format("<RSAKeyValue><Modulus>{0}</Modulus><Exponent>{1}</Exponent></RSAKeyValue>",
                Convert.ToBase64String(publicKeyParam.Modulus.ToByteArrayUnsigned()),
                Convert.ToBase64String(publicKeyParam.Exponent.ToByteArrayUnsigned()));
        }

        /// <summary>
        /// 获取私钥函数
        /// </summary>
        /// <param name="privateKeyStr">PEN格式的私钥</param>
        /// <returns>标准私钥对象</returns>
        private static string getRSAPrivateKey(string privateKeyStr)
        {
            //替换无关字符
            privateKeyStr = privateKeyStr.Replace("-----BEGIN PRIVATE KEY-----", "");
            privateKeyStr = privateKeyStr.Replace("-----END PRIVATE KEY-----", "");
            privateKeyStr = privateKeyStr.Replace("\r", "");
            privateKeyStr = privateKeyStr.Replace("\n", "");
            //初始化标准私钥对象
            RsaPrivateCrtKeyParameters privateKeyParam = (RsaPrivateCrtKeyParameters)PrivateKeyFactory.CreateKey(
                Convert.FromBase64String(privateKeyStr));
            return string.Format("<RSAKeyValue><Modulus>{0}</Modulus><Exponent>{1}</Exponent><P>{2}</P><Q>{3}</Q><DP>{4}</DP><DQ>{5}</DQ><InverseQ>{6}</InverseQ><D>{7}</D></RSAKeyValue>",
                Convert.ToBase64String(privateKeyParam.Modulus.ToByteArrayUnsigned()),
                Convert.ToBase64String(privateKeyParam.PublicExponent.ToByteArrayUnsigned()),
                Convert.ToBase64String(privateKeyParam.P.ToByteArrayUnsigned()),
                Convert.ToBase64String(privateKeyParam.Q.ToByteArrayUnsigned()),
                Convert.ToBase64String(privateKeyParam.DP.ToByteArrayUnsigned()),
                Convert.ToBase64String(privateKeyParam.DQ.ToByteArrayUnsigned()),
                Convert.ToBase64String(privateKeyParam.QInv.ToByteArrayUnsigned()),
                Convert.ToBase64String(privateKeyParam.Exponent.ToByteArrayUnsigned()));
        }
    }
}
