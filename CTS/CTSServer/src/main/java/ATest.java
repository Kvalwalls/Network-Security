import DataBaseUtils.DBCommand;
import DataUtils.OnMovie;
import TransmissionUtils.*;

import java.net.ServerSocket;
import java.net.Socket;
import java.util.Date;

public class ATest {
    public static void main(String[] args) {
        try {

            OnMovie onMovie = new OnMovie();
            onMovie.setOId("O00001");
            onMovie.setTId("T00001");
            onMovie.setMId("M00001");
            onMovie.setOBegin(DatePhaser.addDateMinutes(new Date(),480));
            onMovie.setOPrice(50);
            DBCommand.insertOnMovie(onMovie);
        } catch (Exception exception) {
            exception.printStackTrace();
        }
    }
}