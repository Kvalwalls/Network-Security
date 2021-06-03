using CommonUser.Kerberos;
using CommonUser.Security;
using CommonUser.Transmission;
using System;
using System.Xml;

namespace CommonUser.Entity
{
    //用户
    public class User
    {
        //用户号
        public string Uid { get; set; }
        //名称
        public string Uname { get; set; }
        //密码
        public string Upassword { get; set; }
        //权限
        public byte Uaccess { get; set; }
         //余额
        public float Umoney { get; set; }

        /// <summary>
        /// 带参数的构造函数
        /// </summary>
        public User(string uid, string uname, string upassword, byte uaccess, float umoney)
        {
            Uid = uid;
            Uname = uname;
            Upassword = upassword;
            Uaccess = uaccess;
            Umoney = umoney;
        }

        /// <summary>
        /// 无参数的构造函数
        /// </summary>
        public User() { }
    }

    //影片
    public class Movie
    {
        //影片号
        public string Mid { get; set; }
        //名称
        public string Mname { get; set; }
        //类型
        public string Mtype { get; set; }
        //时长
        public int Mtime { get; set; }
        //评分
        public float Mcomment { get; set; }
        //简介
        public string Mdescription { get; set; }

        /// <summary>
        /// 带参数的构造函数
        /// </summary>
        public Movie(string mid, string mname, string mtype, int mtime, float mcomment, string mdescription)
        {
            Mid = mid;
            Mname = mname;
            Mtype = mtype;
            Mtime = mtime;
            Mcomment = mcomment;
            Mdescription = mdescription;
        }

        /// <summary>
        /// 无参数的构造函数
        /// </summary>
        public Movie() { }
    }

    //影厅
    public class Theater
    {
        //影厅号
        public string Tid { get; set; }
        //类型
        public byte Ttype { get; set; }
        //大小
        public int Tsize { get; set; }

        /// <summary>
        /// 带参数的构造函数
        /// </summary>
        public Theater(string tid, byte ttype, int tsize)
        {
            Tid = tid;
            Ttype = ttype;
            Tsize = tsize;
        }

        /// <summary>
        /// 无参数的构造函数
        /// </summary>
        public Theater() { }
    }

    //场次
    public class OnMovie
    {
        //场次号
        public string Oid { get; set; }
        //影片号
        public string Mid { get; set; }
        //影厅号
        public string Tid { get; set; }
        //开始时间
        public string Obegin { get; set; }
        //结束时间
        public string Oend { get; set; }
        //价格
        public float Oprice { get; set; }

        /// <summary>
        /// 带参数的构造函数
        /// </summary>
        public OnMovie(string oid, string mid, string tid, string obegin, string oend, float oprice)
        {
            Oid = oid;
            Mid = mid;
            Tid = tid;
            Obegin = obegin;
            Oend = oend;
            Oprice = oprice;
        }

        /// <summary>
        /// 无参数的构造函数
        /// </summary>
        public OnMovie() { }
    }

    //座位
    public class Seat
    {
        //座位号
        public string Sid { get; set; }
        //场次号
        public string Oid { get; set; }
        //状态
        public byte Sstatus { get; set; }
        
        /// <summary>
        /// 带参数的构造函数
        /// </summary>
        public Seat(string sid, string oid, byte sstatus)
        {
            Sid = sid;
            Oid = oid;
            Sstatus = sstatus;
        }

        /// <summary>
        /// 无参数的构造函数
        /// </summary>
        public Seat() { }
    }

    //购票记录
	public class Record
	{
        //用户号
		public string Uid { get; set; }
        //场次号
        public string Oid { get; set; }
        //座位号
        public string Sid { get; set; }
        //购票时间
        public string Rtime { get; set; }
		//实际价格
        public float Rprice { get; set; }
        //购票状态
		public byte Rstatus { get; set; }

        /// <summary>
        /// 带参数的构造函数
        /// </summary>
        public Record(string uid, string oid, string sid, string rtime, float rprice, byte rstatus)
        {
            Uid = uid;
            Oid = oid;
            Sid = sid;
            Rtime = rtime;
            Rprice = rprice;
            Rstatus = rstatus;
        }

        /// <summary>
        /// 无参数的构造函数
        /// </summary>
        public Record() { }
    }

    //身份验证
    public class Authenticator
    {
        //客户端ID
        public string ID_c { get; set; }
        //客户端IP地址
        public string AD_c { get; set; }
        //时间戳
        public long timestamp { get; set; }

        /// <summary>
        /// 带参数的构造函数
        /// </summary>
        public Authenticator(string iD_c, string aD_c, long timestamp)
        {
            ID_c = iD_c;
            AD_c = aD_c;
            this.timestamp = timestamp;
        }

        /// <summary>
        /// 无参数的构造函数
        /// </summary>
        public Authenticator() { }

        public string generateAuthenticator(string enKey)
        {
            //创建XMLDocument
            XmlDocument document = new XmlDocument();
            //根节点
            XmlElement authenticator = document.CreateElement("authenticator");
            //子节点
            XmlElement ID_cElement = document.CreateElement("id_c");
            ID_cElement.InnerText = ID_c;
            XmlElement AD_cElement = document.CreateElement("ad_c");
            AD_cElement.InnerText = AD_c;
            XmlElement TSElement = document.CreateElement("ts");
            TSElement.InnerText = timestamp.ToString();
            Console.WriteLine("generateAuthenticator" + timestamp);
            //形成树结构
            authenticator.AppendChild(ID_cElement);
            authenticator.AppendChild(AD_cElement);
            authenticator.AppendChild(TSElement);
            document.AppendChild(authenticator);
            return DESHandler.Encrypt(enKey, XMLPhaser.XmlToString(document));
        }
    }
}