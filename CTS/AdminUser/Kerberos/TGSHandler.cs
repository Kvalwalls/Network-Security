using AdminUser.Entity;
using AdminUser.Transmission;
using System;
using System.Configuration;
using System.Net.Sockets;
using System.Xml;

namespace AdminUser.Kerberos
{
    class TGSHandler
    {
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
            SendRequest(sessionKey, ticket_tgs);
            string[] keyAndTicket = null;
            return keyAndTicket;
        }
        public void CloseTGSConnection()
        {
            //创建XMLDocument
            XmlDocument document = new XmlDocument();
            //根节点
            XmlElement end = document.CreateElement("tgs_end");
            document.AppendChild(end);
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
        private void SendRequest(string ticket_tgs, string key)
        {
            //创建XMLDocument
            XmlDocument document = new XmlDocument();
            //根节点
            XmlElement certificationEle = document.CreateElement("tgs_certification");
            //子节点
            XmlElement id_vEle = document.CreateElement("id_v");
            id_vEle.InnerText = ConfigurationManager.AppSettings["V_ID"];
            XmlElement ticket_tgsEle = document.CreateElement("ticket_tgs");
            ticket_tgsEle.InnerText = ticket_tgs;
            Authenticator authenticator = new Authenticator();
            authenticator.ID_c = ConfigurationManager.AppSettings["My_ID"];
            authenticator.AD_c = ConfigurationManager.AppSettings["My_IPAddress"];
            authenticator.timestamp = Tools.GenerateTS();
            XmlElement authenticator_cEle = document.CreateElement("authenticator_c");
            authenticator_cEle.InnerText = authenticator.generateAuthenticator(key);
            //形成树结构
            certificationEle.AppendChild(id_vEle);
            certificationEle.AppendChild(ticket_tgsEle);
            certificationEle.AppendChild(authenticator_cEle);
            document.AppendChild(certificationEle);
            Console.WriteLine(document.InnerXml);
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
    }
}
