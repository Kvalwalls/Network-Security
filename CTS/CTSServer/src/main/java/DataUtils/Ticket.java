package DataUtils;

public class Ticket {
    private String key;
    private String IDc;
    private String ADc;
    private String ID;
    private String timeStamp;
    private String lifeTime;

    public String getKey() {
        return key;
    }

    public String getIDc() {
        return IDc;
    }

    public String getADc() {
        return ADc;
    }

    public String getID() {
        return ID;
    }

    public String getTimeStamp() {
        return timeStamp;
    }

    public String getLifeTime() {
        return lifeTime;
    }

    public Ticket(String key, String IDc, String ADc, String ID, String timeStamp, String lifeTime) {
        this.key = key;
        this.IDc = IDc;
        this.ADc = ADc;
        this.ID = ID;
        this.timeStamp = timeStamp;
        this.lifeTime = lifeTime;
    }

    public Ticket(String msgStr,String key) {

    }

    public String getEncrypted(String key) {
        return null;
    }
}
