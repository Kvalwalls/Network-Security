package DataUtils;

import SecurityUtils.DESHandler;
import TransmissionUtils.XMLBuilder;
import TransmissionUtils.XMLPhaser;
import org.w3c.dom.Document;
import org.w3c.dom.Element;
import org.w3c.dom.Node;
import org.w3c.dom.NodeList;


public class Ticket {
    //密钥
    private String key;
    //客户端ID
    private String ID_c;
    //客户端IP地址
    private String AD_c;
    //目的机器ID
    private String ID_dest;
    //时间戳
    private long timestamp;
    //生命周期
    private long lifetime;

    /**
     * 带参数的构造方法
     */
    public Ticket(String ticketStr,String enKey) throws Exception {
        ticketStr = DESHandler.decrypt(enKey,ticketStr);
        Document document = XMLPhaser.stringToDoc(ticketStr);
        Element ticketEle = document.getDocumentElement();
        NodeList nodeList = ticketEle.getChildNodes();
        for (int i = 0; i < nodeList.getLength(); i++) {
            Node childNode = nodeList.item(i);
            switch (childNode.getNodeName()) {
                case "key" -> key = childNode.getTextContent();
                case "id_c" -> ID_c = childNode.getTextContent();
                case "ad_c" -> AD_c = childNode.getTextContent();
                case "id_dest" -> ID_dest = childNode.getTextContent();
                case "ts" -> timestamp = Long.parseLong(childNode.getTextContent());
                case "lifetime" -> lifetime = Long.parseLong(childNode.getTextContent());
            }
        }
    }

    /**
     * 无参数的构造方法
     */
    public Ticket() { }

    public String getKey() {
        return key;
    }

    public void setKey(String key) {
        this.key = key;
    }

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

    public String getID_dest() {
        return ID_dest;
    }

    public void setID_dest(String ID_dest) {
        this.ID_dest = ID_dest;
    }

    public long getTimestamp() {
        return timestamp;
    }

    public void setTimestamp(long timestamp) {
        this.timestamp = timestamp;
    }

    public long getLifetime() {
        return lifetime;
    }

    public void setLifetime(long lifetime) {
        this.lifetime = lifetime;
    }

    public String generateTicket(String enKey) throws Exception {
        //创建XMLDocument
        Document document = XMLBuilder.buildXMLDoc();
        //根节点
        Element ticketEle = document.createElement("ticket");
        //子节点
        Element keyEle = document.createElement("key");
        keyEle.setTextContent(key);
        Element id_cEle = document.createElement("id_c");
        id_cEle.setTextContent(ID_c);
        Element ad_cEle = document.createElement("ad_c");
        ad_cEle.setTextContent(AD_c);
        Element id_destEle = document.createElement("id_dest");
        id_destEle.setTextContent(ID_dest);
        Element ts2Ele = document.createElement("ts");
        ts2Ele.setTextContent(String.valueOf(timestamp));
        Element lifetimeEle = document.createElement("lifetime");
        lifetimeEle.setTextContent(String.valueOf(lifetime));
        //形成树结构
        ticketEle.appendChild(keyEle);
        ticketEle.appendChild(id_cEle);
        ticketEle.appendChild(ad_cEle);
        ticketEle.appendChild(id_destEle);
        ticketEle.appendChild(ts2Ele);
        ticketEle.appendChild(lifetimeEle);
        document.appendChild(ticketEle);
        //加密
        String text = XMLPhaser.docToString(document);
        return DESHandler.encrypt(enKey,text);
    }
}