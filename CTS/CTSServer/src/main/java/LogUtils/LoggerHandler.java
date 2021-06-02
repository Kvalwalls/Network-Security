package LogUtils;

import DataUtils.User;
import TransmissionUtils.AddressPhaser;
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

    public LoggerHandler(String logName, String logFilename) throws IOException {
        this.logger = Logger.getLogger(logName);
        logger.setLevel(Level.ALL);
        logger.addHandler(new FileHandler(logFilename));
    }

    public void logConnection(Socket socket) {
        if (socket.isConnected()) {
            logger.info("\n客户端与服务器建立连接成功\n" +
                    "客户端IP：" + socket.getRemoteSocketAddress() + "  连接端口：" + socket.getPort() + "\n" +
                    "服务器IP：" + socket.getLocalSocketAddress() + "  连接端口：" + socket.getLocalPort() + "\n");
            //logger.info("");
        } else {
            logger.info("客户端与服务器建立连接失败\n" +
                    "客户端IP：" + socket.getRemoteSocketAddress() + "  连接端口：" + socket.getPort() + "\n" +
                    "服务器IP：" + socket.getLocalSocketAddress() + "  连接端口：" + socket.getLocalPort() + "\n");
            //logger.info("");
        }
    }

    public void logLogin(User user, Socket socket) {
        logger.info("\n用户登录系统\n" +
                "用户号：" + user.getUId() + "\n" +
                "用户名：" + user.getUName() + "\n" +
                "用户登录IP：" + socket.getRemoteSocketAddress() + "  连接端口：" + socket.getPort() + "\n");
    }

    public void logLogout(User user, Socket socket) {
        logger.info("\n用户登出系统\n" +
                "用户号：" + user.getUId() + "\n" +
                "用户名：" + user.getUName() + "\n" +
                "用户登出IP：" + socket.getRemoteSocketAddress() + "  连接端口：" + socket.getPort() + "\n");
    }

    public void logTransMessage(TransMessage transMessage) {
        logger.info("\n报文日志\n" +
                "源IP地址：" + AddressPhaser.bytesToString(transMessage.getFromAddress()) + "\n" +
                "目的IP地址：" + AddressPhaser.bytesToString(transMessage.getToAddress()) + "\n" +
                "服务类型：" + transMessage.getServiceType() + "\n" +
                "具体类型：" + transMessage.getSpecificType() + "\n" +
                "加密码：" + transMessage.getCryptCode() + "\n" +
                "错误码：" + transMessage.getErrorCode() + "\n" +
                "数字签名：" + transMessage.getSignature() + "\n" +
                "报文内容：" + transMessage.getContents() + "\n");
    }

    public void logSqlCommand(String sqlCommand) {
        logger.info("\n数据库操作\n" +
                "SQL语句：" + sqlCommand + "\n");
    }

    public void logException(Exception exception) {
        logger.severe("\n系统异常\n" +
                "异常名称：" + exception.toString() + "\n" +
                "异常内容：" + exception.getMessage() + "\n" +
                "异常原因：" + exception.getCause() + "\n");
    }
}
