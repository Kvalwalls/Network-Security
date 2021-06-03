package AppServiceUtils;

import TransmissionUtils.TransMessage;
import TransmissionUtils.Transceiver;

import java.net.Socket;

public class CommonUserVHandler extends VHandler implements Runnable {
    private final Transceiver transceiver;

    @Override
    public void run() {

    }

    public CommonUserVHandler(Socket socket) {
        this.transceiver = new Transceiver(socket);
    }

    private void logIn(TransMessage transMessage) {

    }

    private void logOut(TransMessage transMessage) {

    }

    private void getReords(TransMessage transMessage) {

    }

    private void getOnMovie(TransMessage transMessage) {

    }

    private void buyTicket(TransMessage transMessage) {

    }

    private void refundTicket(TransMessage transMessage) {

    }

    private void modifyPWD(TransMessage transMessage) {

    }

    private void recharge(TransMessage transMessage) {

    }
}