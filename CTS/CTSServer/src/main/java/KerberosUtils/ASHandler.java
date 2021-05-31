package KerberosUtils;

import DataBaseUtils.DBCommand;
import EnumUtils.EnumCryptCode;
import EnumUtils.EnumErrorCode;
import EnumUtils.EnumKerberos;
import EnumUtils.EnumServiceType;
import PropertiesUtils.PropertiesHandler;
import TransmissionUtils.AddressPhaser;
import TransmissionUtils.TransMessage;
import TransmissionUtils.Transceiver;
import TransmissionUtils.XMLBuilder;
import org.w3c.dom.Document;
import org.w3c.dom.Element;

import java.net.Socket;
import java.util.Properties;

public class ASHandler implements Runnable {
    private static final int LIFE_TIME = 100000;
    private final Transceiver transceiver;
    private String encryptKey;

    /**
     * 构造方法
     *
     * @param socket socket用于初始化transceiver
     */
    public ASHandler(Socket socket) {
        this.transceiver = new Transceiver(socket);
    }

    @Override
    public void run() {
        try {
            while(true) {
                TransMessage message = transceiver.receiveMessage();
                message.dePackage("src\\resourcesAS\\KeyFiles\\Client.pk",null);

                switch(message.getSpecificType()) {
                    case EnumKerberos.Request: {

                    }
                    case EnumKerberos.End: {

                        return;
                    }
                    default:
                        throw new Exception("未知服务类型！");
                }
            }
        } catch (Exception exception) {
            exception.printStackTrace();
        }
    }

    private TransMessage generateNormalReply(String toAddr,String[] contents) throws Exception {
        //创建XMLDocument
        Document document = XMLBuilder.buildXMLDoc();
        //根节点
        Element root = document.createElement("reply");
        //子节点
        Element keyElement = document.createElement("key");
        keyElement.setTextContent(contents[0]);
        Element id_tgsElement = document.createElement("id_tgs");
        id_tgsElement.setTextContent(contents[1]);
        Element ts2Element = document.createElement("ts2");
        ts2Element.setTextContent(contents[2]);
        Element lifetimeElement = document.createElement("lifetime");
        lifetimeElement.setTextContent(contents[3]);
        Element ticket_tgsElement = document.createElement("ticket_tgs");
        ticket_tgsElement.setTextContent(contents[4]);
        //形成树结构
        root.appendChild(keyElement);
        root.appendChild(id_tgsElement);
        root.appendChild(ts2Element);
        root.appendChild(lifetimeElement);
        root.appendChild(ticket_tgsElement);
        document.appendChild(root);
        //报文初始化
        TransMessage message = new TransMessage();
        message.setToAddress(AddressPhaser.StringToBytes(toAddr));
        message.setFromAddress(AddressPhaser.StringToBytes(
                PropertiesHandler.getPropertiesElement("My_IPAddress")));
        message.setServiceType(EnumServiceType.AS);
        message.setSpecificType(EnumKerberos.Reply);
        //这里的key要修改
        message.enPackage("src\\resourcesAS\\KeyFiles\\AS.sk", "0000");
    }

    private TransMessage generateErrorReply(String toAddr) {
        TransMessage message = new TransMessage();
        message.setToAddress(AddressPhaser.StringToBytes(toAddr));
        message.setFromAddress(AddressPhaser.StringToBytes(
                PropertiesHandler.getPropertiesElement("My_IPAddress")));
        message.setServiceType(EnumServiceType.AS);
        message.setSpecificType(EnumKerberos.Error);
        message.setContents("ASError");
        message.enPackage("src\\resourcesAS\\KeyFiles\\AS.sk", null);
        return message;
    }

    private long generateTS() {
        return (System.currentTimeMillis() / 1000);
    }

    private boolean verifyTS(long ts, long lifetime) {
        return (generateTS() - ts < lifetime);
    }
}