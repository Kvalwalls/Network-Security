package TransmissionUtils;

import SecurityUtils.DESHandler;
import SecurityUtils.RSAHandler;

import java.util.ArrayList;
import java.util.List;

public class TransMessage {
    private byte[] toAddress;
    private byte[] fromAddress;
    private byte serviceType;
    private byte specificType;
    private byte errorCode;
    private byte cryptCode;
    private String signature;
    private String contents;

    public TransMessage(byte[] tAddr, byte[] fAddr, byte serviceT, byte specificT, byte cryptC, String con) {
        this.toAddress = tAddr;
        this.fromAddress = fAddr;
        this.serviceType = serviceT;
        this.specificType = specificT;
        this.cryptCode = cryptC;
        this.contents = con;
    }

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

    public void dePackage(String rsaPKeyFile, String desKey) {
        try {
            if (cryptCode == 1)
                contents = DESHandler.decrypt(desKey, contents);
            if (!RSAHandler.verifySign(rsaPKeyFile, signature, contents))
                errorCode = 1;
        }
        catch (Exception e) {
            errorCode = 1;
        }
    }

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