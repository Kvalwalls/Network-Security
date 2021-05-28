package TransmissionUtils;

import SecurityUtils.DESHandler;
import SecurityUtils.RSAHandler;

import java.util.ArrayList;
import java.util.List;

public class TransMessage {
    //目的IP地址
    private byte[] toAddress;
    //源IP地址
    private byte[] fromAddress;
    //服务类型
    private byte serviceType;
    //具体类型
    private byte specificType;
    //错误码
    private byte errorCode;
    //加密码
    private byte cryptCode;
    //数字签名
    private String signature;
    //报文内容
    private String contents;

    /**
     * 构造方法
     */
    public TransMessage(byte[] tAddr, byte[] fAddr, byte serviceT, byte specificT, byte cryptC, String con) {
        this.toAddress = tAddr;
        this.fromAddress = fAddr;
        this.serviceType = serviceT;
        this.specificType = specificT;
        this.cryptCode = cryptC;
        this.contents = con;
    }

    /**
     * 构造方法
     */
    public TransMessage() {
        this.toAddress = new byte[4];
        this.fromAddress = new byte[4];
    }

    public byte[] getToAddress() {
        return toAddress;
    }

    public void setToAddress(byte[] toAddress) {
        this.toAddress = toAddress;
    }

    public byte[] getFromAddress() {
        return fromAddress;
    }

    public void setFromAddress(byte[] fromAddress) {
        this.fromAddress = fromAddress;
    }

    public byte getServiceType() {
        return serviceType;
    }

    public void setServiceType(byte serviceType) {
        this.serviceType = serviceType;
    }

    public byte getSpecificType() {
        return specificType;
    }

    public void setSpecificType(byte specificType) {
        this.specificType = specificType;
    }

    public byte getErrorCode() {
        return errorCode;
    }

    public void setErrorCode(byte errorCode) {
        this.errorCode = errorCode;
    }

    public byte getCryptCode() {
        return cryptCode;
    }

    public void setCryptCode(byte cryptCode) {
        this.cryptCode = cryptCode;
    }

    public String getSignature() {
        return signature;
    }

    public void setSignature(String signature) {
        this.signature = signature;
    }

    public String getContents() {
        return contents;
    }

    public void setContents(String contents) {
        this.contents = contents;
    }

    /**
     * 报文封装方法
     *
     * @param rsaSKeyFile RSA私钥文件名
     * @param desKey      DES密钥
     */
    public void enPackage(String rsaSKeyFile, String desKey) {
        try {
            signature = RSAHandler.generateSign(rsaSKeyFile, contents);
            if (cryptCode == 1)
                contents = DESHandler.encrypt(desKey, contents);
            errorCode = 0;
        } catch (Exception e) {
            errorCode = 1;
        }
    }

    /**
     * 报文解封方法
     *
     * @param rsaPKeyFile RSA公钥文件名
     * @param desKey      DES密钥
     */
    public void dePackage(String rsaPKeyFile, String desKey) {
        try {
            if (cryptCode == 1)
                contents = DESHandler.decrypt(desKey, contents);
            if (!RSAHandler.verifySign(rsaPKeyFile, signature, contents))
                errorCode = 1;
        } catch (Exception e) {
            errorCode = 1;
        }
    }

    /**
     * 报文转换字节流方法
     *
     * @return 字节流
     */
    public byte[] MessageToBytes() {
        List<Byte> byteList = new ArrayList<>();
        for (byte b : toAddress)
            byteList.add(b);
        for (byte b : fromAddress)
            byteList.add(b);
        byteList.add(serviceType);
        byteList.add(specificType);
        byteList.add(errorCode);
        byteList.add(cryptCode);
        for (byte b : String.format("%-4d", signature.length()).getBytes())
            byteList.add(b);
        for (byte b : String.format("%-4d", contents.length()).getBytes())
            byteList.add(b);
        for (byte b : signature.getBytes())
            byteList.add(b);
        for (byte b : contents.getBytes())
            byteList.add(b);
        byte[] finalByteList = new byte[byteList.size()];
        for (int i = 0; i < byteList.size(); i++)
            finalByteList[i] = byteList.get(i);
        return finalByteList;
    }
}