package DataUtils;

import java.util.Date;

public class OnMovie {
    //场次号
    private String OId;
    //影片号
    private String MId;
    //影厅号
    private String TId;
    //开始时间
    private Date OBegin;
    //结束时间
    private Date OEnd;
    //价格
    private float OPrice;

    /**
     * 带参数的构造方法
     */
    public OnMovie(String OId, String MId, String TId, Date OBegin, Date OEnd, float OPrice) {
        this.OId = OId;
        this.MId = MId;
        this.TId = TId;
        this.OBegin = OBegin;
        this.OEnd = OEnd;
        this.OPrice = OPrice;
    }

    /**
     * 无参数的构造方法
     */
    public OnMovie() {
    }

    public String getOId() {
        return OId;
    }

    public void setOId(String OId) {
        this.OId = OId;
    }

    public String getMId() {
        return MId;
    }

    public void setMId(String MId) {
        this.MId = MId;
    }

    public String getTId() {
        return TId;
    }

    public void setTId(String TId) {
        this.TId = TId;
    }

    public Date getOBegin() {
        return OBegin;
    }

    public void setOBegin(Date OBegin) {
        this.OBegin = OBegin;
    }

    public Date getOEnd() {
        return OEnd;
    }

    public void setOEnd(Date OEnd) {
        this.OEnd = OEnd;
    }

    public float getOPrice() {
        return OPrice;
    }

    public void setOPrice(float OPrice) {
        this.OPrice = OPrice;
    }
}
