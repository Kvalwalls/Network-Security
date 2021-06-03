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
        private const int LIFE_TIME = 60;
        private readonly Transceiver transceiver;
        private static TGSHandler instance = new TGSHandler();
        private TGSHandler()
        {
            Socket socket = Connection.ConnectServer(
            ConfigurationManager.AppSettings["TGS_IPAddress"],
            int.Parse(ConfigurationManager.AppSettings["TGS_Port"]));
            transceiver = new Transceiver(socket);
            if (transceiver == null)
                throw new Exception("Transceiver错误！");
        }
        public static TGSHandler GetInstatnce()
        {
            return instance;
        }
        public string[] TGSCertification(string sessionKey, string ticket_tgs)
        {
            string[] keyAndTicket = null;
            SendRequest(sessionKey,ticket_tgs);
            string[] contents = ReceiveReply(sessionKey);
            if (contents == null)
                keyAndTicket = TGSCertification(sessionKey, ticket_tgs);
            else
            {
                long ts4 = long.Parse(contents[2]);
                if (Tools.VerifyTS(ts4, LIFE_TIME) && contents[1].Equals(ConfigurationManager.AppSettings["V_ID"]))
                {
                    keyAndTicket = new string[2]
                    {
                        contents[0],
                        contents[3]
                    };
                }
                else
                    keyAndTicket = TGSCertification(sessionKey, ticket_tgs);
            }
            return keyAndTicket;
        }
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
            authenticator.timestamp = Tools.GenerateTS();
            XmlElement authenticator_cElement = document.CreateElement("authenticator_c");
            authenticator_cElement.InnerText = authenticator.generateAuthenticator(sessionKey);
            //形成树结构
            certificationElement.AppendChild(id_vElement);
            certificationElement.AppendChild(ticket_tgsElement);
            certificationElement.AppendChild(authenticator_cElement);
            document.AppendChild(certificationElement);
            
            Console.WriteLine("TGSSend");
            Console.WriteLine(XMLPhaser.XmlToString(document));

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
        private string[] ReceiveReply(string sessionKey)
        {
            string[] contents = null;
            TransMessage message = transceiver.ReceiveMessage();
            message.DePackage(ConfigurationManager.AppSettings["TGS_PKeyFile"], sessionKey);

            Console.WriteLine("TGSReceive");
            Console.WriteLine(message.contents);

            if (message.errorCode == EnumErrorCode.NoError)
            {
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
