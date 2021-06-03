package AppServiceUtils;

import TransmissionUtils.TransMessage;
import TransmissionUtils.Transceiver;

import java.net.Socket;
import java.nio.file.Path;

public class AdminUserVHandler extends VHandler implements Runnable {


    @Override
    public void run() {

    }

    public AdminUserVHandler(Socket socket) {
        this.transceiver = new Transceiver(socket);
    }

    private byte[] readPicture(Path picName) {
        return null;
    }

    private void logIn(TransMessage transMessage) {

    }

    private void logOut(TransMessage transMessage) {

    }

    private void addUser(TransMessage transMessage) {

    }

    private void deleteUser(TransMessage transMessage) {

    }

    private void getUser(TransMessage transMessage) {

    }

    private void addMovie(TransMessage transMessage) {

    }

    private void deleteMovie(TransMessage transMessage) {

    }

    private void getMovie(TransMessage transMessage) {

    }

    private void addTheater(TransMessage transMessage) {

    }

    private void deleteTheater(TransMessage transMessage) {

    }

    private void getTheater(TransMessage transMessage) {

    }

    private void addOnMovie(TransMessage transMessage) {

    }

    private void deleteOnMovie(TransMessage transMessage) {

    }

    private void getOnMovie(TransMessage transMessage) {

    }

    private void getTicket(TransMessage transMessage) {

    }
}