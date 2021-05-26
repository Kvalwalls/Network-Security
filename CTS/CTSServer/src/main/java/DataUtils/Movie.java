package DataUtils;

public class Movie {
    private String id;               //影片号
    private String name;             //影片名称
    private String type;             //影片类型
    private int time;                //影片时长
    private float score;             //影片评分
    private String image;            //影片图片路径
    private String description;      //影片简介

    public Movie() {
    }

    //构造方法
    public Movie(String id, String name, String type, int time, float score, String image, String description) {
        this.id = id;
        this.name = name;
        this.type = type;
        this.time = time;
        this.score = score;
        this.image = image;
        this.description = description;
    }

    public String getId() {     //获取影片号
        return id;
    }

    public void setId(String id) {  //设置影片号
        this.id = id;
    }

    public String getName() {       //获取影片名称
        return name;
    }

    public void setName(String name) {  //设置影片名称
        this.name = name;
    }

    public String getType() {           //获取影片类型
        return type;
    }

    public void setType(String type) {  //设置影片类型
        this.type = type;
    }

    public int getTime() {              //获取影片时长
        return time;
    }

    public void setTime(int time) {     //设置影片时长
        this.time = time;
    }

    public float getScore() {           //获取影片评分
        return score;
    }

    public void setScore(float score) {     //设置影片评分
        this.score = score;
    }

    public String getImage() {          //获取影片图片路径
        return image;
    }

    public void setImage(String image) {    //设置影片图片路径
        this.image = image;
    }

    public String getDescription() {        //获取影片简介
        return description;
    }

    public void setDescription(String description) {        //设置影片简介
        this.description = description;
    }
}
