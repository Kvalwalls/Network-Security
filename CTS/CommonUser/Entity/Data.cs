namespace CommonUser
{
    public class User
    {
        public string Uid { get; set; }
        public string Uname { get; set; }
        public string Upassword { get; set; }
        public string Uaccess { get; set; }
        public float Umoney { get; set; }

        public User(string uid, string uname, string upassword, string uaccess, float umoney)
        {
            Uid = uid;
            Uname = uname;
            Upassword = upassword;
            Uaccess = uaccess;
            Umoney = umoney;
        }

        public User()
        {

        }
        
    }

    public class Movie
    {
        public string Mid { get; set; }
        public string Mname { get; set; }
        public string Mtype { get; set; }
        public int Mtime { get; set; }
        public float Mcomment { get; set; }
        public string Mpicture { get; set; }
        public string Mdescription { get; set; }

        public Movie(string mid, string mname, string mtype, int mtime, float mcomment, string mpicture, string mdescription)
        {
            Mid = mid;
            Mname = mname;
            Mtype = mtype;
            Mtime = mtime;
            Mcomment = mcomment;
            Mpicture = mpicture;
            Mdescription = mdescription;
        }

        public Movie()
        {

        }
    }

    public class Seat
    {
        public string Sid { get; set; }
        public string Oid { get; set; }
        public SeatStatus Sstatus { get; set; }

        public Seat(string sid, string oid, SeatStatus sstatus)
        {
            Sid = sid;
            Oid = oid;
            Sstatus = sstatus;
        }

        public Seat()
        {
        }
    }

    public class Theater
    {
        public string Tid { get; set; }
        public TheaterType Ttype { get; set; }
        public int Tsize { get; set; }

        public Theater(string tid, TheaterType ttype, int tsize)
        {
            Tid = tid;
            Ttype = ttype;
            Tsize = tsize;
        }

        public Theater()
        {
        }
    }

    public class OnMovie
    {
        public string Oid { get; set; }
        public string Mid { get; set; }
        public string Tid { get; set; }
        public string Obegin { get; set; }
        public string Oend { get; set; }
        public float Oprice { get; set; }

        public OnMovie(string oid, string mid, string tid, string obegin, string oend, float oprice)
        {
            Oid = oid;
            Mid = mid;
            Tid = tid;
            Obegin = obegin;
            Oend = oend;
            Oprice = oprice;
        }

        public OnMovie()
        {
        }
    }

	public class Record
	{
		public string Uid { get; set; }
		public string Sid { get; set; }
		public string Oid { get; set; }
        public string Tid { get; set; }
        public string Rtime { get; set; }
		public float Rprice { get; set; }
		public string Rstatus { get; set; }
		public string Obegin { get; set; }
		public string Oend { get; set; }
		public string Mname { get; set; }

        public Record(string uid, string sid, string oid, string tid, string rtime, float rprice, string rstatus, string obegin, string oend, string mname)
        {
            Uid = uid;
            Sid = sid;
            Oid = oid;
            Tid = tid;
            Rtime = rtime;
            Rprice = rprice;
            Rstatus = rstatus;
            Obegin = obegin;
            Oend = oend;
            Mname = mname;
        }

        public Record()
        {

        }
    }

}