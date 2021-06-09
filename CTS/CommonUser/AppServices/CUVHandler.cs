using CommonUser.Entity;
using CommonUser.Kerberos;
using CommonUser.Transmission;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net.Sockets;
using System.Threading;
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
        public bool Login(string Uid, string Upassword)
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
            XmlElement xmlRoot = document.DocumentElement;
            return "true".Equals(xmlRoot["state"].InnerText);
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

        public User GetUser(string Uid)
        {
            //创建XMLDocument
            XmlDocument document = new XmlDocument();
            //根节点
            XmlElement getElement = document.CreateElement("get_user");
            //子节点
            XmlElement u_idElement = document.CreateElement("u_id");
            u_idElement.InnerText = Uid;
            //形成树结构
            getElement.AppendChild(u_idElement);
            document.AppendChild(getElement);
            //报文初始化
            TransMessage message = new TransMessage();
            message.fromAddress = fromAddr;
            message.toAddress = toAddr;
            message.serviceType = EnumServiceType.CUV;
            message.specificType = EnumCUV.GetUser;
            message.contents = XMLPhaser.XmlToString(document);
            message.EnPackage(mySKeyFile, sessionKey);
            transceiver.SendMessage(message);
            message = transceiver.ReceiveMessage();
            message.DePackage(vPKeyFile, sessionKey);
            document = XMLPhaser.StringToXml(message.contents);
            if (message.errorCode == EnumErrorCode.Error)
                throw new Exception("获取个人信息函数错误！");
            User user = null;
            XmlElement xmlRoot = document.DocumentElement;
            if ("true".Equals(xmlRoot["state"].InnerText))
            {
                user = new User();
                XmlNodeList userNode = xmlRoot.ChildNodes;
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

        public bool UpgradeAccess(string Uid, byte Uaccess)
        {
            //创建XMLDocument
            XmlDocument document = new XmlDocument();
            //根节点
            XmlElement upgradeElement = document.CreateElement("upgrade_access");
            //子节点
            XmlElement u_idElement = document.CreateElement("u_id");
            u_idElement.InnerText = Uid;
            XmlElement u_accessElement = document.CreateElement("u_access");
            u_accessElement.InnerText = Uaccess.ToString();
            //形成树结构
            upgradeElement.AppendChild(u_idElement);
            upgradeElement.AppendChild(u_accessElement);
            document.AppendChild(upgradeElement);
            //报文初始化
            TransMessage message = new TransMessage();
            message.fromAddress = fromAddr;
            message.toAddress = toAddr;
            message.serviceType = EnumServiceType.CUV;
            message.specificType = EnumCUV.UpgradeAccess;
            message.contents = XMLPhaser.XmlToString(document);
            message.EnPackage(mySKeyFile, sessionKey);
            transceiver.SendMessage(message);
            message = transceiver.ReceiveMessage();
            message.DePackage(vPKeyFile, sessionKey);
            document = XMLPhaser.StringToXml(message.contents);
            if (message.errorCode == EnumErrorCode.Error)
                throw new Exception("升级权限函数错误！");
            XmlElement xmlRoot = document.DocumentElement;
            return "true".Equals(xmlRoot["state"].InnerText);
        }

        public bool Recharge(string Uid, float Umoney)
        {
            //创建XMLDocument
            XmlDocument document = new XmlDocument();
            //根节点
            XmlElement rechargeElement = document.CreateElement("recharge");
            //子节点
            XmlElement u_idElement = document.CreateElement("u_id");
            u_idElement.InnerText = Uid;
            XmlElement u_moneyElement = document.CreateElement("u_money");
            u_moneyElement.InnerText = Umoney.ToString();
            //形成树结构
            rechargeElement.AppendChild(u_idElement);
            rechargeElement.AppendChild(u_moneyElement);
            document.AppendChild(rechargeElement);
            //报文初始化
            TransMessage message = new TransMessage();
            message.fromAddress = fromAddr;
            message.toAddress = toAddr;
            message.serviceType = EnumServiceType.CUV;
            message.specificType = EnumCUV.Recharge;
            message.contents = XMLPhaser.XmlToString(document);
            message.EnPackage(mySKeyFile, sessionKey);
            transceiver.SendMessage(message);
            message = transceiver.ReceiveMessage();
            message.DePackage(vPKeyFile, sessionKey);
            document = XMLPhaser.StringToXml(message.contents);
            if (message.errorCode == EnumErrorCode.Error)
                throw new Exception("金额充值函数错误！");
            XmlElement xmlRoot = document.DocumentElement;
            return "true".Equals(xmlRoot["state"].InnerText);
        }

        public List<Movie> GetMovies()
        {
            //创建XMLDocument
            XmlDocument document = new XmlDocument();
            //根节点
            XmlElement getElement = document.CreateElement("get_movies");
            //形成树结构
            document.AppendChild(getElement);
            //报文初始化
            TransMessage message = new TransMessage();
            message.fromAddress = fromAddr;
            message.toAddress = toAddr;
            message.serviceType = EnumServiceType.CUV;
            message.specificType = EnumCUV.GetMovies;
            message.contents = XMLPhaser.XmlToString(document);
            message.EnPackage(mySKeyFile, sessionKey);
            transceiver.SendMessage(message);
            message = transceiver.ReceiveMessage();
            message.DePackage(vPKeyFile, sessionKey);
            document = XMLPhaser.StringToXml(message.contents);
            if (message.errorCode == EnumErrorCode.Error)
                throw new Exception("获取所有影片信息函数错误！");
            List<Movie> movies = new List<Movie>();
            XmlElement xmlRoot = document.DocumentElement;
            XmlNodeList movieNodeList = xmlRoot.ChildNodes;
            foreach (XmlNode movieNode in movieNodeList)
            {
                Movie temp = new Movie();
                XmlNodeList nodeList = movieNode.ChildNodes;
                foreach (XmlNode node in nodeList)
                {
                    switch (node.Name)
                    {
                        case "m_id":
                            temp.Mid = node.InnerText;
                            break;
                        case "m_name":
                            temp.Mname = node.InnerText;
                            break;
                        case "m_type":
                            temp.Mtype = node.InnerText;
                            break;
                        case "m_time":
                            temp.Mtime = int.Parse(node.InnerText);
                            break;
                        case "m_comment":
                            temp.Mcomment = float.Parse(node.InnerText);
                            break;
                        case "m_description":
                            temp.Mdescription = node.InnerText;
                            break;
                    }
                }
                temp.Mpicture = Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).ToString()).ToString()
                    + "\\MoviePictures\\" + temp.Mid + ".jpg";
                movies.Add(temp);
            }
            return movies;
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
            message.fromAddress = fromAddr;
            message.toAddress = toAddr;
            message.serviceType = EnumServiceType.CUV;
            message.specificType = EnumCUV.GetMoviePictures;
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
                Thread.Sleep(100);
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

        public List<Record> GetRecords(string Uid)
        {
            //创建XMLDocument
            XmlDocument document = new XmlDocument();
            //根节点
            XmlElement getElement = document.CreateElement("get_records");
            XmlElement u_idElement = document.CreateElement("u_id");
            u_idElement.InnerText = Uid;
            //形成树结构
            getElement.AppendChild(u_idElement);
            document.AppendChild(getElement);
            //报文初始化
            TransMessage message = new TransMessage();
            message.fromAddress = fromAddr;
            message.toAddress = toAddr;
            message.serviceType = EnumServiceType.CUV;
            message.specificType = EnumCUV.GetRecords;
            message.contents = XMLPhaser.XmlToString(document);
            message.EnPackage(mySKeyFile, sessionKey);
            transceiver.SendMessage(message);
            message = transceiver.ReceiveMessage();
            message.DePackage(vPKeyFile, sessionKey);
            document = XMLPhaser.StringToXml(message.contents);
            if (message.errorCode == EnumErrorCode.Error)
                throw new Exception("获取所有购票记录函数错误！");
            List<Record> records = new List<Record>();
            XmlElement xmlRoot = document.DocumentElement;
            XmlNodeList recordNodeList = xmlRoot.ChildNodes;
            foreach (XmlNode recordNode in recordNodeList)
            {
                Record temp = new Record();
                XmlNodeList nodeList = recordNode.ChildNodes;
                foreach (XmlNode node in nodeList)
                {
                    switch (node.Name)
                    {
                        case "u_id":
                            temp.Uid = node.InnerText;
                            break;
                        case "o_id":
                            temp.Oid = node.InnerText;
                            break;
                        case "s_id":
                            temp.Sid = node.InnerText;
                            break;
                        case "r_time":
                            temp.Rtime = node.InnerText;
                            break;
                        case "r_price":
                            temp.Rprice = float.Parse(node.InnerText);
                            break;
                        case "r_status":
                            temp.Rstatus = byte.Parse(node.InnerText);
                            break;
                    }
                }
                records.Add(temp);
            }
            return records;
        }

        public List<OnMovie> GetOnMovies(string Mid)
        {
            //创建XMLDocument
            XmlDocument document = new XmlDocument();
            //根节点
            XmlElement getElement = document.CreateElement("get_onmovies");
            XmlElement m_idElement = document.CreateElement("m_id");
            m_idElement.InnerText = Mid;
            //形成树结构
            getElement.AppendChild(m_idElement);
            document.AppendChild(getElement);
            //报文初始化
            TransMessage message = new TransMessage();
            message.fromAddress = fromAddr;
            message.toAddress = toAddr;
            message.serviceType = EnumServiceType.CUV;
            message.specificType = EnumCUV.GetOnMovies;
            message.contents = XMLPhaser.XmlToString(document);
            message.EnPackage(mySKeyFile, sessionKey);
            transceiver.SendMessage(message);
            message = transceiver.ReceiveMessage();
            message.DePackage(vPKeyFile, sessionKey);
            document = XMLPhaser.StringToXml(message.contents);
            if (message.errorCode == EnumErrorCode.Error)
                throw new Exception("获取某影片所有场次信息函数错误！");
            List<OnMovie> onMovies = new List<OnMovie>();
            XmlElement xmlRoot = document.DocumentElement;
            XmlNodeList onMovieNodeList = xmlRoot.ChildNodes;
            foreach (XmlNode onMovieNode in onMovieNodeList)
            {
                OnMovie temp = new OnMovie();
                XmlNodeList nodeList = onMovieNode.ChildNodes;
                foreach (XmlNode node in nodeList)
                {
                    switch (node.Name)
                    {
                        case "o_id":
                            temp.Oid = node.InnerText;
                            break;
                        case "m_id":
                            temp.Mid = node.InnerText;
                            break;
                        case "t_id":
                            temp.Tid = node.InnerText;
                            break;
                        case "o_begin_time":
                            temp.Obegin = node.InnerText;
                            break;
                        case "o_end_time":
                            temp.Oend = node.InnerText;
                            break;
                        case "o_price":
                            temp.Oprice = float.Parse(node.InnerText);
                            break;
                    }
                }
                onMovies.Add(temp);
            }
            return onMovies;
        }

        public List<Seat> GetSeats(string Oid)
        {
            //创建XMLDocument
            XmlDocument document = new XmlDocument();
            //根节点
            XmlElement getElement = document.CreateElement("get_seats");
            XmlElement o_idElement = document.CreateElement("o_id");
            o_idElement.InnerText = Oid;
            //形成树结构
            getElement.AppendChild(o_idElement);
            document.AppendChild(getElement);
            //报文初始化
            TransMessage message = new TransMessage();
            message.fromAddress = fromAddr;
            message.toAddress = toAddr;
            message.serviceType = EnumServiceType.CUV;
            message.specificType = EnumCUV.GetSeats;
            message.contents = XMLPhaser.XmlToString(document);
            message.EnPackage(mySKeyFile, sessionKey);
            transceiver.SendMessage(message);
            message = transceiver.ReceiveMessage();
            message.DePackage(vPKeyFile, sessionKey);
            document = XMLPhaser.StringToXml(message.contents);
            if (message.errorCode == EnumErrorCode.Error)
                throw new Exception("获取某场次所有座位信息函数错误！");
            List<Seat> seats = new List<Seat>();
            XmlElement xmlRoot = document.DocumentElement;
            XmlNodeList seatNodeList = xmlRoot.ChildNodes;
            foreach (XmlNode seatNode in seatNodeList)
            {
                Seat temp = new Seat();
                XmlNodeList nodeList = seatNode.ChildNodes;
                foreach (XmlNode node in nodeList)
                {
                    switch (node.Name)
                    {
                        case "o_id":
                            temp.Oid = node.InnerText;
                            break;
                        case "s_id":
                            temp.Sid = node.InnerText;
                            break;
                        case "s_status":
                            temp.Sstatus = byte.Parse(node.InnerText);
                            break;
                    }
                }
                seats.Add(temp);
            }
            return seats;
        }

        public Theater GetTheater(string Tid)
        {
            //创建XMLDocument
            XmlDocument document = new XmlDocument();
            //根节点
            XmlElement getElement = document.CreateElement("get_theater");
            XmlElement t_idElement = document.CreateElement("t_id");
            t_idElement.InnerText = Tid;
            //形成树结构
            getElement.AppendChild(t_idElement);
            document.AppendChild(getElement);
            //报文初始化
            TransMessage message = new TransMessage();
            message.fromAddress = fromAddr;
            message.toAddress = toAddr;
            message.serviceType = EnumServiceType.CUV;
            message.specificType = EnumCUV.GetTheater;
            message.contents = XMLPhaser.XmlToString(document);
            message.EnPackage(mySKeyFile, sessionKey);
            transceiver.SendMessage(message);
            message = transceiver.ReceiveMessage();
            message.DePackage(vPKeyFile, sessionKey);
            document = XMLPhaser.StringToXml(message.contents);
            if (message.errorCode == EnumErrorCode.Error)
                throw new Exception("获取某场次影厅信息函数错误！");
            Theater theater = new Theater();
            XmlElement xmlRoot = document.DocumentElement;
            XmlNodeList nodeList = xmlRoot.ChildNodes;
            foreach (XmlNode node in nodeList)
            {
                switch (node.Name)
                {
                    case "t_id":
                        theater.Tid = node.InnerText;
                        break;
                    case "t_type":
                        theater.Ttype = byte.Parse(node.InnerText);
                        break;
                    case "t_size":
                        theater.Tsize = int.Parse(node.InnerText);
                        break;
                }
            }
            return theater;
        }

        public float BuyTicket(Seat[] seats)
        {
            //创建XMLDocument
            XmlDocument document = new XmlDocument();
            //根节点
            XmlElement getElement = document.CreateElement("get_theater");
            XmlElement t_idElement = document.CreateElement("t_id");
            t_idElement.InnerText = Tid;
            //形成树结构
            getElement.AppendChild(t_idElement);
            document.AppendChild(getElement);
            //报文初始化
            TransMessage message = new TransMessage();
            message.fromAddress = fromAddr;
            message.toAddress = toAddr;
            message.serviceType = EnumServiceType.CUV;
            message.specificType = EnumCUV.GetTheater;
            message.contents = XMLPhaser.XmlToString(document);
            message.EnPackage(mySKeyFile, sessionKey);
            transceiver.SendMessage(message);
            message = transceiver.ReceiveMessage();
            message.DePackage(vPKeyFile, sessionKey);
            document = XMLPhaser.StringToXml(message.contents);
            if (message.errorCode == EnumErrorCode.Error)
                throw new Exception("获取某场次影厅信息函数错误！");
        }
    }
}
