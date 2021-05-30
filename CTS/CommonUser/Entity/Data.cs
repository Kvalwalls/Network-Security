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

    //票据
    public class Ticket
    {
        //密钥
        public string key { get; set; }
        //客户端ID
        public string ID_c { get; set; }
        //客户端IP地址
        public string AD_c { get; set; }
        //目的机器ID
        public string ID_dest { get; set; }
        //时间戳
        public long timeStamp { get; set; }
        //生命周期
        public long lifeTime { get; set; }

        /// <summary>
        /// 带参数的构造函数
        /// </summary>
        public Ticket(string key, string iD_c, string aD_c, string iD_dest, long timeStamp, long lifeTime)
        {
            this.key = key;
            ID_c = iD_c;
            AD_c = aD_c;
            ID_dest = iD_dest;
            this.timeStamp = timeStamp;
            this.lifeTime = lifeTime;
        }

        /// <summary>
        /// 无参数的构造函数
        /// </summary>
        public Ticket() { }
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
    }
}