using CommonUser.Entity;
using CommonUser.Transmission;
using System;
using System.Configuration;
using System.Net.Sockets;
using System.Xml;

namespace CommonUser.Kerberos
{
    class VHandler
    {
        private const int LIFE_TIME = 60;
        private readonly Transceiver transceiver;
        private static VHandler instance = new VHandler();
        private VHandler()
        {
            Socket socket = Connection.ConnectServer(
               ConfigurationManager.AppSettings["V_IPAddress"],
               int.Parse(ConfigurationManager.AppSettings["V_Port"]));
            transceiver = new Transceiver(socket);
            if (transceiver == null)
                throw new Exception("Transceiver错误！");
        }
        public static VHandler GetInstance()
        {
            return instance;
        }
        public string VCertification(string sessionKey, string ticket_v)
        {
            long TS5 = SendRequest(sessionKey, ticket_v);
            if (ReceiveReply(TS5, sessionKey))
                return sessionKey;
            else
                throw new Exception("V验证错误！");
        }
        private long SendRequest(string sessionKey, string ticket_v)
        {
            //创建XMLDocument
            XmlDocument document = new XmlDocument();
            //根节点
            XmlElement certificationElement = document.CreateElement("v_certification");
            //子节点
            XmlElement ticket_vElement = document.CreateElement("ticket_v");
            ticket_vElement.InnerText = ticket_v;
            Authenticator authenticator = new Authenticator();
            authenticator.ID_c = ConfigurationManager.AppSettings["My_ID"];
            authenticator.AD_c = ConfigurationManager.AppSettings["My_IPAddress"];
            long TS5 = authenticator.timestamp = ToolsKerberos.GenerateTS();
            XmlElement authenticator_cElement = document.CreateElement("authenticator_c");
            authenticator_cElement.InnerText = authenticator.generateAuthenticator(sessionKey);
            //形成树结构
            certificationElement.AppendChild(ticket_vElement);
            certificationElement.AppendChild(authenticator_cElement);
            document.AppendChild(certificationElement);

            Console.WriteLine("VSend");
            Console.WriteLine(XMLPhaser.XmlToString(document));

            //报文初始化
            TransMessage message = new TransMessage();
            message.fromAddress = AddressPhaser.StringToBytes(ConfigurationManager.AppSettings["My_IPAddress"]);
            message.toAddress = AddressPhaser.StringToBytes(ConfigurationManager.AppSettings["V_IPAddress"]);
            message.serviceType = EnumServiceType.CUV;
            message.specificType = EnumKerberos.Request;
            message.contents = XMLPhaser.XmlToString(document);
            message.EnPackage(ConfigurationManager.AppSettings["My_SKeyFile"], null);
            transceiver.SendMessage(message);
            return TS5;
        }
        private bool ReceiveReply(long TS5, string sessionKey)
        {
            TransMessage message = transceiver.ReceiveMessage();
            message.DePackage(ConfigurationManager.AppSettings["V_PKeyFile"], sessionKey);
            if (message.errorCode == EnumErrorCode.NoError)
            {
                XmlDocument document = XMLPhaser.StringToXml(message.contents);
                XmlElement xmlRoot = document.DocumentElement;
                XmlNodeList xmlContents = xmlRoot.ChildNodes;
                long TS5Plus = 0;
                foreach (XmlNode node in xmlContents)
                {
                    if ("ts5plus".Equals(node.Name))
                        TS5Plus = long.Parse(node.InnerText.Trim());
                }
                if (TS5Plus == TS5 + 1)
                    return true;
            }
            return false;
        }
    }
}
