package DataBaseUtils;

import DataUtils.*;


import java.sql.*;
import java.text.DateFormat;
import java.text.ParseException;
import java.text.ParsePosition;
import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.List;

public class DBCommand {
    private static String JDBC_DRIVER;      //SQL数据库引擎
    private static String DB_URL;           //数据源
    private static String Name;             //用户名
    private static String Pwd;              //密码
    private static Connection conn;

    static {
        JDBC_DRIVER = "com.mysql.cj.jdbc.Driver";
        DB_URL = "jdbc:mysql://localhost:3306/test?useSSL=false&allowPublicKeyRetrieval=true&serverTimezone=Asia/Shanghai";
        Name = "root";
        Pwd = "dx123";
        try{
            Class.forName(JDBC_DRIVER);
            conn = DriverManager.getConnection(DB_URL, Name, Pwd);
            System.out.println("连接数据库成功");
        }catch(Exception e){
            e.printStackTrace();
            System.out.println("连接失败");
        }
    }
    /*-------------------------------------------------------User---------------------------------------------------*/

    /*
    select * from t_user;
    参数：无
    返回值：所有User实体
     */
    public static ArrayList<User> getAllUsers(){
        String sql = "select * from t_user";
        Statement state = null;
        ResultSet rs;
        ArrayList<DataUtils.User> users=new ArrayList<User>();
        try {
            state = conn.createStatement();
            rs = state.executeQuery(sql);
            while (rs.next()) {
                DataUtils.User f=new DataUtils.User();
                f.setId(rs.getString("u_id"));
                f.setName(rs.getString("u_name"));
                f.setPassword(rs.getString("u_password"));
                f.setAccess(rs.getString("u_access"));
                f.setMoney(Float.parseFloat(rs.getString("u_money")));
                users.add(f);
            }
        } catch (Exception e) {
            e.printStackTrace();
        }

        try {
            assert state != null;
            state.close();
        } catch (SQLException e) {
            // TODO Auto-generated catch block
            e.printStackTrace();
        }
        return users;       //返回用户
    }


    /*
    select * from t_user where u_access=#{arg0};
    参数：用户权限
    返回值：所有符合要求的User实体
     */
    public static ArrayList<DataUtils.User> getAllUsersByAccess(String access){
        String sql = "select * from t_user where u_access ='" + access + "'";
        Statement state = null;
        ResultSet rs;
        ArrayList<DataUtils.User> users=new ArrayList<User>();
        try {
            state = conn.createStatement();
            rs = state.executeQuery(sql);
            while (rs.next()) {
                DataUtils.User f=new DataUtils.User();
                f.setId(rs.getString("u_id"));
                f.setName(rs.getString("u_name"));
                f.setPassword(rs.getString("u_password"));
                f.setAccess(rs.getString("u_access"));
                f.setMoney(Float.parseFloat(rs.getString("u_money")));
                users.add(f);
            }
        } catch (Exception e) {
            e.printStackTrace();
        }

        try {
            assert state != null;
            state.close();
        } catch (SQLException e) {
            // TODO Auto-generated catch block
            e.printStackTrace();
        }
        if(users.size()==0)
            return null;        //数据库中无指定商店，获取菜单失败
        else
            return users;       //返回菜单
    }

    /*
    select * from t_user where u_name=#{name};
    参数：用户名
    返回值：所有符合要求的User实体
     */
    public static ArrayList<DataUtils.User> getAllUsersByNames(String name){
        String sql = "select * from t_user where u_name ='" + name + "'";
        Statement state = null;
        ResultSet rs;
        ArrayList<DataUtils.User> users=new ArrayList<User>();
        try {
            state = conn.createStatement();
            rs = state.executeQuery(sql);
            while (rs.next()) {
                DataUtils.User f=new DataUtils.User();
                f.setId(rs.getString("u_id"));
                f.setName(rs.getString("u_name"));
                f.setPassword(rs.getString("u_password"));
                f.setAccess(rs.getString("u_access"));
                f.setMoney(Float.parseFloat(rs.getString("u_money")));
                users.add(f);
            }
        } catch (Exception e) {
            e.printStackTrace();
        }

        try {
            assert state != null;
            state.close();
        } catch (SQLException e) {
            // TODO Auto-generated catch block
            e.printStackTrace();
        }
        if(users.size()==0)
            return null;        //数据库中无指定商店，获取菜单失败
        else
            return users;
    }

    /*
    select * from t_user where u_id=#{id};
    参数：用户ID
    返回值：所有符合要求的User实体
     */
    public static ArrayList<DataUtils.User> getUserById(String id){
        String sql = "select * from t_user where u_id ='" + id + "'";
        Statement state = null;
        ResultSet rs;
        ArrayList<DataUtils.User> users=new ArrayList<User>();
        try {
            state = conn.createStatement();
            rs = state.executeQuery(sql);
            while (rs.next()) {
                DataUtils.User f=new DataUtils.User();
                f.setId(rs.getString("u_id"));
                f.setName(rs.getString("u_name"));
                f.setPassword(rs.getString("u_password"));
                f.setAccess(rs.getString("u_access"));
                f.setMoney(Float.parseFloat(rs.getString("u_money")));
                users.add(f);
            }
        } catch (Exception e) {
            e.printStackTrace();
        }

        try {
            assert state != null;
            state.close();
        } catch (SQLException e) {
            // TODO Auto-generated catch block
            e.printStackTrace();
        }
        if(users.size()==0)
            return null;
        else
            return users;
    }

