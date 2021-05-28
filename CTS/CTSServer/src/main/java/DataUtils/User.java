package DataUtils;

public class User {
    public static final String ACCESS_ROOT = "00";//超级管理员
    public static final String ACCESS_ADMIN = "01";//普通管理员
    public static final String ACCESS_COMM = "10";//普通用户
    public static final String ACCESS_VIP = "11";//VIP用户
    public static final String ACCESS_STU = "12";//学生用户

    private String id;//人员号
    private String name;
    private String password;//密码
    private String access;//权限
    private float money;//余额

    public User() {

    }

    public User(String id, String name,String password, String access, float money) {
        this.id = id;
        this.password = password;
        this.access = access;
        this.money = money;
        this.name=name;
    }

    public String getName() {
        return name;
    }

    public void setName(String name) {
        this.name = name;
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

    public float getMoney() {
        return money;
    }

    public void setMoney(float money) {
        this.money = money;
    }
}
