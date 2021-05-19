package TransmissionUtils;

import org.w3c.dom.Document;

import java.net.Socket;

public class Transceiver {
    private final Socket socket;

    public Transceiver(Socket socket) {
        this.socket = socket;
    }

    public void sendMessage(TransMessage message) {
        return;
    }

    public TransMessage receiveMessage() {
        return null;
    }

    private Document parseContents(String msgContents) {
        return null;
    }
}
