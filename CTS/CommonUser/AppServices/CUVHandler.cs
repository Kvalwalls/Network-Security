using CommonUser.Entity;
using CommonUser.Kerberos;
using CommonUser.Transmission;
using System;
using System.Configuration;
using System.Net.Sockets;
using System.Xml;

namespace CommonUser.AppServices
{
    class CUVHandler : VHandler
    {
        //会话DES密钥
        private readonly string sessionKey;
        //本机IP地址
        private byte[] fromAddr = AddressPhaser.StringToBytes(ConfigurationManager.AppSettings["My_IPAddress"]);
        //CUV服务器IP地址
        private byte[] toAddr = AddressPhaser.StringToBytes(ConfigurationManager.AppSettings["V_IPAddress"]);
        //CUVHandler实例
        private static CUVHandler instance = new CUVHandler();
        

        /// <summary>
        /// 私有构造函数
        /// </summary>
        private CUVHandler()
        {
            //AS认证过程
            ASHandler asHandler = ASHandler.GetInstance();
            string[] ASKeyTicket = asHandler.ASCertification();
            if (ASKeyTicket == null)
                throw new Exception("AS认证错误！");
            asHandler.CloseASConnection();
            //TGS认证过程
            TGSHandler tgsHandler = TGSHandler.GetInstance();
            string[] TGSKeyTicket = tgsHandler.TGSCertification(ASKeyTicket[0], ASKeyTicket[1]);
            if (TGSKeyTicket == null)
                throw new Exception("TGS认证错误！");
            tgsHandler.CloseTGSConnection();
            //V认证过程
            Socket socket = Connection.ConnectServer(
               IPStr: ConfigurationManager.AppSettings["V_IPAddress"],
               int.Parse(ConfigurationManager.AppSettings["V_Port"]));
            transceiver = new Transceiver(socket);
            if (transceiver == null)
                throw new Exception("Transceiver初始化错误！");
            sessionKey = VCertification(TGSKeyTicket[0], TGSKeyTicket[1]);
            if (sessionKey == null)
                throw new Exception("V认证错误！");
        }

        /// <summary>
        /// 获取CUVHandler实例函数
        /// </summary>
        /// <returns></returns>
        public static CUVHandler GetInstance()
        {
            return instance;
        }

        /// <summary>
        /// 登录验证函数
        /// </summary>
        /// <param name="Uid">人员号</param>
        /// <param name="Upassword">密码</param>
        /// <returns>登录User对象</returns>
        public User Login(string Uid, string Upassword)
        {
            //创建XMLDocument
            XmlDocument document = new XmlDocument();
            //根节点
            XmlElement loginElement = document.CreateElement("login");
            //子节点
            XmlElement u_idElement = document.CreateElement("u_id");
            u_idElement.InnerText = Uid;
            XmlElement u_passwordElement = document.CreateElement("u_password");
            u_passwordElement.InnerText = Upassword;
            //形成树结构
            loginElement.AppendChild(u_idElement);
            loginElement.AppendChild(u_passwordElement);
            document.AppendChild(loginElement);
            //报文初始化
            TransMessage message = new TransMessage();
            message.fromAddress = fromAddr;
            message.toAddress = toAddr;
            message.serviceType = EnumServiceType.CUV;
            message.specificType = EnumKerberos.Request;
            message.contents = XMLPhaser.XmlToString(document);
            message.EnPackage(ConfigurationManager.AppSettings["My_SKeyFile"], null);
            transceiver.SendMessage(message);
            message = transceiver.ReceiveMessage();

            
        }
        
    }
}
