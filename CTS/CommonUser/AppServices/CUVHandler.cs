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
        //本机私钥文件
        private string mySKeyFile = ConfigurationManager.AppSettings["My_SKeyFile"];
        //服务器公钥文件
        private string vPKeyFile = ConfigurationManager.AppSettings["V_PKeyFile"];
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
        /// 登录函数
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
            message.specificType = EnumCUV.Login;
            message.contents = XMLPhaser.XmlToString(document);
            message.EnPackage(mySKeyFile, sessionKey);
            transceiver.SendMessage(message);
            message = transceiver.ReceiveMessage();
            message.DePackage(vPKeyFile, sessionKey);
            document = XMLPhaser.StringToXml(message.contents);
            if (message.errorCode == EnumErrorCode.Error)
                throw new Exception("登录函数错误！");
            User user = null;
            XmlElement xmlRoot = document.DocumentElement;
            if ("true".Equals(xmlRoot["state"].InnerText))
            {
                user = new User();
                XmlNodeList userNode = xmlRoot["user"].ChildNodes;
                foreach (XmlNode node in userNode)
                {
                    switch (node.Name)
                    {
                        case "u_id":
                            user.Uid = node.InnerText;
                            break;
                        case "u_name":
                            user.Uname = node.InnerText;
                            break;
                        case "u_password":
                            user.Upassword = node.InnerText;
                            break;
                        case "u_money":
                            user.Umoney = float.Parse(node.InnerText);
                            break;
                        case "u_access":
                            user.Uaccess = byte.Parse(node.InnerText);
                            break;
                    }
                }
            }
            return user;
        }

        public bool Register(User user)
        {
            //创建XMLDocument
            XmlDocument document = new XmlDocument();
            //根节点
            XmlElement registerElement = document.CreateElement("register");
            //子节点
            XmlElement u_idElement = document.CreateElement("u_id");
            u_idElement.InnerText = user.Uid;
            XmlElement u_nameElement = document.CreateElement("u_name");
            u_nameElement.InnerText = user.Uname;
            XmlElement u_passwordElement = document.CreateElement("u_password");
            u_passwordElement.InnerText = user.Upassword;
            XmlElement u_accessElement = document.CreateElement("u_access");
            u_accessElement.InnerText = EnumUserAccess.U_Comm.ToString();
            XmlElement u_moneyElement = document.CreateElement("u_money");
            u_moneyElement.InnerText = "0";
            //形成树结构
            registerElement.AppendChild(u_idElement);
            registerElement.AppendChild(u_nameElement);
            registerElement.AppendChild(u_passwordElement);
            registerElement.AppendChild(u_accessElement);
            registerElement.AppendChild(u_moneyElement);
            document.AppendChild(registerElement);
            //报文初始化
            TransMessage message = new TransMessage();
            message.fromAddress = fromAddr;
            message.toAddress = toAddr;
            message.serviceType = EnumServiceType.CUV;
            message.specificType = EnumCUV.Register;
            message.contents = XMLPhaser.XmlToString(document);
            message.EnPackage(mySKeyFile, sessionKey);
            transceiver.SendMessage(message);
            message = transceiver.ReceiveMessage();
            message.DePackage(vPKeyFile, sessionKey);
            document = XMLPhaser.StringToXml(message.contents);
            if (message.errorCode == EnumErrorCode.Error)
                throw new Exception("注册函数错误！");
            XmlElement xmlRoot = document.DocumentElement;
            return "true".Equals(xmlRoot["state"].InnerText);
        }

        public string Refind(string Uid, string Uname)
        {
            //创建XMLDocument
            XmlDocument document = new XmlDocument();
            //根节点
            XmlElement refindElement = document.CreateElement("refind");
            //子节点
            XmlElement u_idElement = document.CreateElement("u_id");
            u_idElement.InnerText = Uid;
            XmlElement u_nameElement = document.CreateElement("u_name");
            u_nameElement.InnerText = Uname;
            //形成树结构
            refindElement.AppendChild(u_idElement);
            refindElement.AppendChild(u_nameElement);
            document.AppendChild(refindElement);
            //报文初始化
            TransMessage message = new TransMessage();
            message.fromAddress = fromAddr;
            message.toAddress = toAddr;
            message.serviceType = EnumServiceType.CUV;
            message.specificType = EnumCUV.Refind;
            message.contents = XMLPhaser.XmlToString(document);
            message.EnPackage(mySKeyFile, sessionKey);
            transceiver.SendMessage(message);
            message = transceiver.ReceiveMessage();
            message.DePackage(vPKeyFile, sessionKey);
            document = XMLPhaser.StringToXml(message.contents);
            if (message.errorCode == EnumErrorCode.Error)
                throw new Exception("找回密码函数错误！");
            XmlElement xmlRoot = document.DocumentElement;
            if ("true".Equals(xmlRoot["state"].InnerText))
                return xmlRoot["u_password"].InnerText;
            else
                return null;
        }

        public bool ModifyName(string Uid, string Uname)
        {
            //创建XMLDocument
            XmlDocument document = new XmlDocument();
            //根节点
            XmlElement modifyElement = document.CreateElement("modify_name");
            //子节点
            XmlElement u_idElement = document.CreateElement("u_id");
            u_idElement.InnerText = Uid;
            XmlElement u_nameElement = document.CreateElement("u_name");
            u_nameElement.InnerText = Uname;
            //形成树结构
            modifyElement.AppendChild(u_idElement);
            modifyElement.AppendChild(u_nameElement);
            document.AppendChild(modifyElement);
            //报文初始化
            TransMessage message = new TransMessage();
            message.fromAddress = fromAddr;
            message.toAddress = toAddr;
            message.serviceType = EnumServiceType.CUV;
            message.specificType = EnumCUV.ModifyName;
            message.contents = XMLPhaser.XmlToString(document);
            message.EnPackage(mySKeyFile, sessionKey);
            transceiver.SendMessage(message);
            message = transceiver.ReceiveMessage();
            message.DePackage(vPKeyFile, sessionKey);
            document = XMLPhaser.StringToXml(message.contents);
            if (message.errorCode == EnumErrorCode.Error)
                throw new Exception("修改名称函数错误！");
            XmlElement xmlRoot = document.DocumentElement;
            return "true".Equals(xmlRoot["state"].InnerText);
        }

        public bool ModifyPassword(string Uid, string Upassword)
        {
            //创建XMLDocument
            XmlDocument document = new XmlDocument();
            //根节点
            XmlElement modifyElement = document.CreateElement("modify_password");
            //子节点
            XmlElement u_idElement = document.CreateElement("u_id");
            u_idElement.InnerText = Uid;
            XmlElement u_passwordElement = document.CreateElement("u_password");
            u_passwordElement.InnerText = Upassword;
            //形成树结构
            modifyElement.AppendChild(u_idElement);
            modifyElement.AppendChild(u_passwordElement);
            document.AppendChild(modifyElement);
            //报文初始化
            TransMessage message = new TransMessage();
            message.fromAddress = fromAddr;
            message.toAddress = toAddr;
            message.serviceType = EnumServiceType.CUV;
            message.specificType = EnumCUV.ModifyPassword;
            message.contents = XMLPhaser.XmlToString(document);
            message.EnPackage(mySKeyFile, sessionKey);
            transceiver.SendMessage(message);
            message = transceiver.ReceiveMessage();
            message.DePackage(vPKeyFile, sessionKey);
            document = XMLPhaser.StringToXml(message.contents);
            if (message.errorCode == EnumErrorCode.Error)
                throw new Exception("修改密码函数错误！");
            XmlElement xmlRoot = document.DocumentElement;
            return "true".Equals(xmlRoot["state"].InnerText);
        }


    }
}
