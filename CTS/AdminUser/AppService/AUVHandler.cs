using AdminUser.Entity;
using AdminUser.Kerberos;
using AdminUser.Transmission;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace AdminUser.AppService
{
    class AUVHandler:VHandler
    {

        //会话DES密钥
        private readonly string sessionKey = "11111111";
        //本机IP地址
        private byte[] fromAddr = AddressPhaser.StringToBytes(ConfigurationManager.AppSettings["My_IPAddress"]);
        //AUV服务器IP地址
        private byte[] toAddr = AddressPhaser.StringToBytes(ConfigurationManager.AppSettings["V_IPAddress"]);
        //本机私钥文件
        private string mySKeyFile = ConfigurationManager.AppSettings["My_SKeyFile"];
        //服务器公钥文件
        private string vPKeyFile = ConfigurationManager.AppSettings["V_PKeyFile"];
        //AUVHandler实例
        private static AUVHandler instance = new AUVHandler();

        public static Package package;

        private AUVHandler()
        {
            /*
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
            */
            Socket socket = Connection.ConnectServer(
              IPStr: ConfigurationManager.AppSettings["V_IPAddress"],
              int.Parse(ConfigurationManager.AppSettings["V_Port"]));
            transceiver = new Transceiver(socket);
            if (transceiver == null)
                throw new Exception("Transceiver错误！");

        }
    
        public static AUVHandler GetInstance()
        {
            return instance;
        }

        public void SetPackage(Package p)
        {
            package = p;
        }

        public void loginRequest(User u)
        {
            //创建XMLDocument
            XmlDocument document = new XmlDocument();
            //根节点
            XmlElement loginEle = document.CreateElement("LogIn");
            //子节点
            XmlElement Id = document.CreateElement("Id");
            Id.InnerText = u.Uid;
            XmlElement Password = document.CreateElement("Password");
            Password.InnerText = u.Upassword;
            
            //形成树结构
            loginEle.AppendChild(Id);
            loginEle.AppendChild(Password);
            document.AppendChild(loginEle);
            Console.WriteLine(document.InnerXml);
            //报文初始化
            TransMessage message = new TransMessage();
            message.package = package;
            message.fromAddress = fromAddr;
            message.toAddress = toAddr;
            message.serviceType = EnumServiceType.AUV;
            message.specificType = EnumAUV.Login;
            message.contents = XMLPhaser.XmlToString(document);
            message.EnPackage(mySKeyFile, sessionKey);
            transceiver.SendMessage(message);
        }

        public string[] loginReply()
        {
            string[] contents = null;
            TransMessage message = transceiver.ReceiveMessage();
            message.package = package;
            message.DePackage(vPKeyFile, sessionKey);
            if (message.errorCode == EnumErrorCode.NoError)
            {
                contents = new string[4];
                XmlDocument document = XMLPhaser.StringToXml(message.contents);
                XmlElement xmlRoot = document.DocumentElement;
                XmlNodeList xmlContents = xmlRoot.ChildNodes;
                foreach (XmlNode node in xmlContents)
                {
                    if ("Status".Equals(node.Name))
                        contents[0] = node.InnerText.Trim();
                    else if ("Name".Equals(node.Name))
                        contents[1] = node.InnerText.Trim();
                    else if ("Money".Equals(node.Name))
                        contents[2] = node.InnerText.Trim();
                    else if ("Access".Equals(node.Name))
                        contents[3] = node.InnerText.Trim();
                }
            }
            return contents;
        }

        public void logOutRequest(User u)
        {
            //创建XMLDocument
            XmlDocument document = new XmlDocument();
            //根节点
            XmlElement logoutEle = document.CreateElement("LogOut");
            //子节点
            XmlElement Id = document.CreateElement("Id");
            Id.InnerText = u.Uid;
            //XmlElement Password = document.CreateElement("Password");
            //Password.InnerText = u.Upassword;

            //形成树结构
            logoutEle.AppendChild(Id);
            //loginEle.AppendChild(Password);
            document.AppendChild(logoutEle);
            Console.WriteLine(document.InnerXml);

            //报文初始化
            TransMessage message = new TransMessage();
            message.package = package;
            message.fromAddress = fromAddr;
            message.toAddress = toAddr;
            message.serviceType = EnumServiceType.AUV;
            message.specificType = EnumAUV.Login;
            message.contents = XMLPhaser.XmlToString(document);
            message.EnPackage(mySKeyFile, sessionKey);
            transceiver.SendMessage(message);
        }

        public string logoutReply()
        {
            string content = null;
            TransMessage message = transceiver.ReceiveMessage();
            message.package = package;
            message.DePackage(vPKeyFile, sessionKey);
            if (message.errorCode == EnumErrorCode.NoError)
            {
                XmlDocument document = XMLPhaser.StringToXml(message.contents);
                XmlElement xmlRoot = document.DocumentElement;
                XmlNodeList xmlContents = xmlRoot.ChildNodes;
                foreach (XmlNode node in xmlContents)
                {
                    if ("Status".Equals(node.Name))
                        content = node.InnerText.Trim();
                }
            }
            return content;
        }

        public void getUserRequest()
        {
            //创建XMLDocument
            XmlDocument document = new XmlDocument();
            //根节点
            XmlElement getUserEle = document.CreateElement("GetAllUser");
            //子节点
            //XmlElement Get = document.CreateElement("Get");
            //Get.InnerText = "User";
            //XmlElement Password = document.CreateElement("Password");
            //Password.InnerText = u.Upassword;

            //形成树结构
            //getUserEle.AppendChild(Get);
            //getUserEle.AppendChild(Password);
            document.AppendChild(getUserEle);
            Console.WriteLine(document.InnerXml);
            //报文初始化
            TransMessage message = new TransMessage();
            message.package = package;
            message.fromAddress = fromAddr;
            message.toAddress = toAddr;
            message.serviceType = EnumServiceType.AUV;
            message.specificType = EnumAUV.GetUser;
            message.contents = XMLPhaser.XmlToString(document);
            message.EnPackage(mySKeyFile, sessionKey);
            transceiver.SendMessage(message);
        }

        public List<User> getUserReply()
        {
            List<User> n = new List<User>();
            TransMessage message = transceiver.ReceiveMessage();
            message.package = package;
            message.DePackage(vPKeyFile, sessionKey);
            if (message.errorCode == EnumErrorCode.NoError)
            {
                XmlDocument document = XMLPhaser.StringToXml(message.contents);
                XmlElement xmlRoot = document.DocumentElement;
                XmlNodeList xmlContents = xmlRoot.ChildNodes;
                foreach (XmlNode xnode in xmlContents)
                {
                    if ("User".Equals(xnode.Name))
                    {
                        User tem = new User();
                        foreach (XmlNode node in xnode.ChildNodes)
                        {
                            if ("Id".Equals(node.Name))
                                tem.Uid = node.InnerText.Trim();
                            else if ("Name".Equals(node.Name))
                                tem.Uname = node.InnerText.Trim();
                            else if ("Password".Equals(node.Name))
                                tem.Upassword = node.InnerText.Trim();
                            else if ("Access".Equals(node.Name))
                            {
                                if (node.InnerText.Trim() == "0")
                                    tem.Uaccess = 0;
                                else if (node.InnerText.Trim() == "1")
                                    tem.Uaccess = 1;
                                else if (node.InnerText.Trim() == "2")
                                    tem.Uaccess = 2;
                                else if (node.InnerText.Trim() == "3")
                                    tem.Uaccess = 3;
                                else if (node.InnerText.Trim() == "4")
                                    tem.Uaccess = 4;
                            }
                            else if ("Money".Equals(node.Name))
                                tem.Umoney = Convert.ToSingle(node.InnerText.Trim());
                        }
                        n.Add(tem);
                    }
                }
            }
            return n;
        }

        public void addUserRequest(User u)
        {
            //创建XMLDocument
            XmlDocument document = new XmlDocument();
            //根节点
            XmlElement addUserEle = document.CreateElement("AddUser");
            //子节点
            XmlElement Id = document.CreateElement("Id");
            Id.InnerText = u.Uid;
            XmlElement Name = document.CreateElement("Name");
            Name.InnerText = u.Uname;
            XmlElement Password = document.CreateElement("Password");
            Password.InnerText = u.Upassword;
            XmlElement Access = document.CreateElement("Access");
            string access = null;
            if (u.Uaccess == 0)
            {
                access = "0";
            }
            else if(u.Uaccess == 1)
            {
                access = "1";
            }
            else if (u.Uaccess == 2)
            {
                access = "2";
            }
            else if (u.Uaccess == 3)
            {
                access = "3";
            }
            else if (u.Uaccess == 4)
            {
                access = "4";
            }
            else if (u.Uaccess == 5)
            {
                access = "5";
            }
            Access.InnerText = access;
            XmlElement Money = document.CreateElement("Money");
            Money.InnerText = u.Umoney.ToString();

            //形成树结构
            addUserEle.AppendChild(Id);
            addUserEle.AppendChild(Name);
            addUserEle.AppendChild(Password);
            addUserEle.AppendChild(Access);
            addUserEle.AppendChild(Money);
            document.AppendChild(addUserEle);
            Console.WriteLine(document.InnerXml);
            //报文初始化
            TransMessage message = new TransMessage();
            message.package = package;
            message.fromAddress = fromAddr;
            message.toAddress = toAddr;
            message.serviceType = EnumServiceType.AUV;
            message.specificType = EnumAUV.AddUser;
            message.contents = XMLPhaser.XmlToString(document);
            message.EnPackage(mySKeyFile, sessionKey);
            transceiver.SendMessage(message);
        }

        public string addUserReply()
        {
            string contents = null;
            TransMessage message = transceiver.ReceiveMessage();
            message.package = package;
            message.DePackage(vPKeyFile, sessionKey);
            if (message.errorCode == EnumErrorCode.NoError)
            {
                XmlDocument document = XMLPhaser.StringToXml(message.contents);
                XmlElement xmlRoot = document.DocumentElement;
                XmlNodeList xmlContents = xmlRoot.ChildNodes;
                foreach (XmlNode node in xmlContents)
                {
                    if ("Status".Equals(node.Name))
                        contents = node.InnerText.Trim();
                }
            }
            return contents;
        }

        public void delUserRequest(User u)
        {
            //创建XMLDocument
            XmlDocument document = new XmlDocument();
            //根节点
            XmlElement delUserEle = document.CreateElement("DelUser");
            //子节点
            XmlElement Id = document.CreateElement("Id");
            Id.InnerText = u.Uid;
            
            //形成树结构
            delUserEle.AppendChild(Id);
            document.AppendChild(delUserEle);
            Console.WriteLine(document.InnerXml);
            //报文初始化
            TransMessage message = new TransMessage();
            message.package = package;
            message.fromAddress = fromAddr;
            message.toAddress = toAddr;
            message.serviceType = EnumServiceType.AUV;
            message.specificType = EnumAUV.DelUser;
            message.contents = XMLPhaser.XmlToString(document);
            message.EnPackage(mySKeyFile, sessionKey);
            transceiver.SendMessage(message);
        }

        public string delUserReply()
        {
            string contents = null;
            TransMessage message = transceiver.ReceiveMessage();
            message.package = package;
            message.DePackage(vPKeyFile, sessionKey);
            if (message.errorCode == EnumErrorCode.NoError)
            {
                XmlDocument document = XMLPhaser.StringToXml(message.contents);
                XmlElement xmlRoot = document.DocumentElement;
                XmlNodeList xmlContents = xmlRoot.ChildNodes;
                foreach (XmlNode node in xmlContents)
                {
                    if ("Status".Equals(node.Name))
                        contents = node.InnerText.Trim();
                }
            }
            return contents;
        }

        public void getTheaterRequest()
        {
            //创建XMLDocument
            XmlDocument document = new XmlDocument();
            //根节点
            XmlElement getTheaterEle = document.CreateElement("GetAllTheater");
            //子节点
            XmlElement Get = document.CreateElement("Get");
            Get.InnerText = "Theater";
            //XmlElement Password = document.CreateElement("Password");
            //Password.InnerText = u.Upassword;

            //形成树结构
            getTheaterEle.AppendChild(Get);
            //getUserEle.AppendChild(Password);
            document.AppendChild(getTheaterEle);
            Console.WriteLine(document.InnerXml);
            //报文初始化
            TransMessage message = new TransMessage();
            message.package = package;
            message.fromAddress = fromAddr;
            message.toAddress = toAddr;
            message.serviceType = EnumServiceType.AUV;
            message.specificType = EnumAUV.GetTheater;
            message.contents = XMLPhaser.XmlToString(document);
            message.EnPackage(mySKeyFile, sessionKey);
            transceiver.SendMessage(message);
        }

        public List<Theater> getTheaterReply()
        {
            List<Theater> n = new List<Theater>();
            TransMessage message = transceiver.ReceiveMessage();
            message.package = package;
            message.DePackage(vPKeyFile, sessionKey);
            if (message.errorCode == EnumErrorCode.NoError)
            {
                XmlDocument document = XMLPhaser.StringToXml(message.contents);
                XmlElement xmlRoot = document.DocumentElement;
                XmlNodeList xmlContents = xmlRoot.ChildNodes;
                foreach (XmlNode xnode in xmlContents)
                {

                    if ("Theater".Equals(xnode.Name))
                    {
                        Theater tem = new Theater();
                        foreach (XmlNode node in xnode.ChildNodes)
                        {
                            
                            if ("Id".Equals(node.Name))
                                tem.Tid = node.InnerText.Trim();
                            else if ("Type".Equals(node.Name))
                            {
                                if (node.InnerText.Trim() == "0")
                                    tem.Ttype = 0;
                                else if (node.InnerText.Trim() == "1")
                                    tem.Ttype = 1;
                                else if (node.InnerText.Trim() == "2")
                                    tem.Ttype = 2;

                            }
                            else if ("Size".Equals(node.Name))
                                tem.Tsize = int.Parse(node.InnerText.Trim());
                        }
                        n.Add(tem);
                    }
                }
                //n.Add(tem);
            }
            return n;
        }

        public void addTheaterRequest(Theater t)
        {
            //创建XMLDocument
            XmlDocument document = new XmlDocument();
            //根节点
            XmlElement addTheaterEle = document.CreateElement("AddTheater");
            //子节点
            XmlElement Id = document.CreateElement("Id");
            Id.InnerText = t.Tid;
            XmlElement Type = document.CreateElement("Type");
            string type = null;
            if (t.Ttype == 0)
            {
                type = "0";
            }
            else if (t.Ttype == 1)
            {
                type = "1";
            }
            else if (t.Ttype == 2)
            {
                type = "2";
            }
            Type.InnerText = type;
            XmlElement Size = document.CreateElement("Size");
            Size.InnerText = t.Tsize.ToString();
            
            //形成树结构
            addTheaterEle.AppendChild(Id);
            addTheaterEle.AppendChild(Type);
            addTheaterEle.AppendChild(Size);
            document.AppendChild(addTheaterEle);
            Console.WriteLine(document.InnerXml);
            //报文初始化
            TransMessage message = new TransMessage();
            message.package = package;
            message.fromAddress = fromAddr;
            message.toAddress = toAddr;
            message.serviceType = EnumServiceType.AUV;
            message.specificType = EnumAUV.AddTheater;
            message.contents = XMLPhaser.XmlToString(document);
            message.EnPackage(mySKeyFile, sessionKey);
            transceiver.SendMessage(message);
        }

        public string addTheaterReply()
        {
            string contents = null;
            TransMessage message = transceiver.ReceiveMessage();
            message.package = package;
            message.DePackage(vPKeyFile, sessionKey);
            if (message.errorCode == EnumErrorCode.NoError)
            {
                XmlDocument document = XMLPhaser.StringToXml(message.contents);
                XmlElement xmlRoot = document.DocumentElement;
                XmlNodeList xmlContents = xmlRoot.ChildNodes;
                foreach (XmlNode node in xmlContents)
                {
                    if ("Status".Equals(node.Name))
                        contents = node.InnerText.Trim();
                }
            }
            return contents;
        }

        public void delTheaterRequest(Theater t)
        {
            //创建XMLDocument
            XmlDocument document = new XmlDocument();
            //根节点
            XmlElement delTheaterEle = document.CreateElement("DelTheater");
            //子节点
            XmlElement Id = document.CreateElement("Id");
            Id.InnerText = t.Tid;

            //形成树结构
            delTheaterEle.AppendChild(Id);
            document.AppendChild(delTheaterEle);
            Console.WriteLine(document.InnerXml);
            //报文初始化
            TransMessage message = new TransMessage();
            message.package = package;
            message.fromAddress = fromAddr;
            message.toAddress = toAddr;
            message.serviceType = EnumServiceType.AUV;
            message.specificType = EnumAUV.DelTheater;
            message.contents = XMLPhaser.XmlToString(document);
            message.EnPackage(mySKeyFile, sessionKey);
            transceiver.SendMessage(message);
        }

        public string delTheaterReply()
        {
            string contents = null;
            TransMessage message = transceiver.ReceiveMessage();
            message.package = package;
            message.DePackage(vPKeyFile, sessionKey);
            if (message.errorCode == EnumErrorCode.NoError)
            {
                XmlDocument document = XMLPhaser.StringToXml(message.contents);
                XmlElement xmlRoot = document.DocumentElement;
                XmlNodeList xmlContents = xmlRoot.ChildNodes;
                foreach (XmlNode node in xmlContents)
                {
                    if ("Status".Equals(node.Name))
                        contents = node.InnerText.Trim();
                }
            }
            return contents;
        }

        public void getMovieRequest()
        {
            //创建XMLDocument
            XmlDocument document = new XmlDocument();
            //根节点
            XmlElement getMovieEle = document.CreateElement("GetAllMovie");
            //子节点
            XmlElement Get = document.CreateElement("Get");
            Get.InnerText = "Movie";
            //XmlElement Password = document.CreateElement("Password");
            //Password.InnerText = u.Upassword;

            //形成树结构
            getMovieEle.AppendChild(Get);
            //getUserEle.AppendChild(Password);
            document.AppendChild(getMovieEle);
            Console.WriteLine(document.InnerXml);
            //报文初始化
            TransMessage message = new TransMessage();
            message.package = package;
            message.fromAddress = fromAddr;
            message.toAddress = toAddr;
            message.serviceType = EnumServiceType.AUV;
            message.specificType = EnumAUV.GetMovie;
            message.contents = XMLPhaser.XmlToString(document);
            message.EnPackage(mySKeyFile, sessionKey);
            transceiver.SendMessage(message);
        }

        public List<Movie> getMovieReply()
        {
            List<Movie> n = new List<Movie>();
            TransMessage message = transceiver.ReceiveMessage();
            message.package = package;
            message.DePackage(vPKeyFile, sessionKey);
            if (message.errorCode == EnumErrorCode.NoError)
            {
                XmlDocument document = XMLPhaser.StringToXml(message.contents);
                XmlElement xmlRoot = document.DocumentElement;
                XmlNodeList xmlContents = xmlRoot.ChildNodes;
                foreach (XmlNode xnode in xmlContents)
                { 
                    if ("Movie".Equals(xnode.Name))
                    {
                        Movie tem = new Movie();
                        foreach (XmlNode node in xnode.ChildNodes)
                        {
                            
                            if ("Id".Equals(node.Name))
                                tem.Mid = node.InnerText.Trim();
                            else if ("Name".Equals(node.Name))
                                tem.Mname = node.InnerText.Trim();
                            else if ("Type".Equals(node.Name))
                                tem.Mtype = node.InnerText.Trim();
                            else if ("Time".Equals(node.Name))
                                tem.Mtime = int.Parse(node.InnerText.Trim());
                            else if ("Comment".Equals(node.Name))
                                tem.Mcomment = Convert.ToSingle(node.InnerText.Trim());
                            else if ("Description".Equals(node.Name))
                                tem.Mdescription = node.InnerText.Trim();
                            tem.Mpicture = Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).ToString()).ToString()
                    + "\\MoviePictures\\" + tem.Mid + ".jpg";
                        }
                        n.Add(tem);
                    }
                }
                //n.Add(tem);
            }
            return n;
        }

        public void addMovieRequest(Movie m)
        {
            //创建XMLDocument
            XmlDocument document = new XmlDocument();
            //根节点
            XmlElement addMovieEle = document.CreateElement("AddMovie");
            //子节点
            XmlElement Id = document.CreateElement("Id");
            Id.InnerText = m.Mid;
            XmlElement Name = document.CreateElement("Name");
            Name.InnerText = m.Mname;
            XmlElement Type = document.CreateElement("Type");
            Type.InnerText = m.Mtype;
            XmlElement Time = document.CreateElement("Time");
            Time.InnerText = m.Mtime.ToString();
            XmlElement Comment = document.CreateElement("Comment");
            Comment.InnerText = m.Mcomment.ToString();
            XmlElement Description = document.CreateElement("Description");
            Description.InnerText = m.Mdescription;

            //形成树结构
            addMovieEle.AppendChild(Id);
            addMovieEle.AppendChild(Name);
            addMovieEle.AppendChild(Type);
            addMovieEle.AppendChild(Time);
            addMovieEle.AppendChild(Comment);
            addMovieEle.AppendChild(Description);
            document.AppendChild(addMovieEle);
            Console.WriteLine(document.InnerXml);
            //报文初始化
            TransMessage message = new TransMessage();
            message.package = package;
            message.fromAddress = fromAddr;
            message.toAddress = toAddr;
            message.serviceType = EnumServiceType.AUV;
            message.specificType = EnumAUV.AddMovie;
            message.contents = XMLPhaser.XmlToString(document);
            message.EnPackage(mySKeyFile, sessionKey);
            transceiver.SendMessage(message);
        }

        public string addMovieReply()
        {
            string contents = null;
            TransMessage message = transceiver.ReceiveMessage();
            message.package = package;
            message.DePackage(vPKeyFile, sessionKey);
            if (message.errorCode == EnumErrorCode.NoError)
            {
                XmlDocument document = XMLPhaser.StringToXml(message.contents);
                XmlElement xmlRoot = document.DocumentElement;
                XmlNodeList xmlContents = xmlRoot.ChildNodes;
                foreach (XmlNode node in xmlContents)
                {
                    if ("Status".Equals(node.Name))
                        contents = node.InnerText.Trim();
                }
            }
            return contents;
        }

        public void delMovieRequest(Movie m)
        {
            //创建XMLDocument
            XmlDocument document = new XmlDocument();
            //根节点
            XmlElement delMovieEle = document.CreateElement("DelMovie");
            //子节点
            XmlElement Id = document.CreateElement("Id");
            Id.InnerText = m.Mid;

            //形成树结构
            delMovieEle.AppendChild(Id);
            document.AppendChild(delMovieEle);
            Console.WriteLine(document.InnerXml);
            //报文初始化
            TransMessage message = new TransMessage();
            message.package = package;
            message.fromAddress = fromAddr;
            message.toAddress = toAddr;
            message.serviceType = EnumServiceType.AUV;
            message.specificType = EnumAUV.DelMovie;
            message.contents = XMLPhaser.XmlToString(document);
            message.EnPackage(mySKeyFile, sessionKey);
            transceiver.SendMessage(message);
        }

        public string delMovieReply()
        {
            string contents = null;
            TransMessage message = transceiver.ReceiveMessage();
            message.package = package;
            message.DePackage(vPKeyFile, sessionKey);
            if (message.errorCode == EnumErrorCode.NoError)
            {
                XmlDocument document = XMLPhaser.StringToXml(message.contents);
                XmlElement xmlRoot = document.DocumentElement;
                XmlNodeList xmlContents = xmlRoot.ChildNodes;
                foreach (XmlNode node in xmlContents)
                {
                    if ("Status".Equals(node.Name))
                        contents = node.InnerText.Trim();
                }
            }
            return contents;
        }

        public void getOnMovieRequest()
        {
            //创建XMLDocument
            XmlDocument document = new XmlDocument();
            //根节点
            XmlElement getOnMovieEle = document.CreateElement("GetAllOnMovie");
            //子节点
            //XmlElement Get = document.CreateElement("Get");
            //Get.InnerText = "Movie";
            //XmlElement Password = document.CreateElement("Password");
            //Password.InnerText = u.Upassword;

            //形成树结构
            //getOnMovieEle.AppendChild(Get);
            //getUserEle.AppendChild(Password);
            document.AppendChild(getOnMovieEle);
            Console.WriteLine(document.InnerXml);
            //报文初始化
            TransMessage message = new TransMessage();
            message.package = package;
            message.fromAddress = fromAddr;
            message.toAddress = toAddr;
            message.serviceType = EnumServiceType.AUV;
            message.specificType = EnumAUV.GetOnMovie;
            message.contents = XMLPhaser.XmlToString(document);
            message.EnPackage(mySKeyFile, sessionKey);
            transceiver.SendMessage(message);
        }

        public List<OnMovie> getOnMovieReply()
        {
            List<OnMovie> n = new List<OnMovie>();
            TransMessage message = transceiver.ReceiveMessage();
            message.package = package;
            message.DePackage(vPKeyFile, sessionKey);
            if (message.errorCode == EnumErrorCode.NoError)
            {
                XmlDocument document = XMLPhaser.StringToXml(message.contents);
                XmlElement xmlRoot = document.DocumentElement;
                XmlNodeList xmlContents = xmlRoot.ChildNodes;
                foreach (XmlNode xnode in xmlContents)
                {
                    if ("OnMovie".Equals(xnode.Name))
                    {
                        OnMovie tem = new OnMovie();
                        foreach (XmlNode node in xnode.ChildNodes)
                        {
                            
                            if ("Oid".Equals(node.Name))
                                tem.Oid = node.InnerText.Trim();
                            else if ("Mid".Equals(node.Name))
                                tem.Mid = node.InnerText.Trim();
                            else if ("Tid".Equals(node.Name))
                                tem.Tid = node.InnerText.Trim();
                            else if ("Obegin".Equals(node.Name))
                                tem.Obegin = Convert.ToDateTime(node.InnerText.Trim());
                            else if ("Oend".Equals(node.Name))
                                tem.Oend = Convert.ToDateTime(node.InnerText.Trim());
                            else if ("Oprice".Equals(node.Name))
                                tem.Oprice = Convert.ToSingle(node.InnerText.Trim());
                            
                        }
                        n.Add(tem);
                    }
                }
                //n.Add(tem);
            }
            return n;
        }

        public void addOnMovieRequest(OnMovie o)
        {
            //创建XMLDocument
            XmlDocument document = new XmlDocument();
            //根节点
            XmlElement addOnMovieEle = document.CreateElement("AddOnMovie");
            //子节点
            XmlElement Oid = document.CreateElement("Oid");
            Oid.InnerText = o.Oid;
            XmlElement Mid = document.CreateElement("Mid");
            Mid.InnerText = o.Mid;
            XmlElement Tid = document.CreateElement("Tid");
            Tid.InnerText = o.Tid;
            XmlElement Obegin = document.CreateElement("Obegin");
            Obegin.InnerText = o.Obegin.ToString();
            XmlElement Oend = document.CreateElement("Oend");
            Oend.InnerText = o.Oend.ToString();
            XmlElement Oprice = document.CreateElement("Oprice");
            Oprice.InnerText = o.Oprice.ToString();

            //形成树结构
            addOnMovieEle.AppendChild(Oid);
            addOnMovieEle.AppendChild(Mid);
            addOnMovieEle.AppendChild(Tid);
            addOnMovieEle.AppendChild(Obegin);
            addOnMovieEle.AppendChild(Oend);
            addOnMovieEle.AppendChild(Oprice);
            document.AppendChild(addOnMovieEle);
            Console.WriteLine(document.InnerXml);
            //报文初始化
            TransMessage message = new TransMessage();
            message.package = package;
            message.fromAddress = fromAddr;
            message.toAddress = toAddr;
            message.serviceType = EnumServiceType.AUV;
            message.specificType = EnumAUV.AddOnMovie;
            message.contents = XMLPhaser.XmlToString(document);
            message.EnPackage(mySKeyFile, sessionKey);
            transceiver.SendMessage(message);
        }

        public string addOnMovieReply()
        {
            string contents = null;
            TransMessage message = transceiver.ReceiveMessage();
            message.package = package;
            message.DePackage(vPKeyFile, sessionKey);
            if (message.errorCode == EnumErrorCode.NoError)
            {
                XmlDocument document = XMLPhaser.StringToXml(message.contents);
                XmlElement xmlRoot = document.DocumentElement;
                XmlNodeList xmlContents = xmlRoot.ChildNodes;
                foreach (XmlNode node in xmlContents)
                {
                    if ("Status".Equals(node.Name))
                        contents = node.InnerText.Trim();
                }
            }
            return contents;
        }

        public void delOnMovieRequest(OnMovie m)
        {
            //创建XMLDocument
            XmlDocument document = new XmlDocument();
            //根节点
            XmlElement delOnMovieEle = document.CreateElement("DelOnMovie");
            //子节点
            XmlElement Oid = document.CreateElement("Oid");
            Oid.InnerText = m.Oid;

            //形成树结构
            delOnMovieEle.AppendChild(Oid);
            document.AppendChild(delOnMovieEle);
            Console.WriteLine(document.InnerXml);
            //报文初始化
            TransMessage message = new TransMessage();
            message.package = package;
            message.fromAddress = fromAddr;
            message.toAddress = toAddr;
            message.serviceType = EnumServiceType.AUV;
            message.specificType = EnumAUV.DelOnMovie;
            message.contents = XMLPhaser.XmlToString(document);
            message.EnPackage(mySKeyFile, sessionKey);
            transceiver.SendMessage(message);
        }

        public string delOnMovieReply()
        {
            string contents = null;
            TransMessage message = transceiver.ReceiveMessage(); 
            message.package = package;
            message.DePackage(vPKeyFile, sessionKey);
            if (message.errorCode == EnumErrorCode.NoError)
            {
                XmlDocument document = XMLPhaser.StringToXml(message.contents);
                XmlElement xmlRoot = document.DocumentElement;
                XmlNodeList xmlContents = xmlRoot.ChildNodes;
                foreach (XmlNode node in xmlContents)
                {
                    if ("Status".Equals(node.Name))
                        contents = node.InnerText.Trim();
                }
            }
            return contents;
        }

        public void getRecordRequest()
        {
            //创建XMLDocument
            XmlDocument document = new XmlDocument();
            //根节点
            XmlElement getRecordEle = document.CreateElement("GetAllRecord");
            //子节点
            XmlElement Get = document.CreateElement("Get");
            Get.InnerText = "Record";
            //XmlElement Password = document.CreateElement("Password");
            //Password.InnerText = u.Upassword;

            //形成树结构
            getRecordEle.AppendChild(Get);
            //getUserEle.AppendChild(Password);
            document.AppendChild(getRecordEle);
            Console.WriteLine(document.InnerXml);
            //报文初始化
            TransMessage message = new TransMessage();
            message.package = package;
            message.fromAddress = fromAddr;
            message.toAddress = toAddr;
            message.serviceType = EnumServiceType.AUV;
            message.specificType = EnumAUV.GetRecord;
            message.contents = XMLPhaser.XmlToString(document);
            message.EnPackage(mySKeyFile, sessionKey);
            transceiver.SendMessage(message);
        }

        public List<Record> getRecordReply()
        {
            List<Record> n = new List<Record>();
            TransMessage message = transceiver.ReceiveMessage();
            message.package = package;
            message.DePackage(vPKeyFile, sessionKey);
            if (message.errorCode == EnumErrorCode.NoError)
            {
                XmlDocument document = XMLPhaser.StringToXml(message.contents);
                XmlElement xmlRoot = document.DocumentElement;
                XmlNodeList xmlContents = xmlRoot.ChildNodes;
                foreach (XmlNode xnode in xmlContents)
                {
                    if ("Ticket".Equals(xnode.Name))
                    {
                        Record tem = new Record();
                        foreach (XmlNode node in xnode.ChildNodes)
                        {
                            
                            if ("Uid".Equals(node.Name))
                                tem.Uid = node.InnerText.Trim();
                            else if ("Oid".Equals(node.Name))
                                tem.Oid = node.InnerText.Trim();
                            else if ("Sid".Equals(node.Name))
                                tem.Sid = node.InnerText.Trim();
                            else if ("Rtime".Equals(node.Name))
                                tem.Rtime = Convert.ToDateTime(node.InnerText.Trim());
                            else if ("Rprice".Equals(node.Name))
                                tem.Rprice = Convert.ToSingle(node.InnerText.Trim());
                            else if ("Rstatus".Equals(node.Name))
                            {
                                if (node.InnerText.Trim() == "0")
                                    tem.Rstatus = 0;
                                else if (node.InnerText.Trim() == "1")
                                    tem.Rstatus = 1;
                                else if (node.InnerText.Trim() == "2")
                                    tem.Rstatus = 2;
                            }

                            
                        }
                        n.Add(tem);
                    }
                }
                //n.Add(tem);
            }
            return n;
        }

        public void GetMoviePictures()
        {
            //创建XMLDocument
            XmlDocument document = new XmlDocument();
            //根节点
            XmlElement getElement = document.CreateElement("get_movie_pictures");
            //形成树结构
            document.AppendChild(getElement);
            //报文初始化
            TransMessage message = new TransMessage();
            message.package = package;
            message.fromAddress = fromAddr;
            message.toAddress = toAddr;
            message.serviceType = EnumServiceType.AUV;
            message.specificType = EnumAUV.GetMoviePic;
            message.contents = XMLPhaser.XmlToString(document);
            message.EnPackage(mySKeyFile, sessionKey);
            transceiver.SendMessage(message);
            message = transceiver.ReceiveMessage();
            message.DePackage(vPKeyFile, sessionKey);
            document = XMLPhaser.StringToXml(message.contents);
            if (message.errorCode == EnumErrorCode.Error)
                throw new Exception("获取所有影片图片函数错误！");
            XmlElement xmlRoot = document.DocumentElement;
            int count = int.Parse(xmlRoot["count"].InnerText);
            for (int i = 0; i < count; i++)
            {
                Thread.Sleep(50);
                message = transceiver.ReceiveMessage();
                message.DePackage(vPKeyFile, sessionKey);
                document = XMLPhaser.StringToXml(message.contents);
                if (message.errorCode == EnumErrorCode.Error)
                    throw new Exception("获取所有影片图片函数错误！");
                xmlRoot = document.DocumentElement;
                XmlNodeList nodeList = xmlRoot.ChildNodes;
                string picName = string.Empty;
                string moviePicture = string.Empty;
                foreach (XmlNode node in nodeList)
                {
                    switch (node.Name)
                    {
                        case "m_id":
                            picName = "..\\..\\MoviePictures\\" + node.InnerText + ".jpg";
                            break;
                        case "m_picture":
                            moviePicture = node.InnerText;
                            break;
                    }
                }
                PicturePhaser.Base64ToPicture(moviePicture, picName);
            }
        }

        public void SendPicture(string picture,string Mid)
        {
            //创建XMLDocument
            XmlDocument document = new XmlDocument();
            //根节点
            XmlElement sendElement = document.CreateElement("send_movie_picture");
            //子节点
            XmlElement mid = document.CreateElement("Mid");
            mid.InnerText = Mid;
            XmlElement pic = document.CreateElement("Picture");
            pic.InnerText = picture;
            //形成树结构
            sendElement.AppendChild(mid);
            sendElement.AppendChild(pic);
            document.AppendChild(sendElement);
            //报文初始化
            TransMessage message = new TransMessage();
            message.package = package;
            message.fromAddress = fromAddr;
            message.toAddress = toAddr;
            message.serviceType = EnumServiceType.AUV;
            message.specificType = EnumAUV.SRMoviePic;
            message.contents = XMLPhaser.XmlToString(document);
            message.EnPackage(mySKeyFile, sessionKey);
            transceiver.SendMessage(message);
        }
    }
}