    /*
    update t_user set u_name=#{name} where u_id=#{id};
    参数：id——待修改的用户ID，name——指定用户的新用户名
    返回值：修改成功——true，修改失败——false
     */
    public static boolean updateUID(String name ,String id){
        String sql = "update t_user set u_name='" + name +  "'"+" where u_id='" + id+"'";
        //创建数据库链接
        Statement state = null;
        int a = 0;

        try {
            state = conn.createStatement();
            a = state.executeUpdate(sql);
        } catch (Exception e) {
            e.printStackTrace();
        }

        try {
            state.close();
        } catch (SQLException e) {
            // TODO Auto-generated catch block
            e.printStackTrace();
        }
        if(a==0)return false;
        else return true;
    }

    /*
    update t_user set u_password=#{password} where u_id=#{id};
    参数：id——待修改的用户ID，password——指定用户的新密码
    返回值：修改成功——true，修改失败——false
     */
    public static boolean updatePassword(String id,String password){
        String sql = "update t_user set u_password='" + password +  "'"+" where u_id='" + id+"'";
        //创建数据库链接
        Statement state = null;
        int a = 0;

        try {
            state = conn.createStatement();
            a = state.executeUpdate(sql);
        } catch (Exception e) {
            e.printStackTrace();
        }

        try {
            state.close();
        } catch (SQLException e) {
            // TODO Auto-generated catch block
            e.printStackTrace();
        }
        if(a==0)return false;
        else return true;

    }

    /*
    update t_user set u_access=#{access} where u_id=#{id};
    参数：id——待修改的用户ID，access——指定用户的新权限
    返回值：修改成功——true，修改失败——false
     */
    public static boolean updateAccess(String id,String access){
        String sql = "update t_user set u_access='" + access +  "'"+" where u_id='" + id+"'";
        //创建数据库链接
        Statement state = null;
        int a = 0;

        try {
            state = conn.createStatement();
            a = state.executeUpdate(sql);
        } catch (Exception e) {
            e.printStackTrace();
        }

        try {
            state.close();
        } catch (SQLException e) {
            // TODO Auto-generated catch block
            e.printStackTrace();
        }
        if(a==0)return false;
        else return true;
    }

    /*
    根据用户表中的用户ID删除指定user
    delete from t_user where u_id=#{id};
    参数：id——待删除的用户ID
    返回值：删除成功——true，删除失败——false
     */
    public static boolean deleteUser(String id){
        String sql = "delete from t_user where u_id='" +id+ "'";
        //String sql = "delete from course where id='" + id + "'";
        //创建数据库链接
        //Connection conn = DBUtil.getConnection();
        Statement state = null;
        int a = 0;

        try {
            state = conn.createStatement();
            a = state.executeUpdate(sql);
        } catch (Exception e) {
            e.printStackTrace();
        }

        try {
            state.close();
        } catch (SQLException e) {
            // TODO Auto-generated catch block
            e.printStackTrace();
        }
        if(a==0)return false;
        else return true;
    }


    /*-------------------------------------------------------Movie---------------------------------------------------*/

    /*
    获取所有Movie实体
    select * from t_Movie
    参数：无
    返回值：Movie数组
     */
    public static ArrayList<Movie> getAllMovies(){
        String sql = "select * from t_movie";
        Statement state = null;
        ResultSet rs;
        ArrayList<Movie> users=new ArrayList<Movie>();
        try {
            state = conn.createStatement();
            rs = state.executeQuery(sql);
            while (rs.next()) {
                Movie f=new Movie();
                f.setId(rs.getString("m_id"));
                f.setName(rs.getString("m_name"));
                f.setType(rs.getString("m_type"));
                f.setTime(Integer.parseInt(rs.getString("m_time")));
                f.setScore(Float.parseFloat(rs.getString("m_comment")));
                f.setImage(rs.getString("m_picture"));
                f.setDescription(rs.getString("m_description"));
                users.add(f);
            }
        } catch (Exception e) {
            e.printStackTrace();
        }

        try {
            assert state != null;
            state.close();
        } catch (SQLException e) {
            // TODO Auto-generated catch block
            e.printStackTrace();
        }
        if(users.size()==0)
            return null;        //数据库中无指定商店，获取菜单失败
        else
            return users;       //返回菜单
    }

