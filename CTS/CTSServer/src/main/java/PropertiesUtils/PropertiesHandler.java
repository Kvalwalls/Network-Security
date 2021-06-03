package PropertiesUtils;

import java.io.*;
import java.util.Properties;

public class PropertiesHandler {
    //配置对象
    private static Properties properties;

    //初始化
    static {
        try {
            InputStream in = new FileInputStream("AppConfig.properties");
            properties = new Properties();
            properties.load(in);
            in.close();
        } catch (Exception e) {
            e.printStackTrace();
        }
    }

    /**
     * 获取配置文件元素方法
     *
     * @param tag 配置文件元素名
     * @return 配置文件元素值
     */
    public static String getElement(String tag) {
        return properties.getProperty(tag);
    }
}