﻿using CommonUser.Security;
using System;
using System.Collections.Generic;
using System.Text;

namespace CommonUser.Transmission
{
    class TransMessage
    {
        //目的IP地址
        public byte[] toAddress { get; set; }
        //源IP地址
        public byte[] fromAddress { get; set; }
        //服务类型
        public byte serviceType { get; set; }
        //具体类型
        public byte specificType { get; set; }
        //错误码
        public byte errorCode { get; set; }
        //加密码
        public byte cryptCode { get; set; }
        //数字签名
        public string signature { get; set; }
        //报文内容
        public string contents { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public TransMessage(byte[] tAddr, byte[] fAddr, byte serviceT, byte specificT, byte cryptC, string con)
        {
            toAddress = tAddr;
            fromAddress = fAddr;
            serviceType = serviceT;
            specificType = specificT;
            cryptCode = cryptC;
            contents = con;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public TransMessage()
        {
            toAddress = new byte[4];
            fromAddress = new byte[4];
        }

        /// <summary>
        /// 报文封装函数
        /// </summary>
        /// <param name="rsaSKeyFile">RSA私钥文件名</param>
        /// <param name="desKey">DES密钥</param>
        public void EnPackage(string rsaSKeyFile, string desKey)
        {
            try
            {
                signature = RSAHandler.GenerateSign(rsaSKeyFile, contents);
                if (cryptCode == 1)
                    contents = DESHandler.Encrypt(desKey, contents);
                errorCode = 0;
            }
            catch (Exception ex)
            {
                errorCode = 1;
            }
        }

        /// <summary>
        /// 报文解封函数
        /// </summary>
        /// <param name="rsaPKeyFile">RSA公钥文件名</param>
        /// <param name="desKey">DES密钥</param>
        public void DePackage(string rsaPKeyFile, string desKey)
        {
            try
            {
                if (cryptCode == 1)
                    contents = DESHandler.Decrypt(desKey, contents);
                if (!RSAHandler.VerifySign(rsaPKeyFile, signature, contents))
                    errorCode = 1;
            }
            catch(Exception ex)
            {
                errorCode = 1;
            }
        }

        /// <summary>
        /// 报文转换字节流函数
        /// </summary>
        /// <returns>字节流</returns>
        public byte[] MessageToBytes()
        {
            List<byte> byteList = new List<byte>();
            byteList.AddRange(toAddress);
            byteList.AddRange(fromAddress);
            byteList.Add(serviceType);
            byteList.Add(specificType);
            byteList.Add(errorCode);
            byteList.Add(cryptCode);
            byteList.AddRange(Encoding.UTF8.GetBytes(string.Format("{0,-4}", signature.Length)));
            byteList.AddRange(Encoding.UTF8.GetBytes(string.Format("{0,-4}", contents.Length)));
            byteList.AddRange(Encoding.UTF8.GetBytes(signature));
            byteList.AddRange(Encoding.UTF8.GetBytes(contents));
            return byteList.ToArray();
        }
    }
}