package DataUtils;

public class Authenticator {
    //客户端ID
    private String ID_c;
    //客户端IP地址
    private String AD_c;
    //时间戳
    private long timestamp;

    /**
     * 带参数的构造方法
     */
    public Authenticator(String ID_c, String AD_c, long timestamp) {
        this.ID_c = ID_c;
        this.AD_c = AD_c;
        this.timestamp = timestamp;
    }

    /**
     * 无参数的构造方法
     */
    public Authenticator() { }

    public String getID_c() {
        return ID_c;
    }

    public void setID_c(String ID_c) {
        this.ID_c = ID_c;
    }

    public String getAD_c() {
        return AD_c;
    }

    public void setAD_c(String AD_c) {
        this.AD_c = AD_c;
    }

    public long getTimestamp() {
        return timestamp;
    }

    public void setTimestamp(long timestamp) {
        this.timestamp = timestamp;
    }
}