    /*
    通过电影名获取所有Movie实体
    select * from t_Movie where m_name=#{arg0};
    参数：name——电影名
    返回值：Movie数组
     */
    public static ArrayList<Movie> getAllMoviesByName(String name){
        String sql = "select * from t_Movie where m_name ='" + name + "'";
        Statement state = null;
        ResultSet rs;
        ArrayList<Movie> users=new ArrayList<Movie>();
        try {
            state = conn.createStatement();
            rs = state.executeQuery(sql);
            while (rs.next()) {
                Movie f=new Movie();
                f.setId(rs.getString("m_id"));
                f.setName(rs.getString("m_name"));
                f.setType(rs.getString("m_type"));
                f.setTime(Integer.parseInt(rs.getString("m_time")));
                f.setScore(Float.parseFloat(rs.getString("m_comment")));
                f.setImage(rs.getString("m_picture"));
                f.setDescription(rs.getString("m_description"));
                users.add(f);
            }
        } catch (Exception e) {
            e.printStackTrace();
        }

        try {
            assert state != null;
            state.close();
        } catch (SQLException e) {
            // TODO Auto-generated catch block
            e.printStackTrace();
        }
        if(users.size()==0)
            return null;
        else
            return users;
    }

    /*
    通过电影ID获取所有Movie实体
    select * from t_movie where m_id=#{arg0};
    参数：id——电影ID
    返回值：Movie数组
     */
    public static ArrayList<Movie> getAllMoviesById(String id){
        String sql = "select * from t_movie where m_id ='" + id + "'";
        Statement state = null;
        ResultSet rs;
        ArrayList<Movie> users=new ArrayList<Movie>();
        try {
            state = conn.createStatement();
            rs = state.executeQuery(sql);
            while (rs.next()) {
                Movie f=new Movie();
                f.setId(rs.getString("m_id"));
                f.setName(rs.getString("m_name"));
                f.setType(rs.getString("m_type"));
                f.setTime(Integer.parseInt(rs.getString("m_time")));
                f.setScore(Float.parseFloat(rs.getString("m_comment")));
                f.setImage(rs.getString("m_picture"));
                f.setDescription(rs.getString("m_description"));
                users.add(f);
            }
        } catch (Exception e) {
            e.printStackTrace();
        }

        try {
            assert state != null;
            state.close();
        } catch (SQLException e) {
            // TODO Auto-generated catch block
            e.printStackTrace();
        }
        if(users.size()==0)
            return null;
        else
            return users;
    }

    /*
    添加电影
    insert into t_movie values(#{M_id},#{M_name},#{M_type},#{M_time},#{M_comment},#{M_picture},#{M_description})
    参数：待添加电影实体
    返回值：添加成功——true，添加失败——false
     */
    public static boolean AddMovie(Movie movie){
        String sql = "insert into t_movie values('" + movie.getId() + "','" + movie.getName()+ "','" + movie.getType()+ "','" + movie.getTime()+ "','" + movie.getScore()+ "','" + movie.getImage()+ "','" + movie.getDescription()+ "')";
        //创建数据库链接
        Statement state = null;
        int a = 0;

        try {
            state = conn.createStatement();
            a = state.executeUpdate(sql);
        } catch (Exception e) {
            e.printStackTrace();
        }

        try {
            state.close();
        } catch (SQLException e) {
            // TODO Auto-generated catch block
            e.printStackTrace();
        }

        if(a==0)return false;
        else return true;
    }

    /*
    根据电影ID删除指定电影
    delete from t_movie where m_id=#{arg0};
    参数：id——待删除电影ID
    返回值：删除成功——true，删除失败——false
     */
    public static boolean deleteMovieById(String id){
        String sql = "delete from t_movie where m_id='" +id+ "'";
        //String sql = "delete from course where id='" + id + "'";
        //创建数据库链接
        //Connection conn = DBUtil.getConnection();
        Statement state = null;
        int a = 0;

        try {
            state = conn.createStatement();
            a = state.executeUpdate(sql);
        } catch (Exception e) {
            e.printStackTrace();
        }

        try {
            state.close();
        } catch (SQLException e) {
            // TODO Auto-generated catch block
            e.printStackTrace();
        }
        if(a==0)return false;
        else return true;
    }



    /*-------------------------------------------------------Theater---------------------------------------------------*/

    /*
    获取所有Movie实体
    select * from T_Theater ;
    参数：无
    返回值：theater数组
     */
    public static ArrayList<Theater> getAllTheater(){
        String sql = "select * from T_Theater";
        Statement state = null;
        ResultSet rs;
        ArrayList<Theater> users=new ArrayList<Theater>();
        try {
            state = conn.createStatement();
            rs = state.executeQuery(sql);
            while (rs.next()) {
                Theater f=new Theater();
                f.setId(rs.getString("t_id"));
                f.setType(rs.getString("t_type"));
                f.setSize(Integer.parseInt(rs.getString("t_size")));
                users.add(f);
            }
        } catch (Exception e) {
            e.printStackTrace();
        }

        try {
            assert state != null;
            state.close();
        } catch (SQLException e) {
            // TODO Auto-generated catch block
            e.printStackTrace();
        }
        if(users.size()==0)
            return null;
        else
            return users;
    }

