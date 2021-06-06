package TransmissionUtils;

import java.io.*;
import java.util.Base64;

public class PicturePhaser {
    /**
     * 图片转换Base64方法
     *
     * @param picName 图片名称
     * @return Base64格式字符串
     * @throws Exception IO异常
     */
    public static String pictureToBase64(String picName) throws Exception {
        InputStream inputStream = new FileInputStream(picName);
        byte[] data = new byte[inputStream.available()];
        inputStream.read(data);
        inputStream.close();
        Base64.Encoder encoder = Base64.getEncoder();
        return encoder.encodeToString(data);
    }

    /**
     * Base64转换图片方法
     *
     * @param base64Str Base64格式字符串
     * @param picName   图片名
     * @throws Exception IO异常
     */
    public static void base64ToPicture(String base64Str, String picName) throws Exception {
        OutputStream outputStream = new FileOutputStream(picName);
        Base64.Decoder decoder = Base64.getDecoder();
        byte[] picBytes = decoder.decode(base64Str.getBytes());
        outputStream.write(picBytes, 0, picBytes.length);
        outputStream.close();
    }
}