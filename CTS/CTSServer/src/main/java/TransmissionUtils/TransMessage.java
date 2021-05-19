package TransmissionUtils;

import org.w3c.dom.Document;

public class TransMessage {
    private byte[] fromAddress;
    private byte[] toAddress;
    private byte serviceType;
    private byte specificType;
    private byte errorCode;
    private int signLength;
    private int contentLength;
    private String signature;
    private Document contents;

    public TransMessage() {

    }

    public TransMessage(byte[] bytes) {

    }

    public TransMessage(byte[] fromAddress, byte[] toAddress, byte serviceType, byte specificType, byte errorCode, int contentLength, Document contents) {
        this.fromAddress = fromAddress;
        this.toAddress = toAddress;
        this.serviceType = serviceType;
        this.specificType = specificType;
        this.errorCode = errorCode;
        this.contentLength = contentLength;
        this.contents = contents;
    }

    public TransMessage(byte[] fromAddress, byte[] toAddress, byte serviceType, byte specificType, byte errorCode, int signLength, int contentLength, String signature, Document contents) {
        this.fromAddress = fromAddress;
        this.toAddress = toAddress;
        this.serviceType = serviceType;
        this.specificType = specificType;
        this.errorCode = errorCode;
        this.signLength = signLength;
        this.contentLength = contentLength;
        this.signature = signature;
        this.contents = contents;
    }

    public byte[] getFromAddress() {
        return fromAddress;
    }

    public void setFromAddress(byte[] fromAddress) {
        this.fromAddress = fromAddress;
    }

    public byte[] getToAddress() {
        return toAddress;
    }

    public void setToAddress(byte[] toAddress) {
        this.toAddress = toAddress;
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

    public int getSignLength() {
        return signLength;
    }

    public void setSignLength(int signLength) {
        this.signLength = signLength;
    }

    public int getContentLength() {
        return contentLength;
    }

    public void setContentLength(int contentLength) {
        this.contentLength = contentLength;
    }

    public String getSignature() {
        return signature;
    }

    public void setSignature(String signature) {
        this.signature = signature;
    }

    public Document getContents() {
        return contents;
    }

    public void setContents(Document contents) {
        this.contents = contents;
    }

    public boolean verifySign() {
        return false;
    }

    public byte[] convertBytes() {
        return null;
    }

    public byte[] convertEncryptedBytes(String key) {
        return null;
    }

    private void generateSign() {

    }
}