    /*
    通过影厅ID获取所有Theater实体
    select * from T_Theater where t_id=#{arg0};
    参数：id——影厅ID
    返回值：Theater数组
     */
    public static ArrayList<Theater> getAllTheaterByID(String id){
        String sql = "select * from T_Theater where t_id='"+id+"'";
        Statement state = null;
        ResultSet rs;
        ArrayList<Theater> users=new ArrayList<Theater>();
        try {
            state = conn.createStatement();
            rs = state.executeQuery(sql);
            while (rs.next()) {
                Theater f=new Theater();
                f.setId(rs.getString("t_id"));
                f.setType(rs.getString("t_type"));
                f.setSize(Integer.parseInt(rs.getString("t_size")));
                users.add(f);
            }
        } catch (Exception e) {
            e.printStackTrace();
        }

        try {
            assert state != null;
            state.close();
        } catch (SQLException e) {
            // TODO Auto-generated catch block
            e.printStackTrace();
        }
        if(users.size()==0)
            return null;
        else
            return users;
    }

    /*
    通过影厅类型获取所有Movie实体
    select * from T_Theater where t_type=#{arg0};
    参数：type——影厅类型
    返回值：Theater数组
     */
    public static ArrayList<Theater> getAllTheaterByType(String type){
        String sql = "select * from T_Theater where t_type='"+type+"'";
        Statement state = null;
        ResultSet rs;
        ArrayList<Theater> users=new ArrayList<Theater>();
        try {
            state = conn.createStatement();
            rs = state.executeQuery(sql);
            while (rs.next()) {
                Theater f=new Theater();
                f.setId(rs.getString("t_id"));
                f.setType(rs.getString("t_type"));
                f.setSize(Integer.parseInt(rs.getString("t_size")));
                users.add(f);
            }
        } catch (Exception e) {
            e.printStackTrace();
        }

        try {
            assert state != null;
            state.close();
        } catch (SQLException e) {
            // TODO Auto-generated catch block
            e.printStackTrace();
        }
        if(users.size()==0)
            return null;
        else
            return users;
    }

    /*
   添加电影
   insert into t_theater values(#{T_id},#{T_type},#{T_size});
   参数：待添加影厅实体
   返回值：添加成功——true，添加失败——false
    */
    public static boolean addTheater(Theater theater){
        if(theater.getSize()%16!=0)return false;
        String sql = "insert into t_theater values('" + theater.getId() + "','" + theater.getType()+ "','" + theater.getSize()+ "')";
        //创建数据库链接
        Statement state = null;
        int a = 0;

        try {
            state = conn.createStatement();
            a = state.executeUpdate(sql);
        } catch (Exception e) {
            e.printStackTrace();
        }

        try {
            state.close();
        } catch (SQLException e) {
            // TODO Auto-generated catch block
            e.printStackTrace();
        }

        if(a==0)return false;
        else return true;
    }

    /*
    根据电影ID删除指定电影
    delete from t_theater where t_id=#{arg0};
    参数：id——待删除影厅ID
    返回值：删除成功——true，删除失败——false
     */
    public static boolean deleteTheater(String id){
        String sql = "delete from t_theater where t_id='" +id+ "'";
        //String sql = "delete from course where id='" + id + "'";
        //创建数据库链接
        //Connection conn = DBUtil.getConnection();
        Statement state = null;
        int a = 0;

        try {
            state = conn.createStatement();
            a = state.executeUpdate(sql);
        } catch (Exception e) {
            e.printStackTrace();
        }

        try {
            state.close();
        } catch (SQLException e) {
            // TODO Auto-generated catch block
            e.printStackTrace();
        }
        if(a==0)return false;
        else return true;
    }

    /*-------------------------------------------------------OnMovie---------------------------------------------------*/

    public static java.util.Date strToDateLong(String strDate) {
        SimpleDateFormat formatter = new SimpleDateFormat("yyyy-MM-dd HH:mm:ss");
        ParsePosition pos = new ParsePosition(0);
        java.util.Date strtodate = formatter.parse(strDate, pos);
        return strtodate;
    }
    public static String dateToString(java.util.Date date) {
        SimpleDateFormat sformat = new SimpleDateFormat("yyyy-MM-dd HH:mm:ss");//日期格式
        String tiem = sformat.format(date);
        return tiem;
    }
    /*
    获取所有场次实体
    select * from T_OnMovie;
    参数：无
    返回值：场次动态数组
     */
    public static ArrayList<OnMovie> getAllOnMovies(){
        String sql = "select * from T_OnMovie";
        Statement state = null;
        ResultSet rs;
        ArrayList<OnMovie> users=new ArrayList<OnMovie>();
        try {
            state = conn.createStatement();
            rs = state.executeQuery(sql);
            while (rs.next()) {
                OnMovie f=new OnMovie();
                f.setOid(rs.getString("o_id"));
                f.setMid(rs.getString("m_id"));
                f.setTid(rs.getString("t_id"));
                f.setStartTime(strToDateLong(rs.getString("o_begintime")));
                f.setEndTime(strToDateLong(rs.getString("o_endtime")));
                f.setPrice(Float.parseFloat(rs.getString("o_price")));
                users.add(f);
            }
        } catch (Exception e) {
            e.printStackTrace();
        }

        try {
            assert state != null;
            state.close();
        } catch (SQLException e) {
            // TODO Auto-generated catch block
            e.printStackTrace();
        }
        if(users.size()==0)
            return null;
        else
            return users;
    }

