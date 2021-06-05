using CommonUser.Entity;
using CommonUser.Transmission;
using System;
using System.Configuration;
using System.Net.Sockets;
using System.Xml;

namespace CommonUser.Kerberos
{
    class TGSHandler
    {
        //数据收发器
        private readonly Transceiver transceiver;
        //TGSHandler实例
        private static TGSHandler instance = new TGSHandler();

        /// <summary>
        /// 私有构造函数
        /// </summary>
        private TGSHandler()
        {
            Socket socket = Connection.ConnectServer(
            ConfigurationManager.AppSettings["TGS_IPAddress"],
            int.Parse(ConfigurationManager.AppSettings["TGS_Port"]));
            transceiver = new Transceiver(socket);
            if (transceiver == null)
                throw new Exception("Transceiver初始化错误！");
        }

        /// <summary>
        /// 获取TGSHandler实例函数
        /// </summary>
        /// <returns>TGSHandler实例</returns>
        public static TGSHandler GetInstance()
        {
            return instance;
        }

        /// <summary>
        /// TGS认证函数
        /// </summary>
        /// <param name="sessionKey">Key(c,tgs)</param>
        /// <param name="ticket_tgs">Ticket_tgs</param>
        /// <returns>Key(c,v)+Ticket_v</returns>
        public string[] TGSCertification(string sessionKey, string ticket_tgs)
        {
            string[] keyAndTicket = null;
            //发送请求
            SendRequest(sessionKey,ticket_tgs);
            //接收回复
            string[] contents = ReceiveReply(sessionKey);
            if (contents == null)
                throw new Exception("TGS认证错误！");
            else
            {
                long ts4 = long.Parse(contents[2]);
                //回复报文的验证
                if (ToolsKerberos.VerifyTS(ts4, ToolsKerberos.LIFE_TIME) && contents[1].Equals(ConfigurationManager.AppSettings["V_ID"]))
                    keyAndTicket = new string[2] { contents[0], contents[3] };
                else
                    throw new Exception("TGS认证错误！");
            }
            return keyAndTicket;
        }

        /// <summary>
        /// 关闭TGS连接函数
        /// </summary>
        public void CloseTGSConnection()
        {
            //创建XMLDocument
            XmlDocument document = new XmlDocument();
            //根节点
            XmlElement endElement = document.CreateElement("tgs_end");
            document.AppendChild(endElement);
            //报文初始化
            TransMessage message = new TransMessage();
            message.fromAddress = AddressPhaser.StringToBytes(ConfigurationManager.AppSettings["My_IPAddress"]);
            message.toAddress = AddressPhaser.StringToBytes(ConfigurationManager.AppSettings["TGS_IPAddress"]);
            message.serviceType = EnumServiceType.TGS;
            message.specificType = EnumKerberos.End;
            message.contents = XMLPhaser.XmlToString(document);
            message.EnPackage(ConfigurationManager.AppSettings["My_SKeyFile"], null);
            transceiver.SendMessage(message);
            transceiver.CloseTransceiver();
        }

        /// <summary>
        /// 发送请求函数
        /// </summary>
        /// <param name="sessionKey">Key(c,tgs)</param>
        /// <param name="ticket_tgs">Tikcket_tgs</param>
        private void SendRequest(string sessionKey, string ticket_tgs)
        {
            //创建XMLDocument
            XmlDocument document = new XmlDocument();
            //根节点
            XmlElement certificationElement = document.CreateElement("tgs_certification");
            //子节点
            XmlElement id_vElement = document.CreateElement("id_v");
            id_vElement.InnerText = ConfigurationManager.AppSettings["V_ID"];
            XmlElement ticket_tgsElement = document.CreateElement("ticket_tgs");
            ticket_tgsElement.InnerText = ticket_tgs;
            Authenticator authenticator = new Authenticator();
            authenticator.ID_c = ConfigurationManager.AppSettings["My_ID"];
            authenticator.AD_c = ConfigurationManager.AppSettings["My_IPAddress"];
            authenticator.timestamp = ToolsKerberos.GenerateTS();
            XmlElement authenticator_cElement = document.CreateElement("authenticator_c");
            authenticator_cElement.InnerText = authenticator.generateAuthenticator(sessionKey);
            //形成树结构
            certificationElement.AppendChild(id_vElement);
            certificationElement.AppendChild(ticket_tgsElement);
            certificationElement.AppendChild(authenticator_cElement);
            document.AppendChild(certificationElement);
            //报文初始化
            TransMessage message = new TransMessage();
            message.fromAddress = AddressPhaser.StringToBytes(ConfigurationManager.AppSettings["My_IPAddress"]);
            message.toAddress = AddressPhaser.StringToBytes(ConfigurationManager.AppSettings["TGS_IPAddress"]);
            message.serviceType = EnumServiceType.TGS;
            message.specificType = EnumKerberos.Request;
            message.contents = XMLPhaser.XmlToString(document);
            message.EnPackage(ConfigurationManager.AppSettings["My_SKeyFile"], null);
            transceiver.SendMessage(message);
        }

        /// <summary>
        /// 接收回复函数
        /// </summary>
        /// <param name="sessionKey">Key(c,tgs)</param>
        /// <returns>回复报文内容</returns>
        private string[] ReceiveReply(string sessionKey)
        {
            string[] contents = null;
            //接收报文
            TransMessage message = transceiver.ReceiveMessage();
            message.DePackage(ConfigurationManager.AppSettings["TGS_PKeyFile"], sessionKey);
            if (message.errorCode == EnumErrorCode.NoError)
            {
                //分析报文内容
                contents = new string[4];
                XmlDocument document = XMLPhaser.StringToXml(message.contents);
                XmlElement xmlRoot = document.DocumentElement;
                XmlNodeList xmlContents = xmlRoot.ChildNodes;
                foreach (XmlNode node in xmlContents)
                {
                    if ("key".Equals(node.Name))
                        contents[0] = node.InnerText.Trim();
                    else if ("id_v".Equals(node.Name))
                        contents[1] = node.InnerText.Trim();
                    else if ("ts4".Equals(node.Name))
                        contents[2] = node.InnerText.Trim();
                    else if ("ticket_v".Equals(node.Name))
                        contents[3] = node.InnerText.Trim();
                }
            }
            return contents;
        }
    }
}
