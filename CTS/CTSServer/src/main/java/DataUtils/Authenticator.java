package DataUtils;

import SecurityUtils.DESHandler;
import TransmissionUtils.XMLPhaser;
import org.w3c.dom.Document;
import org.w3c.dom.Element;
import org.w3c.dom.Node;
import org.w3c.dom.NodeList;

public class Authenticator {
    //客户端ID
    private String ID_c;
    //客户端IP地址
    private String AD_c;
    //时间戳
    private long timestamp;

    /**
     * 带参数的构造方法
     */
    public Authenticator(String authStr,String enKey) throws Exception {
        authStr = DESHandler.decrypt(enKey, authStr);
        Document document = XMLPhaser.stringToDoc(authStr);
        Element authEle = document.getDocumentElement();
        NodeList nodeList = authEle.getChildNodes();
        for (int i = 0; i < nodeList.getLength(); i++) {
            Node childNode = nodeList.item(i);
            switch (childNode.getNodeName()) {
                case "id_c" -> ID_c = childNode.getTextContent();
                case "ad_c" -> AD_c = childNode.getTextContent();
                case "ts" -> timestamp = Long.parseLong(childNode.getTextContent());
            }
        }
    }

    /**
     * 无参数的构造方法
     */
    public Authenticator() { }

    public String getID_c() {
        return ID_c;
    }

    public void setID_c(String ID_c) {
        this.ID_c = ID_c;
    }

    public String getAD_c() {
        return AD_c;
    }

    public void setAD_c(String AD_c) {
        this.AD_c = AD_c;
    }

    public long getTimestamp() {
        return timestamp;
    }

    public void setTimestamp(long timestamp) {
        this.timestamp = timestamp;
    }
}
