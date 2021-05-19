package Data;

import java.util.Date;

public class Record {

    public String uid;                  //用户号
    public String sid;                  //座位号
    public String oid;                  //上映号
    public Date time;                   //购买时间
    public float price;                 //实际价格
    public String status;               //状态

    public Record(){}
    //构造方法
    public Record(String uid, String sid, String oid, Date time, float price, String status) {
        this.uid = uid;
        this.sid = sid;
        this.oid = oid;
        this.time = time;
        this.price = price;
        this.status = status;
    }

    public String getUid() {     //获取用户号
        return uid;
    }

    public void setUid(String uid) {  //设置用户号
        this.uid = uid;
    }

    public String getSid() {     //获取座位号
        return sid;
    }

    public void setSid(String sid) {    //设置座位号
        this.sid = sid;
    }

    public String getOid() {     //获取上映号
        return oid;
    }

    public void setOid(String oid) {  //设置上映号
        this.oid = oid;
    }

    public Date getTime() {       //获取购买时间
        return time;
    }

    public void setTime(Date time) {  //设置购买时间
        this.time = time;
    }

    public String getStatus() {           //获取状态
        return status;
    }

    public void setStatus(String status) {      //设置状态
        this.status = status;
    }

    public float getPrice() {              //获取实际价格
        return price;
    }

    public void setPrice(float price) {     //设置实际价格
        this.price = price;
    }
}
