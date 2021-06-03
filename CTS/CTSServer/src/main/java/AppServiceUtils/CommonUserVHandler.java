package AppServiceUtils;

import TransmissionUtils.TransMessage;
import TransmissionUtils.Transceiver;

import java.net.Socket;

public class CommonUserVHandler extends VHandler implements Runnable {


    @Override
    public void run() {
        try {
            while (true) {
                if (VCertification()) {
                    break;
                } else {
                    continue;
                }
            }
            System.out.println("yes!");
        } catch (Exception exception) {
            exception.printStackTrace();
        }
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