package TransmissionUtils;

import java.text.ParsePosition;
import java.text.SimpleDateFormat;
import java.util.Calendar;
import java.util.Date;
import java.util.GregorianCalendar;

public class DatePhaser {
    /**
     * Date字符串转换Date对象
     *
     * @param strDate Date字符串
     * @return Date对象
     */
    public static Date dateStrToDate(String strDate) {
        SimpleDateFormat simpleDateFormat = new SimpleDateFormat("yyyy/MM/dd HH:mm:ss");
        ParsePosition pos = new ParsePosition(0);
        Date date = simpleDateFormat.parse(strDate, pos);
        return date;
    }

    /**
     * Date对象转换Date字符串
     *
     * @param date Date对象
     * @return Date字符串
     */
    public static String dateToDateStr(Date date) {
        SimpleDateFormat simpleDateFormat = new SimpleDateFormat("yyyy/MM/dd HH:mm:ss");
        String strDate = simpleDateFormat.format(date);
        return strDate;
    }

    public static Date addDateMinutes(Date date,int minutes) {
        Calendar calendar = new GregorianCalendar();
        calendar.setTime(date);
        calendar.add(Calendar.MINUTE, minutes);
        return calendar.getTime();
    }
}
