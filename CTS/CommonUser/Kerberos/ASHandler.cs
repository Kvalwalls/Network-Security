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
        public string[] ASCertification()
        {
            string[] keyAndTicket = null;
            SendRequest();
            string[] contents = ReceiveReply();
            if (contents == null)
                keyAndTicket = ASCertification();
            else
            {
                long ts2 = long.Parse(contents[2]);
                long lifetime = long.Parse(contents[3]);
                if (Tools.VerifyTS(ts2, lifetime) && contents[1].Equals(ConfigurationManager.AppSettings["TGS_ID"]))
                {
                    keyAndTicket = new string[2] 
                    {
                        contents[0],
                        contents[4]
                    };
                }
                else
                    keyAndTicket = ASCertification();
            }
            return keyAndTicket;
        }

        public void CloseASConnection()
        {
            //创建XMLDocument
            XmlDocument document = new XmlDocument();
            //根节点
            XmlElement endElement = document.CreateElement("as_end");
            document.AppendChild(endElement);
            //报文初始化
            TransMessage message = new TransMessage();
            message.fromAddress = AddressPhaser.StringToBytes(ConfigurationManager.AppSettings["My_IPAddress"]);
            message.toAddress = AddressPhaser.StringToBytes(ConfigurationManager.AppSettings["AS_IPAddress"]);
            message.serviceType = EnumServiceType.AS;
            message.specificType = EnumKerberos.End;
            message.contents = XMLPhaser.XmlToString(document);
            message.EnPackage(ConfigurationManager.AppSettings["My_SKeyFile"], null);
            transceiver.SendMessage(message);
            transceiver.CloseTransceiver();
        }

        /// <summary>
        /// 发送请求
        /// </summary>
        private void SendRequest()
        {
            //创建XMLDocument
            XmlDocument document = new XmlDocument();
            //根节点
            XmlElement certificationElement = document.CreateElement("as_certification");
            //子节点
            XmlElement id_cElement = document.CreateElement("id_c");
            id_cElement.InnerText = ConfigurationManager.AppSettings["My_ID"];
            XmlElement id_tgsElement = document.CreateElement("id_tgs");
            id_tgsElement.InnerText = ConfigurationManager.AppSettings["TGS_ID"];
            XmlElement ts1Element = document.CreateElement("ts1");
            ts1Element.InnerText = Tools.GenerateTS().ToString();
            //形成树结构
            certificationElement.AppendChild(id_cElement);
            certificationElement.AppendChild(id_tgsElement);
            certificationElement.AppendChild(ts1Element);
            document.AppendChild(certificationElement);
            
            Console.WriteLine("ASSend");
            Console.WriteLine(XMLPhaser.XmlToString(document));

            //报文初始化
            TransMessage message = new TransMessage();
            message.fromAddress = AddressPhaser.StringToBytes(ConfigurationManager.AppSettings["My_IPAddress"]);
            message.toAddress = AddressPhaser.StringToBytes(ConfigurationManager.AppSettings["AS_IPAddress"]);
            message.serviceType = EnumServiceType.AS;
            message.specificType = EnumKerberos.Request;
            message.contents = XMLPhaser.XmlToString(document);
            message.EnPackage(ConfigurationManager.AppSettings["My_SKeyFile"], null);
            transceiver.SendMessage(message);
        }

        private string[] ReceiveReply()
        {
            string[] contents = null;
            TransMessage message = transceiver.ReceiveMessage();
            message.DePackage(ConfigurationManager.AppSettings["AS_PKeyFile"], ConfigurationManager.AppSettings["My_Key"]);
            
            Console.WriteLine("ASReceive");
            Console.WriteLine(message.contents);

            if (message.errorCode == EnumErrorCode.NoError)
            {
                contents = new string[5];
                XmlDocument document = XMLPhaser.StringToXml(message.contents);
                XmlElement xmlRoot = document.DocumentElement;
                XmlNodeList xmlContents = xmlRoot.ChildNodes;
                foreach(XmlNode node in xmlContents)
                {
                    if ("key".Equals(node.Name))
                        contents[0] = node.InnerText.Trim();
                    else if ("id_tgs".Equals(node.Name))
                        contents[1] = node.InnerText.Trim();
                    else if ("ts2".Equals(node.Name))
                        contents[2] = node.InnerText.Trim();
                    else if ("lifetime".Equals(node.Name))
                        contents[3] = node.InnerText.Trim();
                    else if ("ticket_tgs".Equals(node.Name))
                        contents[4] = node.InnerText.Trim();
                }
            }
            return contents;
        }
        
    }
}