    /*
    根据场次号获取所有场次实体
    select * from T_OnMovie where o_id=#{arg0};
    参数：oid——场次号
    返回值：场次动态数组
     */
    public static ArrayList<OnMovie> getAllOnMoviesByOnId(String oid){
        String sql = "select * from T_OnMovie where o_id='"+oid+"'";
        Statement state = null;
        ResultSet rs;
        ArrayList<OnMovie> users=new ArrayList<OnMovie>();
        try {
            state = conn.createStatement();
            rs = state.executeQuery(sql);
            while (rs.next()) {
                OnMovie f=new OnMovie();
                f.setOid(rs.getString("o_id"));
                f.setMid(rs.getString("m_id"));
                f.setTid(rs.getString("t_id"));
                f.setStartTime(strToDateLong(rs.getString("o_begintime")));
                f.setEndTime(strToDateLong(rs.getString("o_endtime")));
                f.setPrice(Float.parseFloat(rs.getString("o_price")));
                users.add(f);
            }
        } catch (Exception e) {
            e.printStackTrace();
        }

        try {
            assert state != null;
            state.close();
        } catch (SQLException e) {
            // TODO Auto-generated catch block
            e.printStackTrace();
        }
        if(users.size()==0)
            return null;
        else
            return users;
    }

    /*
    根据影片号获取所有场次实体
    select * from T_OnMovie where m_id=#{arg0};
    参数：mid——影片号
    返回值：场次动态数组
     */
    public static ArrayList<OnMovie> getAllOnMoviesByMovieId(String mid){
        String sql = "select * from T_OnMovie where m_id='"+mid+"'";
        Statement state = null;
        ResultSet rs;
        ArrayList<OnMovie> users=new ArrayList<OnMovie>();
        try {
            state = conn.createStatement();
            rs = state.executeQuery(sql);
            while (rs.next()) {
                OnMovie f=new OnMovie();
                f.setOid(rs.getString("o_id"));
                f.setMid(rs.getString("m_id"));
                f.setTid(rs.getString("t_id"));
                f.setStartTime(strToDateLong(rs.getString("o_begintime")));
                f.setEndTime(strToDateLong(rs.getString("o_endtime")));
                f.setPrice(Float.parseFloat(rs.getString("o_price")));
                users.add(f);
            }
        } catch (Exception e) {
            e.printStackTrace();
        }

        try {
            assert state != null;
            state.close();
        } catch (SQLException e) {
            // TODO Auto-generated catch block
            e.printStackTrace();
        }
        if(users.size()==0)
            return null;
        else
            return users;
    }

    /*
    根据影厅号获取所有场次实体
    select * from T_OnMovie where t_id=#{arg0};
    参数：tid——影厅号
    返回值：场次动态数组
     */
    public static ArrayList<OnMovie> getAllOnMoviesByTheaterId(String tid){
        String sql = "select * from T_OnMovie where t_id='"+tid+"'";
        Statement state = null;
        ResultSet rs;
        ArrayList<OnMovie> users=new ArrayList<OnMovie>();
        try {
            state = conn.createStatement();
            rs = state.executeQuery(sql);
            while (rs.next()) {
                OnMovie f=new OnMovie();
                f.setOid(rs.getString("o_id"));
                f.setMid(rs.getString("m_id"));
                f.setTid(rs.getString("t_id"));
                f.setStartTime(strToDateLong(rs.getString("o_begintime")));
                f.setEndTime(strToDateLong(rs.getString("o_endtime")));
                f.setPrice(Float.parseFloat(rs.getString("o_price")));
                users.add(f);
            }
        } catch (Exception e) {
            e.printStackTrace();
        }

        try {
            assert state != null;
            state.close();
        } catch (SQLException e) {
            // TODO Auto-generated catch block
            e.printStackTrace();
        }
        if(users.size()==0)
            return null;
        else
            return users;
    }

    /*
    根据开始时间获取所有场次实体
    select * from T_OnMovie where o_begintime=#{arg0};
    参数：begintime——开始时间
    返回值：场次动态数组
     */
    public static ArrayList<OnMovie> getAllOnMoviesByBeginTime(String begintime){
        String sql = "select * from T_OnMovie where o_begintime='"+begintime+"'";
        Statement state = null;
        ResultSet rs;
        ArrayList<OnMovie> users=new ArrayList<OnMovie>();
        try {
            state = conn.createStatement();
            rs = state.executeQuery(sql);
            while (rs.next()) {
                OnMovie f=new OnMovie();
                f.setOid(rs.getString("o_id"));
                f.setMid(rs.getString("m_id"));
                f.setTid(rs.getString("t_id"));
                f.setStartTime(strToDateLong(rs.getString("o_begintime")));
                f.setEndTime(strToDateLong(rs.getString("o_endtime")));
                f.setPrice(Float.parseFloat(rs.getString("o_price")));
                users.add(f);
            }
        } catch (Exception e) {
            e.printStackTrace();
        }

        try {
            assert state != null;
            state.close();
        } catch (SQLException e) {
            // TODO Auto-generated catch block
            e.printStackTrace();
        }
        if(users.size()==0)
            return null;
        else
            return users;
    }

