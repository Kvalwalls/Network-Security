package DataUtils;

public class Ticket {
    //密钥
    private String key;
    //客户端ID
    private String ID_c;
    //客户端IP地址
    private String AD_c;
    //目的机器ID
    private String ID_dest;
    //时间戳
    private long timestamp;
    //生命周期
    private long lifetime;

    /**
     * 带参数的构造方法
     */
    public Ticket(String key, String ID_c, String AD_c, String ID_dest, long timestamp, long lifetime) {
        this.key = key;
        this.ID_c = ID_c;
        this.AD_c = AD_c;
        this.ID_dest = ID_dest;
        this.timestamp = timestamp;
        this.lifetime = lifetime;
    }

    /**
     * 无参数的构造方法
     */
    public Ticket() {

    }

    public String getKey() {
        return key;
    }

    public void setKey(String key) {
        this.key = key;
    }

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

    public String getID_dest() {
        return ID_dest;
    }

    public void setID_dest(String ID_dest) {
        this.ID_dest = ID_dest;
    }

    public long getTimestamp() {
        return timestamp;
    }

    public void setTimestamp(long timestamp) {
        this.timestamp = timestamp;
    }

    public long getLifetime() {
        return lifetime;
    }

    public void setLifetime(long lifetime) {
        this.lifetime = lifetime;
    }
}