package DataUtils;

public class Seat {
    //座位号
    private String SId;
    //场次号
    private String OId;
    //座位状态
    private byte SStatus;

    /**
     * 带参数的构造方法
     */
    public Seat(String SId, String OId, byte SStatus) {
        this.SId = SId;
        this.OId = OId;
        this.SStatus = SStatus;
    }

    /**
     * 无参数的构造函数
     */
    public Seat() {
    }

    public String getSId() {
        return SId;
    }

    public void setSId(String SId) {
        this.SId = SId;
    }

    public String getOId() {
        return OId;
    }

    public void setOId(String OId) {
        this.OId = OId;
    }

    public byte getSStatus() {
        return SStatus;
    }

    public void setSStatus(byte SStatus) {
        this.SStatus = SStatus;
    }
}
