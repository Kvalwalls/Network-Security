package DataUtils;

import java.util.Date;

public class Record {
    //用户号
    private String UId;
    //场次号
    private String OId;
    //座位号
    private String SId;
    //购票时间
    private Date RTime;
    //实际价格
    private float RPrice;
    //购票状态
    private byte RStatus;

    /**
     * 带参数的构造方法
     */
    public Record(String UId, String OId, String SId, Date RTime, float RPrice, byte RStatus) {
        this.UId = UId;
        this.OId = OId;
        this.SId = SId;
        this.RTime = RTime;
        this.RPrice = RPrice;
        this.RStatus = RStatus;
    }

    /**
     * 无参数的构造方法
     */
    public Record() { }

    public String getUId() {
        return UId;
    }

    public void setUId(String UId) {
        this.UId = UId;
    }

    public String getOId() {
        return OId;
    }

    public void setOId(String OId) {
        this.OId = OId;
    }

    public String getSId() {
        return SId;
    }

    public void setSId(String SId) {
        this.SId = SId;
    }

    public Date getRTime() {
        return RTime;
    }

    public void setRTime(Date RTime) {
        this.RTime = RTime;
    }

    public float getRPrice() {
        return RPrice;
    }

    public void setRPrice(float RPrice) {
        this.RPrice = RPrice;
    }

    public byte getStatus() {
        return RStatus;
    }

    public void setStatus(byte RStatus) {
        this.RStatus = RStatus;
    }
}
