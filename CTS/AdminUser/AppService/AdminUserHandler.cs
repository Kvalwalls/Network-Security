using AdminUser.Entity;
using AdminUser.Kerberos;
using AdminUser.Transmission;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace AdminUser.AppService
{
    class AdminUserHandler
    {
        private readonly Transceiver transceiver;
        private static AdminUserHandler instance = new AdminUserHandler();
        private AdminUserHandler()
        {
            Socket socket = Connection.ConnectServer(
               ConfigurationManager.AppSettings["AS_IPAddress"],
               int.Parse(ConfigurationManager.AppSettings["AS_Port"]));
            transceiver = new Transceiver(socket);
            if (transceiver == null)
                throw new Exception("Transceiver错误！");
        }
        public static AdminUserHandler GetInstatnce()
        {
            return instance;
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
            message.fromAddress = AddressPhaser.StringToBytes(ConfigurationManager.AppSettings["My_IPAddress"]);
            message.toAddress = AddressPhaser.StringToBytes(ConfigurationManager.AppSettings["V_IPAddress"]);
            message.serviceType = EnumServiceType.AUV;
            message.specificType = EnumKerberos.Request;
            message.contents = XMLPhaser.XmlToString(document);
            message.EnPackage(ConfigurationManager.AppSettings["My_SKeyFile"], null);
            transceiver.SendMessage(message);
        }

        public string[] loginReply()
        {
            string[] contents = null;
            TransMessage message = transceiver.ReceiveMessage();
            message.DePackage(ConfigurationManager.AppSettings["AS_PKeyFile"], ConfigurationManager.AppSettings["My_Key"]);
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
            message.fromAddress = AddressPhaser.StringToBytes(ConfigurationManager.AppSettings["My_IPAddress"]);
            message.toAddress = AddressPhaser.StringToBytes(ConfigurationManager.AppSettings["V_IPAddress"]);
            message.serviceType = EnumServiceType.AUV;
            message.specificType = EnumKerberos.Request;
            message.contents = XMLPhaser.XmlToString(document);
            message.EnPackage(ConfigurationManager.AppSettings["My_SKeyFile"], null);
            transceiver.SendMessage(message);
        }

        public string logoutReply()
        {
            string content = null;
            TransMessage message = transceiver.ReceiveMessage();
            message.DePackage(ConfigurationManager.AppSettings["AS_PKeyFile"], ConfigurationManager.AppSettings["My_Key"]);
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
            message.fromAddress = AddressPhaser.StringToBytes(ConfigurationManager.AppSettings["My_IPAddress"]);
            message.toAddress = AddressPhaser.StringToBytes(ConfigurationManager.AppSettings["V_IPAddress"]);
            message.serviceType = EnumServiceType.AUV;
            message.specificType = EnumKerberos.Request;
            message.contents = XMLPhaser.XmlToString(document);
            message.EnPackage(ConfigurationManager.AppSettings["My_SKeyFile"], null);
            transceiver.SendMessage(message);
        }

        public List<User> getUserReply()
        {
            List<User> n = new List<User>();
            TransMessage message = transceiver.ReceiveMessage();
            message.DePackage(ConfigurationManager.AppSettings["AS_PKeyFile"], ConfigurationManager.AppSettings["My_Key"]);
            if (message.errorCode == EnumErrorCode.NoError)
            {
                XmlDocument document = XMLPhaser.StringToXml(message.contents);
                XmlElement xmlRoot = document.DocumentElement;
                XmlNodeList xmlContents = xmlRoot.ChildNodes;
                foreach (XmlNode xnode in xmlContents)
                {
                    if ("User".Equals(xnode.Name))
                    {
                        foreach (XmlNode node in xnode.ChildNodes)
                        {
                            User tem = new User();
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
                            n.Add(tem);
                        }
                    }
                }
                //n.Add(tem);
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
            message.fromAddress = AddressPhaser.StringToBytes(ConfigurationManager.AppSettings["My_IPAddress"]);
            message.toAddress = AddressPhaser.StringToBytes(ConfigurationManager.AppSettings["V_IPAddress"]);
            message.serviceType = EnumServiceType.AUV;
            message.specificType = EnumKerberos.Request;
            message.contents = XMLPhaser.XmlToString(document);
            message.EnPackage(ConfigurationManager.AppSettings["My_SKeyFile"], null);
            transceiver.SendMessage(message);
        }

        public string addUserReply()
        {
            string contents = null;
            TransMessage message = transceiver.ReceiveMessage();
            message.DePackage(ConfigurationManager.AppSettings["AS_PKeyFile"], ConfigurationManager.AppSettings["My_Key"]);
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
            message.fromAddress = AddressPhaser.StringToBytes(ConfigurationManager.AppSettings["My_IPAddress"]);
            message.toAddress = AddressPhaser.StringToBytes(ConfigurationManager.AppSettings["V_IPAddress"]);
            message.serviceType = EnumServiceType.AUV;
            message.specificType = EnumKerberos.Request;
            message.contents = XMLPhaser.XmlToString(document);
            message.EnPackage(ConfigurationManager.AppSettings["My_SKeyFile"], null);
            transceiver.SendMessage(message);
        }

        public string delUserReply()
        {
            string contents = null;
            TransMessage message = transceiver.ReceiveMessage();
            message.DePackage(ConfigurationManager.AppSettings["AS_PKeyFile"], ConfigurationManager.AppSettings["My_Key"]);
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
            message.fromAddress = AddressPhaser.StringToBytes(ConfigurationManager.AppSettings["My_IPAddress"]);
            message.toAddress = AddressPhaser.StringToBytes(ConfigurationManager.AppSettings["V_IPAddress"]);
            message.serviceType = EnumServiceType.AUV;
            message.specificType = EnumKerberos.Request;
            message.contents = XMLPhaser.XmlToString(document);
            message.EnPackage(ConfigurationManager.AppSettings["My_SKeyFile"], null);
            transceiver.SendMessage(message);
        }

        public List<Theater> getTheaterReply()
        {
            List<Theater> n = new List<Theater>();
            TransMessage message = transceiver.ReceiveMessage();
            message.DePackage(ConfigurationManager.AppSettings["AS_PKeyFile"], ConfigurationManager.AppSettings["My_Key"]);
            if (message.errorCode == EnumErrorCode.NoError)
            {
                XmlDocument document = XMLPhaser.StringToXml(message.contents);
                XmlElement xmlRoot = document.DocumentElement;
                XmlNodeList xmlContents = xmlRoot.ChildNodes;
                foreach (XmlNode xnode in xmlContents)
                {

                    if ("Theater".Equals(xnode.Name))
                    {
                        foreach (XmlNode node in xnode.ChildNodes)
                        {
                            Theater tem = new Theater();
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
                            n.Add(tem);
                        }
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
            message.fromAddress = AddressPhaser.StringToBytes(ConfigurationManager.AppSettings["My_IPAddress"]);
            message.toAddress = AddressPhaser.StringToBytes(ConfigurationManager.AppSettings["V_IPAddress"]);
            message.serviceType = EnumServiceType.AUV;
            message.specificType = EnumKerberos.Request;
            message.contents = XMLPhaser.XmlToString(document);
            message.EnPackage(ConfigurationManager.AppSettings["My_SKeyFile"], null);
            transceiver.SendMessage(message);
        }

        public string addTheaterReply()
        {
            string contents = null;
            TransMessage message = transceiver.ReceiveMessage();
            message.DePackage(ConfigurationManager.AppSettings["AS_PKeyFile"], ConfigurationManager.AppSettings["My_Key"]);
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
            message.fromAddress = AddressPhaser.StringToBytes(ConfigurationManager.AppSettings["My_IPAddress"]);
            message.toAddress = AddressPhaser.StringToBytes(ConfigurationManager.AppSettings["V_IPAddress"]);
            message.serviceType = EnumServiceType.AUV;
            message.specificType = EnumKerberos.Request;
            message.contents = XMLPhaser.XmlToString(document);
            message.EnPackage(ConfigurationManager.AppSettings["My_SKeyFile"], null);
            transceiver.SendMessage(message);
        }

        public string delTheaterReply()
        {
            string contents = null;
            TransMessage message = transceiver.ReceiveMessage();
            message.DePackage(ConfigurationManager.AppSettings["AS_PKeyFile"], ConfigurationManager.AppSettings["My_Key"]);
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
            message.fromAddress = AddressPhaser.StringToBytes(ConfigurationManager.AppSettings["My_IPAddress"]);
            message.toAddress = AddressPhaser.StringToBytes(ConfigurationManager.AppSettings["V_IPAddress"]);
            message.serviceType = EnumServiceType.AUV;
            message.specificType = EnumKerberos.Request;
            message.contents = XMLPhaser.XmlToString(document);
            message.EnPackage(ConfigurationManager.AppSettings["My_SKeyFile"], null);
            transceiver.SendMessage(message);
        }

        public List<Movie> getMovieReply()
        {
            List<Movie> n = new List<Movie>();
            TransMessage message = transceiver.ReceiveMessage();
            message.DePackage(ConfigurationManager.AppSettings["AS_PKeyFile"], ConfigurationManager.AppSettings["My_Key"]);
            if (message.errorCode == EnumErrorCode.NoError)
            {
                XmlDocument document = XMLPhaser.StringToXml(message.contents);
                XmlElement xmlRoot = document.DocumentElement;
                XmlNodeList xmlContents = xmlRoot.ChildNodes;
                foreach (XmlNode xnode in xmlContents)
                { 
                    if ("Movie".Equals(xnode.Name))
                    {
                        foreach (XmlNode node in xnode.ChildNodes)
                        {
                            Movie tem = new Movie();
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
                            n.Add(tem);
                        }
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
            Id.InnerText = m.Mname;
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
            addMovieEle.AppendChild(Type);
            addMovieEle.AppendChild(Time);
            addMovieEle.AppendChild(Comment);
            addMovieEle.AppendChild(Description);
            document.AppendChild(addMovieEle);
            Console.WriteLine(document.InnerXml);
            //报文初始化
            TransMessage message = new TransMessage();
            message.fromAddress = AddressPhaser.StringToBytes(ConfigurationManager.AppSettings["My_IPAddress"]);
            message.toAddress = AddressPhaser.StringToBytes(ConfigurationManager.AppSettings["V_IPAddress"]);
            message.serviceType = EnumServiceType.AUV;
            message.specificType = EnumKerberos.Request;
            message.contents = XMLPhaser.XmlToString(document);
            message.EnPackage(ConfigurationManager.AppSettings["My_SKeyFile"], null);
            transceiver.SendMessage(message);
        }

        public string addMovieReply()
        {
            string contents = null;
            TransMessage message = transceiver.ReceiveMessage();
            message.DePackage(ConfigurationManager.AppSettings["AS_PKeyFile"], ConfigurationManager.AppSettings["My_Key"]);
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
            message.fromAddress = AddressPhaser.StringToBytes(ConfigurationManager.AppSettings["My_IPAddress"]);
            message.toAddress = AddressPhaser.StringToBytes(ConfigurationManager.AppSettings["V_IPAddress"]);
            message.serviceType = EnumServiceType.AUV;
            message.specificType = EnumKerberos.Request;
            message.contents = XMLPhaser.XmlToString(document);
            message.EnPackage(ConfigurationManager.AppSettings["My_SKeyFile"], null);
            transceiver.SendMessage(message);
        }

        public string delMovieReply()
        {
            string contents = null;
            TransMessage message = transceiver.ReceiveMessage();
            message.DePackage(ConfigurationManager.AppSettings["AS_PKeyFile"], ConfigurationManager.AppSettings["My_Key"]);
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
            message.fromAddress = AddressPhaser.StringToBytes(ConfigurationManager.AppSettings["My_IPAddress"]);
            message.toAddress = AddressPhaser.StringToBytes(ConfigurationManager.AppSettings["V_IPAddress"]);
            message.serviceType = EnumServiceType.AUV;
            message.specificType = EnumKerberos.Request;
            message.contents = XMLPhaser.XmlToString(document);
            message.EnPackage(ConfigurationManager.AppSettings["My_SKeyFile"], null);
            transceiver.SendMessage(message);
        }

        public List<OnMovie> getOnMovieReply()
        {
            List<OnMovie> n = new List<OnMovie>();
            TransMessage message = transceiver.ReceiveMessage();
            message.DePackage(ConfigurationManager.AppSettings["AS_PKeyFile"], ConfigurationManager.AppSettings["My_Key"]);
            if (message.errorCode == EnumErrorCode.NoError)
            {
                XmlDocument document = XMLPhaser.StringToXml(message.contents);
                XmlElement xmlRoot = document.DocumentElement;
                XmlNodeList xmlContents = xmlRoot.ChildNodes;
                foreach (XmlNode xnode in xmlContents)
                {
                    if ("OnMovie".Equals(xnode.Name))
                    {
                        foreach (XmlNode node in xnode.ChildNodes)
                        {
                            OnMovie tem = new OnMovie();
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
                            n.Add(tem);
                        }
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
            message.fromAddress = AddressPhaser.StringToBytes(ConfigurationManager.AppSettings["My_IPAddress"]);
            message.toAddress = AddressPhaser.StringToBytes(ConfigurationManager.AppSettings["V_IPAddress"]);
            message.serviceType = EnumServiceType.AUV;
            message.specificType = EnumKerberos.Request;
            message.contents = XMLPhaser.XmlToString(document);
            message.EnPackage(ConfigurationManager.AppSettings["My_SKeyFile"], null);
            transceiver.SendMessage(message);
        }

        public string addOnMovieReply()
        {
            string contents = null;
            TransMessage message = transceiver.ReceiveMessage();
            message.DePackage(ConfigurationManager.AppSettings["AS_PKeyFile"], ConfigurationManager.AppSettings["My_Key"]);
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
            XmlElement Mid = document.CreateElement("Mid");
            Mid.InnerText = m.Mid;
            XmlElement Tid = document.CreateElement("Tid");
            Tid.InnerText = m.Tid;

            //形成树结构
            delOnMovieEle.AppendChild(Oid);
            delOnMovieEle.AppendChild(Mid);
            delOnMovieEle.AppendChild(Tid);
            document.AppendChild(delOnMovieEle);
            Console.WriteLine(document.InnerXml);
            //报文初始化
            TransMessage message = new TransMessage();
            message.fromAddress = AddressPhaser.StringToBytes(ConfigurationManager.AppSettings["My_IPAddress"]);
            message.toAddress = AddressPhaser.StringToBytes(ConfigurationManager.AppSettings["V_IPAddress"]);
            message.serviceType = EnumServiceType.AUV;
            message.specificType = EnumKerberos.Request;
            message.contents = XMLPhaser.XmlToString(document);
            message.EnPackage(ConfigurationManager.AppSettings["My_SKeyFile"], null);
            transceiver.SendMessage(message);
        }

        public string delOnMovieReply()
        {
            string contents = null;
            TransMessage message = transceiver.ReceiveMessage();
            message.DePackage(ConfigurationManager.AppSettings["AS_PKeyFile"], ConfigurationManager.AppSettings["My_Key"]);
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
            message.fromAddress = AddressPhaser.StringToBytes(ConfigurationManager.AppSettings["My_IPAddress"]);
            message.toAddress = AddressPhaser.StringToBytes(ConfigurationManager.AppSettings["V_IPAddress"]);
            message.serviceType = EnumServiceType.AUV;
            message.specificType = EnumKerberos.Request;
            message.contents = XMLPhaser.XmlToString(document);
            message.EnPackage(ConfigurationManager.AppSettings["My_SKeyFile"], null);
            transceiver.SendMessage(message);
        }

        public List<Record> getRecordReply()
        {
            List<Record> n = new List<Record>();
            TransMessage message = transceiver.ReceiveMessage();
            message.DePackage(ConfigurationManager.AppSettings["AS_PKeyFile"], ConfigurationManager.AppSettings["My_Key"]);
            if (message.errorCode == EnumErrorCode.NoError)
            {
                XmlDocument document = XMLPhaser.StringToXml(message.contents);
                XmlElement xmlRoot = document.DocumentElement;
                XmlNodeList xmlContents = xmlRoot.ChildNodes;
                foreach (XmlNode xnode in xmlContents)
                {
                    if ("OnMovie".Equals(xnode.Name))
                    {
                        foreach (XmlNode node in xnode.ChildNodes)
                        {
                            Record tem = new Record();
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
                            n.Add(tem);
                        }
                    }
                }
                //n.Add(tem);
            }
            return n;
        }

        public void getPictureRequest(int num,string Mid)
        {
            //创建XMLDocument
            XmlDocument document = new XmlDocument();
            //根节点
            XmlElement getPicEle = document.CreateElement("GetPicture");
            //子节点
            XmlElement Num = document.CreateElement("Picturenum");
            Num.InnerText = Convert.ToString(num);
            XmlElement MID = document.CreateElement("Picturemid");
            MID.InnerText = Mid;
            //XmlElement Password = document.CreateElement("Password");
            //Password.InnerText = u.Upassword;

            //形成树结构
            getPicEle.AppendChild(Num);
            getPicEle.AppendChild(MID);
            document.AppendChild(getPicEle);
            Console.WriteLine(document.InnerXml);
            //报文初始化
            TransMessage message = new TransMessage();
            message.fromAddress = AddressPhaser.StringToBytes(ConfigurationManager.AppSettings["My_IPAddress"]);
            message.toAddress = AddressPhaser.StringToBytes(ConfigurationManager.AppSettings["V_IPAddress"]);
            message.serviceType = EnumServiceType.AUV;
            message.specificType = EnumKerberos.Request;
            message.contents = XMLPhaser.XmlToString(document);
            message.EnPackage(ConfigurationManager.AppSettings["My_SKeyFile"], null);
            transceiver.SendMessage(message);
        }

        public string getPictureReply()
        {
            string content = null;
            TransMessage message = transceiver.ReceiveMessage();
            message.DePackage(ConfigurationManager.AppSettings["AS_PKeyFile"], ConfigurationManager.AppSettings["My_Key"]);
            if (message.errorCode == EnumErrorCode.NoError)
            {
                XmlDocument document = XMLPhaser.StringToXml(message.contents);
                XmlElement xmlRoot = document.DocumentElement;
                XmlNodeList xmlContents = xmlRoot.ChildNodes;
                foreach (XmlNode node in xmlContents)
                {
                    if ("Pic".Equals(node.Name))
                        content = node.InnerText.Trim();
                    
                }
            }
            return content;
        }

        public void sendPictureRequest(string Mid, string Pic)
        {
            //创建XMLDocument
            XmlDocument document = new XmlDocument();
            //根节点
            XmlElement sendPicEle = document.CreateElement("SendPicture");
            //子节点
            XmlElement MID = document.CreateElement("Picturemid");
            MID.InnerText = Mid;
            XmlElement PIC = document.CreateElement("Picture");
            PIC.InnerText = Pic;
            //XmlElement Password = document.CreateElement("Password");
            //Password.InnerText = u.Upassword;

            //形成树结构
            sendPicEle.AppendChild(MID);
            sendPicEle.AppendChild(PIC);
            document.AppendChild(sendPicEle);
            Console.WriteLine(document.InnerXml);
            //报文初始化
            TransMessage message = new TransMessage();
            message.fromAddress = AddressPhaser.StringToBytes(ConfigurationManager.AppSettings["My_IPAddress"]);
            message.toAddress = AddressPhaser.StringToBytes(ConfigurationManager.AppSettings["V_IPAddress"]);
            message.serviceType = EnumServiceType.AUV;
            message.specificType = EnumKerberos.Request;
            message.contents = XMLPhaser.XmlToString(document);
            message.EnPackage(ConfigurationManager.AppSettings["My_SKeyFile"], null);
            transceiver.SendMessage(message);
        }

        public string sendPictureReply()
        {
            string content = null;
            TransMessage message = transceiver.ReceiveMessage();
            message.DePackage(ConfigurationManager.AppSettings["AS_PKeyFile"], ConfigurationManager.AppSettings["My_Key"]);
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
    }
}
