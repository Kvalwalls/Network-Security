package TransmissionUtils;

import javax.imageio.stream.FileImageInputStream;
import javax.imageio.stream.FileImageOutputStream;
import java.io.*;

public class PicturePhaser {
    //读取图片
    public static byte[] pictureToBytes(String filename) {

        byte[] data = null;
        FileImageInputStream input = null;
        try {
            input = new FileImageInputStream(new File(filename));
            ByteArrayOutputStream output = new ByteArrayOutputStream();
            byte[] buf = new byte[1024];
            int numBytesRead = 0;
            while ((numBytesRead = input.read(buf)) != -1) {
                output.write(buf, 0, numBytesRead);
            }
            data = output.toByteArray();
            output.close();
            input.close();
        }
        catch (FileNotFoundException ex1) {
            ex1.printStackTrace();
        }
        catch (IOException ex1) {
            ex1.printStackTrace();
        }
        return data;

    }
    //字节流写图片文件
    public static void bytesToPicture(byte[] bytes) {
        try{
            FileImageOutputStream imageOutput = new FileImageOutputStream(new File("D:\\1\\M.jpg"));
            imageOutput.write(bytes, 0, bytes.length);
            imageOutput.close();
        } catch(Exception ex) {
            System.out.println("Exception: " + ex);
            ex.printStackTrace();
        }
    }
}
