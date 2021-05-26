package DataUtils;

public class AuthenticatorC {
    private String IDc;
    private String ADc;
    private String timeStamp;

    public String getIDc() {
        return IDc;
    }

    public String getADc() {
        return ADc;
    }

    public String getTimeStamp() {
        return timeStamp;
    }

    public AuthenticatorC(String IDc, String ADc, String timeStamp) {
        this.IDc = IDc;
        this.ADc = ADc;
        this.timeStamp = timeStamp;
    }

    public AuthenticatorC(String msgStr,String key) {

    }

    public String getEncrypted(String key) {
        return null;
    }
}
