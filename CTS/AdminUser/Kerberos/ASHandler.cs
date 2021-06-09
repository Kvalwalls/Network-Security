using System;
using System.Configuration;
using System.Net.Sockets;
using System.Xml;
using AdminUser.Entity;
using AdminUser.Transmission;

namespace AdminUser.Kerberos
{
    class ASHandler
    {
        //数据收发器
        private readonly Transceiver transceiver;
        //ASHandler实例
        private static ASHandler instance = new ASHandler();

        /// <summary>
        /// 私有构造函数
        /// </summary>
        private ASHandler()
        {
            Socket socket = Connection.ConnectServer(
               ConfigurationManager.AppSettings["AS_IPAddress"],
               int.Parse(ConfigurationManager.AppSettings["AS_Port"]));
            transceiver = new Transceiver(socket);
            if (transceiver == null)
                throw new Exception("Transceiver初始化错误！");
        }

        /// <summary>
        /// 获取ASHandler实例函数
        /// </summary>
        /// <returns></returns>
        public static ASHandler GetInstance()
        {
            return instance;
        }

        /// <summary>
        /// AS认证函数
        /// </summary>
        /// <returns>Key(c,tgs)+Ticket_tgs</returns>
        public string[] ASCertification()
        {
            string[] keyAndTicket = null;
            //发送请求
            SendRequest();
            //接收回复
            string[] contents = ReceiveReply();
            if (contents == null)
                throw new Exception("AS认证错误！");
            else
            {
                long ts2 = long.Parse(contents[2]);
                long lifetime = long.Parse(contents[3]);
                //回复报文的验证
                if (Tools.VerifyTS(ts2, lifetime) && contents[1].Equals(ConfigurationManager.AppSettings["TGS_ID"]))
                    keyAndTicket = new string[2] { contents[0], contents[4] };
                else
                    throw new Exception("AS认证错误！");
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

        /// <summary>
        /// 接收回复函数
        /// </summary>
        /// <returns>回复报文内容</returns>
        private string[] ReceiveReply()
        {
            string[] contents = null;
            //接收报文
            TransMessage message = transceiver.ReceiveMessage();
            message.DePackage(ConfigurationManager.AppSettings["AS_PKeyFile"], ConfigurationManager.AppSettings["My_Key"]);
            if (message.errorCode == EnumErrorCode.NoError)
            {
                //分析报文内容
                contents = new string[5];
                XmlDocument document = XMLPhaser.StringToXml(message.contents);
                XmlElement xmlRoot = document.DocumentElement;
                XmlNodeList xmlContents = xmlRoot.ChildNodes;
                foreach (XmlNode node in xmlContents)
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