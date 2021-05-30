package DataUtils;

public class Theater {
    //影厅号
    private String TId;
    //影厅类型
    private byte TType;
    //影厅大小
    private int TSize;

    /**
     * 带参数的构造方法
     * @param TId
     * @param TType
     * @param TSize
     */
    public Theater(String TId, byte TType, int TSize) {
        this.TId = TId;
        this.TType = TType;
        this.TSize = TSize;
    }
    /**
     * 无参数的构造方法
     */
    public Theater() {

    }

    public String getTId() {
        return TId;
    }

    public void setTId(String TId) {
        this.TId = TId;
    }

    public byte getTType() {
        return TType;
    }

    public void setTType(byte TType) {
        this.TType = TType;
    }

    public int getTSize() {
        return TSize;
    }

    public void setTSize(int TSize) {
        this.TSize = TSize;
    }
}