    /*
    如果data1=data2  return 0;
    如果data1>data2  return 1;
    如果data1<data2  return -1;
     */
    public static int CompareTime(java.util.Date data1,java.util.Date data2) throws ParseException {
        String DateStr1 = dateToString(data1);
        String DateStr2 = dateToString(data2);
        DateFormat dateFormat = new SimpleDateFormat("yyyy-MM-dd HH:mm:ss");
        java.util.Date dateTime1 = dateFormat.parse(DateStr1);
        java.util.Date dateTime2 = dateFormat.parse(DateStr2);
        int i = dateTime1.compareTo(dateTime2);
        return i;
    }



    /*
    根据OnMovie实体添加场次信息
    insert into t_onmovie values(#{O_id},#{M_id},#{T_id},#{O_beginTime},#{O_endTime},#{O_price});
    参数：onMovie——场次实体
    返回值：true——添加成功，false——添加失败
     */
    public static boolean addOnMovie(OnMovie onMovie) throws ParseException {
        List<OnMovie> t=getAllOnMoviesByTheaterId(onMovie.getTid());
        //判断是否有时间冲突
        for(OnMovie onMovie1:t){
            if(!((CompareTime(onMovie.getStartTime(),onMovie1.getEndTime())==1)&&(CompareTime(onMovie.getEndTime(),onMovie1.getStartTime())==-1))) {
                System.out.println("addOnMovie failed: Time conflict!");
                return false;
            }
        }

        //若没有时间冲突，根据外键约束先添加场次，然后添加座位
        String sql = "insert into t_onmovie values('" + onMovie.getOid() + "','" + onMovie.getMid()+ "','" + onMovie.getTid()+ "','" + dateToString(onMovie.getStartTime())+ "','" + dateToString(onMovie.getEndTime())+ "','" + onMovie.getPrice() + "')";
        //创建数据库链接
        Statement state = null;
        int a = 0;

        try {
            state = conn.createStatement();
            a = state.executeUpdate(sql);
        } catch (Exception e) {
            e.printStackTrace();
        }

        try {
            state.close();
        } catch (SQLException e) {
            // TODO Auto-generated catch block
            e.printStackTrace();
        }

        //添加座位
        int size=getAllTheaterByID(onMovie.getTid()).get(0).getSize();
        int row=size/16;
        for(int i=1;i<=row;i++){
            for(int j=1;j<=16;j++){
                addSeat(new Seat(getString(i)+getString(j),onMovie.getOid(),"0"));
            }
        }

        if(a==0)return false;
        else return true;
    }
    public static String  getString(int i){
        if(i>0&&i<10){
            return "0"+i;
        }
        if(i>=10&&i<=99){
            return Integer.toString(i);
        }
        return null;
    }

    /*
    根据场次号删除指定电影
    delete from t_onmovie where o_id=#{arg0};
    参数：oid——待删除场次号
    返回值：删除成功——true，删除失败——false
     */
    public static boolean deleteOnMovie(String oid){
        String sql = "delete from t_onmovie where o_id='" +oid+ "'";
        //String sql = "delete from course where id='" + id + "'";
        //创建数据库链接
        //Connection conn = DBUtil.getConnection();
        Statement state = null;
        int a = 0;

        try {
            state = conn.createStatement();
            a = state.executeUpdate(sql);
        } catch (Exception e) {
            e.printStackTrace();
        }

        try {
            state.close();
        } catch (SQLException e) {
            // TODO Auto-generated catch block
            e.printStackTrace();
        }
        if(a==0)return false;
        else return true;
    }


    /*-------------------------------------------------------Seat---------------------------------------------------*/

    /*
    获取所有座位实体
    select * from T_Seat;
    参数：无
    返回值：座位动态数组
     */
    public static ArrayList<Seat> getAllSeat(){
        String sql = "select * from T_Seat";
        Statement state = null;
        ResultSet rs;
        ArrayList<Seat> users=new ArrayList<Seat>();
        try {
            state = conn.createStatement();
            rs = state.executeQuery(sql);
            while (rs.next()) {
                Seat f=new Seat();
                f.setOid(rs.getString("o_id"));
                f.setSid(rs.getString("s_id"));
                f.setStatus(rs.getString("s_status"));
                users.add(f);
            }
        } catch (Exception e) {
            e.printStackTrace();
        }

        try {
            assert state != null;
            state.close();
        } catch (SQLException e) {
            // TODO Auto-generated catch block
            e.printStackTrace();
        }
        if(users.size()==0)
            return null;
        else
            return users;
    }

