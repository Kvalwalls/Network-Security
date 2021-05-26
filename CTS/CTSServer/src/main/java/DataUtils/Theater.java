package DataUtils;

public class Theater {
    private String id;           //影厅号
    private String type;         //影厅类型
    private int size;            //影厅大小


    public Theater() {
    }

    //构造方法
    public Theater(String id, String type, int size) {
        this.id = id;
        this.type = type;
        this.size = size;

    }

    public String getId() {
        return id;
    }

    public void setId(String id) {
        this.id = id;
    }

    public String getType() {
        return type;
    }

    public void setType(String type) {
        this.type = type;
    }

    public int getSize() {
        return size;
    }

    public void setSize(int size) {
        this.size = size;
    }
}


