package DataUtils;

public class Movie {
    //影片号
    private String MId;
    //名称
    private String MName;
    //类型
    private String MType;
    //时长
    private int time;
    //评分
    private float MComment;
    //简介
    private String description;

    /**
     * 带参数的构造方法
     */
    public Movie(String MId, String MName, String MType, int time, float MComment, String description) {
        this.MId = MId;
        this.MName = MName;
        this.MType = MType;
        this.time = time;
        this.MComment = MComment;
        this.description = description;
    }

    /**
     * 无参数的构造方法
     */
    public Movie() {

    }

    public String getMId() {
        return MId;
    }

    public void setMId(String MId) {
        this.MId = MId;
    }

    public String getMName() {
        return MName;
    }

    public void setMName(String MName) {
        this.MName = MName;
    }

    public String getMType() {
        return MType;
    }

    public void setMType(String MType) {
        this.MType = MType;
    }

    public int getTime() {
        return time;
    }

    public void setTime(int time) {
        this.time = time;
    }

    public float getMComment() {
        return MComment;
    }

    public void setMComment(float MComment) {
        this.MComment = MComment;
    }

    public String getDescription() {
        return description;
    }

    public void setDescription(String description) {
        this.description = description;
    }
}
