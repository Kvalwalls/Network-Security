using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 服务器UI
{
    public class User
    {
        public string id { set; get; }
        public string name { set; get; }
        public string password { set; get; }
        public string access { set; get; }
        public float money { set; get; }

        public User(string id, string name, string password, string access, float money)
        {
            this.id = id;
            this.name = name;
            this.password = password;
            this.access = access;
            this.money = money;
        }

        public User()
        {

        }

    }

    public class Theater
    {
        public string id { set; get; }
        public string type { set; get; }
        public int size { set; get; }

        public Theater(string id, string type, int size)
        {
            this.id = id;
            this.type = type;
            this.size = size;
        }

        public Theater()
        {

        }

    }

    public class Movie
    {
        public string id { set; get; }
        public string name { set; get; }
        public string type { set; get; }
        public int time { set; get; }
        public float comment { set; get; }
        public string picture { set; get; }
        public string description { set; get; }


        public Movie(string id, string name, string type, int time, float comment, string picture, string description)
        {
            this.id = id;
            this.name = name;
            this.type = type;
            this.time = time;
            this.comment = comment;
            this.picture = picture;
            this.description = description;
        }

        public Movie()
        {

        }

    }

    public class MovieForShow
    {
        public string id { set; get; }
        public string name { set; get; }
        public string type { set; get; }
        public int time { set; get; }
        public float comment { set; get; }


        public MovieForShow(string id, string name, string type, int time, float comment)
        {
            this.id = id;
            this.name = name;
            this.type = type;
            this.time = time;
            this.comment = comment;
        }

        public MovieForShow()
        {

        }

    }

    public class OnMovie
    {
        public string oid { set; get; }
        public string mid { set; get; }
        public string tid { set; get; }
        public DateTime begintime { set; get; }
        public DateTime endtime { set; get; }
        public float price { set; get; }

        public OnMovie(string oid, string mid, string tid, DateTime begintime, DateTime endtime, float price)
        {
            this.oid = oid;
            this.mid = mid;
            this.tid = tid;
            this.begintime = begintime;
            this.endtime = endtime;
            this.price = price;
        }

        public OnMovie()
        {

        }

    }

    public class Record
    {
        public string uid { set; get; }
        public string oid { set; get; }
        public string sid { set; get; }
        public DateTime time { set; get; }
        public float price { set; get; }
        public string status { set; get; }

        public Record(string uid, string oid, string sid, DateTime time, float price, string status)
        {
            this.uid = uid;
            this.oid = oid;
            this.sid = sid;
            this.time = time;
            this.price = price;
            this.status = status;
        }

        public Record()
        {

        }

    }

    public class RecordForUserSta
    {
        public string oid { set; get; }
        public string sid { set; get; }
        public DateTime time { set; get; }
        public float price { set; get; }
        public string status { set; get; }

        public RecordForUserSta(string oid, string sid, DateTime time, float price, string status)
        {
            
            this.oid = oid;
            this.sid = sid;
            this.time = time;
            this.price = price;
            this.status = status;
        }

        public RecordForUserSta()
        {

        }

    }

    public class RecordForOnMovieSta
    {
        public string uid { set; get; }
        public string sid { set; get; }
        public DateTime time { set; get; }
        public float price { set; get; }
        public string status { set; get; }

        public RecordForOnMovieSta(string uid, string sid, DateTime time, float price, string status)
        {
            this.uid = uid;
            this.sid = sid;
            this.time = time;
            this.price = price;
            this.status = status;
        }

        public RecordForOnMovieSta()
        {

        }

    }

    public class TransMessageR
    {
        public int no { set; get; }
        public DateTime time { set; get; }
        public string Sid { set; get; }
        public string Did { set; get; }
        public string SerType { set; get; }
        public string SpeType { set; get; }
        public string Error { set; get; }
        public string Crypt { set; get; }
        public int SLength { set; get; }
        public int CLength { set; get; }
        public string content { set; get; }
        //public TransMessage trans { set; get; }


        public TransMessageR(int no, DateTime time, string Sid, string Did, string SerType, string SpeType, string Error, int SLenth, int CLenth, string content)
        {
            this.no = no;
            this.time = time;
            this.Sid = Sid;
            this.Did = Did;
            this.SerType = SerType;
            this.SpeType = SpeType;
            this.Error = Error;
            this.SLength = SLenth;
            this.CLength = CLenth;
            this.content = content;
        }

        public TransMessageR()
        {

        }

    }
}
