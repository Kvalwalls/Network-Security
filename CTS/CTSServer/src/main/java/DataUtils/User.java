package DataUtils;

public class User {
    //人员号
    private String UId;
    //名称
    private String UName;
    //密码
    private String UPassword;
    //权限
    private byte UAccess;
    //余额
    private float UMoney;

    /**
     * 带参数的构造方法
     */
    public User(String UId, String UName, String UPassword, byte UAccess, float UMoney) {
        this.UId = UId;
        this.UName = UName;
        this.UPassword = UPassword;
        this.UAccess = UAccess;
        this.UMoney = UMoney;
    }

    /**
     * 无参数的构造方法
     */
    public User() { }

    public String getUId() {
        return UId;
    }

    public void setUId(String UId) {
        this.UId = UId;
    }

    public String getUName() {
        return UName;
    }

    public void setUName(String UName) {
        this.UName = UName;
    }

    public String getUPassword() {
        return UPassword;
    }

    public void setUPassword(String UPassword) {
        this.UPassword = UPassword;
    }

    public byte getUAccess() {
        return UAccess;
    }

    public void setUAccess(byte UAccess) {
        this.UAccess = UAccess;
    }

    public float getUMoney() {
        return UMoney;
    }

    public void setUMoney(float UMoney) {
        this.UMoney = UMoney;
    }
}