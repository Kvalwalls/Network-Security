import DataUtils.*;

import java.sql.*;
import java.util.ArrayList;

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

    public static void main(String[] args) {
        for(DataUtils.User a:getAllUsers()){
            System.out.println(a);
        }
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

    }

    /*
    通过电影名获取所有Movie实体
    select * from t_Movie where m_name=#{arg0};
    参数：name——电影名
    返回值：Movie数组
     */
    public static ArrayList<Movie> getAllMoviesByName(String name){

    }

    /*
    通过电影ID获取所有Movie实体
    select * from t_movie where m_id=#{arg0};
    参数：id——电影ID
    返回值：Movie数组
     */
    public static ArrayList<Movie> getAllMoviesById(String id){

    }

    /*
    添加电影
    insert into t_movie values(#{M_id},#{M_name},#{M_type},#{M_time},#{M_comment},#{M_picture},#{M_description})
    参数：待添加电影实体
    返回值：添加成功——true，添加失败——false
     */
    public static boolean AddMovie(String id){

    }

    /*
    根据电影ID删除指定电影
    delete from t_movie where m_id=#{arg0};
    参数：id——待删除电影ID
    返回值：删除成功——true，删除失败——false
     */
    public static boolean deleteMovieById(String id){

    }



    /*-------------------------------------------------------Theater---------------------------------------------------*/

    /*
    获取所有Movie实体
    select * from T_Theater ;
    参数：无
    返回值：theater数组
     */
    public static ArrayList<Theater> getAllTheater(){

    }

    /*
    通过影厅ID获取所有Theater实体
    select * from T_Theater where t_id=#{arg0};
    参数：id——影厅ID
    返回值：Theater数组
     */
    public static ArrayList<Theater> getAllTheaterByID(String id){

    }

    /*
    通过影厅类型获取所有Movie实体
    select * from T_Theater where t_type=#{arg0};
    参数：type——影厅类型
    返回值：Theater数组
     */
    public static ArrayList<Theater> getAllTheaterByType(String type){

    }

    /*
   添加电影
   insert into t_theater values(#{T_id},#{T_type},#{T_size});
   参数：待添加影厅实体
   返回值：添加成功——true，添加失败——false
    */
    public static boolean addTheater(Theater theater){

    }

    /*
    根据电影ID删除指定电影
    delete from t_theater where t_id=#{arg0};
    参数：id——待删除影厅ID
    返回值：删除成功——true，删除失败——false
     */
    public static boolean deleteTheater(String id){

    }

    /*-------------------------------------------------------OnMovie---------------------------------------------------*/

    /*
    获取所有场次实体
    select * from T_OnMovie;
    参数：无
    返回值：场次动态数组
     */
    public static ArrayList<OnMovie> getAllOnMovies(){

    }

    /*
    根据场次号获取所有场次实体
    select * from T_OnMovie where o_id=#{arg0};
    参数：oid——场次号
    返回值：场次动态数组
     */
    public static ArrayList<OnMovie> getAllOnMoviesByOnId(String oid){

    }

    /*
    根据影片号获取所有场次实体
    select * from T_OnMovie where m_id=#{arg0};
    参数：mid——影片号
    返回值：场次动态数组
     */
    public static ArrayList<OnMovie> getAllOnMoviesByMovieId(String mid){

    }

    /*
    根据影厅号获取所有场次实体
    select * from T_OnMovie where t_id=#{arg0};
    参数：tid——影厅号
    返回值：场次动态数组
     */
    public static ArrayList<OnMovie> getAllOnMoviesByTheaterId(String tid){

    }

    /*
    根据开始时间获取所有场次实体
    select * from T_OnMovie where o_begintime=#{arg0};
    参数：begintime——开始时间
    返回值：场次动态数组
     */
    public static ArrayList<OnMovie> getAllOnMoviesByBeginTime(String begintime){

    }

    /*
    根据OnMovie实体添加场次信息
    insert into t_onmovie values(#{O_id},#{M_id},#{T_id},#{O_beginTime},#{O_endTime},#{O_price});
    参数：onMovie——场次实体
    返回值：true——添加成功，false——添加失败
     */
    public static boolean addOnMovie(OnMovie onMovie){

    }

    /*
    根据场次号删除指定电影
    delete from t_onmovie where o_id=#{arg0};
    参数：oid——待删除场次号
    返回值：删除成功——true，删除失败——false
     */
    public static boolean deleteOnMovie(String oid){

    }


    /*-------------------------------------------------------Seat---------------------------------------------------*/

    /*
    获取所有座位实体
    select * from T_Seat;
    参数：无
    返回值：座位动态数组
     */
    public static ArrayList<Seat> getAllSeat(){

    }

    /*
    根据座位号获取所有座位实体
    select * from T_Seat where s_id=#{arg0};
    参数：sid——座位号
    返回值：座位动态数组
     */
    public static ArrayList<Seat> getAllSeatBySid(String sid){

    }

    /*
    根据场次号获取所有座位实体
    select * from T_Seat where o_id=#{arg0};
    参数：oid——场次号
    返回值：座位动态数组
     */
    public static ArrayList<Seat> getAllSeatByOid(String oid){

    }

    /*
    根据Seat实体添加场次信息
    insert into T_Seat values(#{O_id},#{S_id},#{S_status})
    参数：seat——场次实体
    返回值：true——添加成功，false——添加失败
     */
    private static boolean addSeat(Seat seat){

    }


    /*-------------------------------------------------------Record---------------------------------------------------*/
    /*
    获取所有购票信息
    select * from T_Record;
    参数：无
    返回值：Record动态数组
     */
    public static ArrayList<DataUtils.Record> getAllRecord(){

    }

    /*
    根据用户号获取购票信息
    select * from T_Record where u_id=#{arg0};
    参数：uid——用户ID
    返回值：Record动态数组
     */
    public static ArrayList<DataUtils.Record> getAllRecordByUid(String uid){

    }

    /*
    根据场次号获取购票信息
    select * from T_Record where o_id=#{arg0};
    参数：oid——场次号
    返回值：Record动态数组
     */
    public static ArrayList<DataUtils.Record> getAllRecordByOid(String oid){


    }

    /*
    根据用户号，座位号，场次号获取指定购票信息
    update T_Record set r_status=#{arg3} where u_id=#{arg0} and s_id=#{arg1} and o_id=#{arg2};
    参数：uid——用户号，sid——座位号，oid——场次号
    返回值：true——更新成功，false——更新失败
     */
    public static boolean updateStatus(String U_id,String S_id,String O_id,String newStatus){

    }

    /*
    根据Record实体添加购票信息
    insert into T_Record values(#{U_id},#{O_id},#{S_id},#{R_time},#{R_price},#{R_status});
    参数：record——购票实体
    返回值：true——添加成功，false——添加失败
     */
    public static boolean addRecord(DataUtils.Record record){

    }


    /*-------------------------------------------------------Idk---------------------------------------------------*/
    /*
    根据指定ID获取key
    select * from T_IDK where id=#{arg0};
    参数：指定IDK的id
    返回值：idk实体
     */
    public static ArrayList<Idk> getIDKById(String id){

    }

    /*
    添加Idk实体
    insert into T_IDK values(#{id},#{t_key});
    参数：Idk实体
    返回值：true——添加成功  false——添加失败
     */
    public static boolean addIDK(Idk idk){

    }

}