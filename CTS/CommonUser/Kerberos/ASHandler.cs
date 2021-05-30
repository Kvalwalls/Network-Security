using CommonUser.Entity;
using CommonUser.Transmission;
using System;
using System.Configuration;
using System.Net.Sockets;
using System.Xml;

namespace CommonUser.Kerberos
{
    class ASHandler
    {
        private readonly Transceiver transceiver;
        private static ASHandler instance = new ASHandler();
        private ASHandler()
        {
            Socket socket = Connection.ConnectServer(
               ConfigurationManager.AppSettings["AS_IPAddress"],
               int.Parse(ConfigurationManager.AppSettings["AS_Port"]));
            transceiver = new Transceiver(socket);
            if (transceiver == null)
                throw new Exception("Transceiver错误！");
        }
        public static ASHandler GetInstatnce()
        {
            return instance;
        }
        public static bool ASCertification()
        {
            return false;
        }
        private void SendRequest()
        {
            //XML对象的建立
            XmlDocument document = new XmlDocument();
            XmlElement certification = document.CreateElement("as_certification");
            //创建XML元素
            XmlElement ID_c = document.CreateElement("id_c");
            ID_c.InnerText = ConfigurationManager.AppSettings["My_ID"];
            XmlElement ID_tgs = document.CreateElement("id_tgs");
            ID_tgs.InnerText = ConfigurationManager.AppSettings["TGS_ID"];
            XmlElement TS1 = document.CreateElement("ts1");
            TS1.InnerText = GenerateTS().ToString();
            //形成树形结构
            certification.AppendChild(ID_c);
            certification.AppendChild(ID_tgs);
            certification.AppendChild(TS1);
            document.AppendChild(certification);
            //报文初始化
            TransMessage message = new TransMessage();
            message.fromAddress = AddressPhaser.StringToBytes(ConfigurationManager.AppSettings["My_IPAddress"]);
            message.toAddress = AddressPhaser.StringToBytes(ConfigurationManager.AppSettings["AS_IPAddress"]);
            message.serviceType = (byte)EnumServiceType.AS;
            message.specificType = (byte)EnumKerberos.Request;
            message.contents = XMLPhaser.XmlToString(document);
            message.EnPackage("..\\..\\KeyFiles\\Client.sk", null);
            transceiver.SendMessage(message);
        }
        private void ReceiveReply()
        {
            TransMessage message = transceiver.ReceiveMessage();
            message.DePackage("..\\..\\KeyFiles\\AS.pk", ConfigurationManager.AppSettings["My_Key"]);
            if(message.errorCode == (byte)EnumErrorCode.NoError)
            {

            }
        }
        private static long GenerateTS()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds);
        }

    }
}
