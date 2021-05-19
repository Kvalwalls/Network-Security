package Data;

public class User {
    public static final String ACCESS_ROOT = "00";//超级管理员
    public static final String ACCESS_ADMIN = "01";//普通管理员
    public static final String ACCESS_COMM = "10";//普通用户
    public static final String ACCESS_VIP = "11";//VIP用户
    public static final String ACCESS_STU = "12";//学生用户

    private String id;//人员号
    private String password;//密码
    private String access;//权限
    private int money;//余额

    public User() {

    }

    public User(String id, String password, String access, int money) {
        this.id = id;
        this.password = password;
        this.access = access;
        this.money = money;
    }

    public String getId() {
        return id;
    }

    public void setId(String id) {
        this.id = id;
    }

    public String getAccess() {
        return access;
    }

    public void setAccess(String access) {
        this.access = access;
    }

    public String getPassword() {
        return password;
    }

    public void setPassword(String password) {
        this.password = password;
    }

    public int getMoney() {
        return money;
    }

    public void setMoney(int money) {
        this.money = money;
    }
}
