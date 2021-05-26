package LogUtils;

import DataUtils.User;
import TransmissionUtils.TransMessage;

import java.io.IOException;
import java.net.Socket;
import java.util.logging.ConsoleHandler;
import java.util.logging.FileHandler;
import java.util.logging.Level;
import java.util.logging.Logger;

public class LoggerHandler {
    private Logger logger;

    public LoggerHandler(String logName) {
        this.logger = Logger.getLogger(logName);
        logger.setLevel(Level.ALL);
        logger.addHandler(new ConsoleHandler());
    }

    public LoggerHandler(String logName,String logFilename) throws IOException {
        this.logger = Logger.getLogger(logName);
        logger.setLevel(Level.ALL);
        logger.addHandler(new FileHandler(logFilename));
    }

    public void logConnection(Socket socket) {

    }

    public void logLogin(User user,Socket socket) {

    }

    public void logLogout(User user,Socket socket) {

    }

    public void logTransMessage(TransMessage transMessage) {

    }

    public void logSqlCommand(String sqlCommand) {

    }

    public void logException(Exception exception) {

    }
}