    /*
    根据座位号获取所有座位实体
    select * from T_Seat where s_id=#{arg0};
    参数：sid——座位号
    返回值：座位动态数组
     */
    public static ArrayList<Seat> getAllSeatBySid(String sid){
        String sql = "select * from T_Seat where s_id='"+sid+"'";
        Statement state = null;
        ResultSet rs;
        ArrayList<Seat> users=new ArrayList<Seat>();
        try {
            state = conn.createStatement();
            rs = state.executeQuery(sql);
            while (rs.next()) {
                Seat f=new Seat();
                f.setOid(rs.getString("o_id"));
                f.setSid(rs.getString("s_id"));
                f.setStatus(rs.getString("s_status"));
                users.add(f);
            }
        } catch (Exception e) {
            e.printStackTrace();
        }

        try {
            assert state != null;
            state.close();
        } catch (SQLException e) {
            // TODO Auto-generated catch block
            e.printStackTrace();
        }
        if(users.size()==0)
            return null;
        else
            return users;
    }

    /*
    根据场次号获取所有座位实体
    select * from T_Seat where o_id=#{arg0};
    参数：oid——场次号
    返回值：座位动态数组
     */
    public static ArrayList<Seat> getAllSeatByOid(String oid){
        String sql = "select * from T_Seat where o_id='"+oid+"'";
        Statement state = null;
        ResultSet rs;
        ArrayList<Seat> users=new ArrayList<Seat>();
        try {
            state = conn.createStatement();
            rs = state.executeQuery(sql);
            while (rs.next()) {
                Seat f=new Seat();
                f.setOid(rs.getString("o_id"));
                f.setSid(rs.getString("s_id"));
                f.setStatus(rs.getString("s_status"));
                users.add(f);
            }
        } catch (Exception e) {
            e.printStackTrace();
        }

        try {
            assert state != null;
            state.close();
        } catch (SQLException e) {
            // TODO Auto-generated catch block
            e.printStackTrace();
        }
        if(users.size()==0)
            return null;
        else
            return users;
    }

    /*
    根据Seat实体添加场次信息
    insert into T_Seat values(#{O_id},#{S_id},#{S_status})
    参数：seat——场次实体
    返回值：true——添加成功，false——添加失败
     */
    private static boolean addSeat(Seat seat){
        String sql = "insert into T_Seat values('" + seat.getOid() + "','" + seat.getSid()+ "','" + seat.getStatus()+ "')";
        //创建数据库链接
        Statement state = null;
        int a = 0;

        try {
            state = conn.createStatement();
            a = state.executeUpdate(sql);
        } catch (Exception e) {
            e.printStackTrace();
        }

        try {
            state.close();
        } catch (SQLException e) {
            // TODO Auto-generated catch block
            e.printStackTrace();
        }

        if(a==0)return false;
        else return true;
    }

    /*-------------------------------------------------------Record---------------------------------------------------*/
    /*
    获取所有购票信息
    select * from T_Record;
    参数：无
    返回值：Record动态数组
     */
    public static ArrayList<DataUtils.Record> getAllRecord(){
        String sql = "select * from T_Record";
        Statement state = null;
        ResultSet rs;
        ArrayList<DataUtils.Record> users=new ArrayList<DataUtils.Record>();
        try {
            state = conn.createStatement();
            rs = state.executeQuery(sql);
            while (rs.next()) {
                DataUtils.Record f=new DataUtils.Record();
                f.setUid(rs.getString("u_id"));
                f.setOid(rs.getString("o_id"));
                f.setSid(rs.getString("s_id"));
                f.setTime(strToDateLong(rs.getString("r_time")));
                f.setPrice(Float.parseFloat(rs.getString("r_price")));
                f.setStatus(rs.getString("r_status"));
                users.add(f);
            }
        } catch (Exception e) {
            e.printStackTrace();
        }

        try {
            assert state != null;
            state.close();
        } catch (SQLException e) {
            // TODO Auto-generated catch block
            e.printStackTrace();
        }
        if(users.size()==0)
            return null;
        else
            return users;
    }

    /*
    根据用户号获取购票信息
    select * from T_Record where u_id=#{arg0};
    参数：uid——用户ID
    返回值：Record动态数组
     */
    public static ArrayList<DataUtils.Record> getAllRecordByUid(String uid){
        String sql = "select * from T_Record where u_id='"+uid+"'";
        Statement state = null;
        ResultSet rs;
        ArrayList<DataUtils.Record> users=new ArrayList<DataUtils.Record>();
        try {
            state = conn.createStatement();
            rs = state.executeQuery(sql);
            while (rs.next()) {
                DataUtils.Record f=new DataUtils.Record();
                f.setUid(rs.getString("u_id"));
                f.setOid(rs.getString("o_id"));
                f.setSid(rs.getString("s_id"));
                f.setTime(strToDateLong(rs.getString("r_time")));
                f.setPrice(Float.parseFloat(rs.getString("r_price")));
                f.setStatus(rs.getString("r_status"));
                users.add(f);
            }
        } catch (Exception e) {
            e.printStackTrace();
        }

        try {
            assert state != null;
            state.close();
        } catch (SQLException e) {
            // TODO Auto-generated catch block
            e.printStackTrace();
        }
        if(users.size()==0)
            return null;
        else
            return users;
    }

