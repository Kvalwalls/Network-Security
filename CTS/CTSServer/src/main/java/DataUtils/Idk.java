package DataUtils;

public class Idk {
    private String id;
    private String t_key;

    public Idk(String id,String t_key){
        setId(id);
        setT_key(t_key);
    }

    @Override
    public String toString() {
        return "Idk{" +
                "id='" + id + '\'' +
                ", t_key='" + t_key + '\'' +
                '}';
    }

    public String getId() {
        return id;
    }

    public void setId(String id) {
        this.id = id;
    }

    public String getT_key() {
        return t_key;
    }

    public void setT_key(String t_key) {
        this.t_key = t_key;
    }
}
