package Data;

import java.util.Date;

public class OnMovie {
    private String oid;              //上映号
    private String mid;              //影片号
    private String tid;              //影厅号
    private Date startTime;        //开始时间
    private Date endTime;          //结束时间
    private float price;               //票价

    public OnMovie() {

    }

    //构造方法
    public OnMovie(String oid, String mid, String tid, Date startTime, Date endTime, float price) {
        this.oid = oid;
        this.mid = mid;
        this.tid = tid;
        this.startTime = startTime;
        this.endTime = endTime;
        this.price = price;
    }

    public String getOid() {
        return oid;
    }

    public void setOid(String oid) {
        this.oid = oid;
    }

    public String getMid() {
        return mid;
    }

    public void setMid(String mid) {
        this.mid = mid;
    }

    public String getTid() {
        return tid;
    }

    public void setTid(String tid) {
        this.tid = tid;
    }

    public Date getStartTime() {
        return startTime;
    }

    public void setStartTime(Date startTime) {
        this.startTime = startTime;
    }

    public Date getEndTime() {
        return endTime;
    }

    public void setEndTime(Date endTime) {
        this.endTime = endTime;
    }

    public float getPrice() {
        return price;
    }

    public void setPrice(float price) {
        this.price = price;
    }
}
