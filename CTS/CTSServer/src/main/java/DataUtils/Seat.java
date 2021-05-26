package DataUtils;

public class Seat {

    private String sid;               //座位号
    private String oid;               //影厅号
    private String status;            //座位状态

    public Seat() {

    }

    //构造方法
    public Seat(String sid, String oid, String status) {
        this.sid = sid;
        this.oid = oid;
        this.status = status;
    }

    public String getSid() {
        return sid;
    }

    public void setSid(String sid) {
        this.sid = sid;
    }

    public String getOid() {
        return oid;
    }

    public void setOid(String tid) {
        this.oid = tid;
    }

    public String getStatus() {
        return status;
    }

    public void setStatus(String status) {
        this.status = status;
    }
}