    /*
    根据场次号获取购票信息
    select * from T_Record where o_id=#{arg0};
    参数：oid——场次号
    返回值：Record动态数组
     */
    public static ArrayList<DataUtils.Record> getAllRecordByOid(String oid){
        String sql = "select * from T_Record where o_id='"+oid+"'";
        Statement state = null;
        ResultSet rs;
        ArrayList<DataUtils.Record> users=new ArrayList<DataUtils.Record>();
        try {
            state = conn.createStatement();
            rs = state.executeQuery(sql);
            while (rs.next()) {
                DataUtils.Record f=new DataUtils.Record();
                f.setUid(rs.getString("u_id"));
                f.setOid(rs.getString("o_id"));
                f.setSid(rs.getString("s_id"));
                f.setTime(strToDateLong(rs.getString("r_time")));
                f.setPrice(Float.parseFloat(rs.getString("r_price")));
                f.setStatus(rs.getString("r_status"));
                users.add(f);
            }
        } catch (Exception e) {
            e.printStackTrace();
        }

        try {
            assert state != null;
            state.close();
        } catch (SQLException e) {
            // TODO Auto-generated catch block
            e.printStackTrace();
        }
        if(users.size()==0)
            return null;
        else
            return users;
    }

    /*
    根据用户号，座位号，场次号获取指定购票信息
    update T_Record set r_status=#{arg3} where u_id=#{arg0} and s_id=#{arg1} and o_id=#{arg2};
    参数：uid——用户号，sid——座位号，oid——场次号
    返回值：true——更新成功，false——更新失败
     */
    public static boolean updateStatus(String U_id,String S_id,String O_id,String newStatus){
        String sql = "update T_Record set r_status = '" + newStatus +  "'"+" where u_id = '" + U_id+"' and s_id = '"+S_id+"' and oid = '"+O_id+"'";
        //创建数据库链接
        Statement state = null;
        int a = 0;

        try {
            state = conn.createStatement();
            a = state.executeUpdate(sql);
        } catch (Exception e) {
            e.printStackTrace();
        }

        try {
            state.close();
        } catch (SQLException e) {
            // TODO Auto-generated catch block
            e.printStackTrace();
        }
        if(a==0)return false;
        else return true;
    }

    /*
    根据Record实体添加购票信息
    insert into T_Record values(#{U_id},#{O_id},#{S_id},#{R_time},#{R_price},#{R_status});
    参数：record——购票实体
    返回值：true——添加成功，false——添加失败
     */
    public static boolean addRecord(DataUtils.Record record){
        String sql = "insert into T_Record values('" + record.getUid() + "','" + record.getOid()+ "','" + record.getSid()+ "','" + dateToString(record.getTime())+ "','" + record.getPrice()+ "','" + record.getStatus() + "')";
        //创建数据库链接
        Statement state = null;
        int a = 0;

        try {
            state = conn.createStatement();
            a = state.executeUpdate(sql);
        } catch (Exception e) {
            e.printStackTrace();
        }

        try {
            state.close();
        } catch (SQLException e) {
            // TODO Auto-generated catch block
            e.printStackTrace();
        }

        if(a==0)return false;
        else return true;
    }


    /*-------------------------------------------------------Idk---------------------------------------------------*/
    /*
    根据指定ID获取key
    select * from T_IDK where id=#{arg0};
    参数：指定IDK的id
    返回值：idk实体
     */
    public static ArrayList<Idk> getIDKById(String id){
        String sql = "select * from T_IDK where id='"+id+"'";
        Statement state = null;
        ResultSet rs;
        ArrayList<Idk> users=new ArrayList<Idk>();
        try {
            state = conn.createStatement();
            rs = state.executeQuery(sql);
            while (rs.next()) {
                Idk f=new Idk();
                f.setId(rs.getString("id"));
                f.setT_key(rs.getString("t_key"));
                users.add(f);
            }
        } catch (Exception e) {
            e.printStackTrace();
        }

        try {
            assert state != null;
            state.close();
        } catch (SQLException e) {
            // TODO Auto-generated catch block
            e.printStackTrace();
        }
        if(users.size()==0)
            return null;
        else
            return users;
    }

    /*
    添加Idk实体
    insert into T_IDK values(#{id},#{t_key});
    参数：Idk实体
    返回值：true——添加成功  false——添加失败
     */
    public static boolean addIDK(Idk idk){
        String sql = "insert into T_IDK(Id,t_key) values('" + idk.getId() + "','" + idk.getT_key() + "')";
        //创建数据库链接
        Statement state = null;
        int a = 0;

        try {
            state = conn.createStatement();
            a = state.executeUpdate(sql);
        } catch (Exception e) {
            e.printStackTrace();
        }

        try {
            state.close();
        } catch (SQLException e) {
            // TODO Auto-generated catch block
            e.printStackTrace();
        }

        if(a==0)return false;
        else return true;
    }

}