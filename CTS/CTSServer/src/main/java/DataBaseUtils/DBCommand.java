package DataBaseUtils;

import DataUtils.Record;
import DataUtils.*;
import EnumUtils.EnumSeatStatus;
import PropertiesUtils.PropertiesHandler;
import TransmissionUtils.DatePhaser;

import java.sql.Connection;
import java.sql.DriverManager;
import java.sql.ResultSet;
import java.sql.Statement;
import java.util.ArrayList;
import java.util.List;

public class DBCommand {
    //数据库连接
    private static Connection connection;

    //初始化
    static {
        try {
            //数据库引擎
            String JDBC_DRIVER = PropertiesHandler.getElement("JDBC_Driver");
            //数据库数据源
            String DB_URL = PropertiesHandler.getElement("DB_URL");
            //数据库用户名
            String USER = PropertiesHandler.getElement("User");
            //数据库密码
            String PWD = PropertiesHandler.getElement("Password");
            Class.forName(JDBC_DRIVER);
            connection = DriverManager.getConnection(DB_URL, USER, PWD);
            System.out.println("数据库连接成功！");
        } catch (Exception e) {
            e.printStackTrace();
            System.out.println("数据库连接失败！");
        }
    }

    /*----人员表T_User接口----*/

    /**
     * 获取所有User实体
     *
     * @return User对象列表
     */
    public static ArrayList<User> getAllUsers() {
        String sql = "select * from t_user";
        Statement state = null;
        ResultSet rs = null;
        ArrayList<User> users = new ArrayList<>();
        try {
            state = connection.createStatement();
            rs = state.executeQuery(sql);
            while (rs.next()) {
                User temp = new User();
                temp.setUId(rs.getString("u_id"));
                temp.setUName(rs.getString("u_name"));
                temp.setUPassword(rs.getString("u_password"));
                temp.setUAccess(Byte.parseByte(rs.getString("u_access")));
                temp.setUMoney(Float.parseFloat(rs.getString("u_money")));
                users.add(temp);
            }
            state.close();
        } catch (Exception e) {
            e.printStackTrace();
        }
        return users;
    }

    /**
     * 根据人员号获取User实体
     *
     * @param id 人员号
     * @return User对象列表
     */
    public static User getUserById(String id) {
        String sql = "select * from t_user where u_id ='" + id + "'";
        Statement state = null;
        ResultSet rs = null;
        User user = null;
        try {
            state = connection.createStatement();
            rs = state.executeQuery(sql);
            if (rs.next()) {
                user = new User();
                user.setUId(rs.getString("u_id"));
                user.setUName(rs.getString("u_name"));
                user.setUPassword(rs.getString("u_password"));
                user.setUAccess(Byte.parseByte(rs.getString("u_access")));
                user.setUMoney(Float.parseFloat(rs.getString("u_money")));
            }
            state.close();
        } catch (Exception e) {
            e.printStackTrace();
        }
        return user;
    }

    /**
     * 通过权限获取User实体
     *
     * @param access 权限
     * @return User对象列表
     */
    public static ArrayList<User> getUsersByAccess(byte access) {
        String sql = "select * from t_user where u_access ='" + Byte.toString(access) + "'";
        Statement state = null;
        ResultSet rs = null;
        ArrayList<User> users = new ArrayList<>();
        try {
            state = connection.createStatement();
            rs = state.executeQuery(sql);
            while (rs.next()) {
                User temp = new User();
                temp.setUId(rs.getString("u_id"));
                temp.setUName(rs.getString("u_name"));
                temp.setUPassword(rs.getString("u_password"));
                temp.setUAccess(Byte.parseByte(rs.getString("u_access")));
                temp.setUMoney(Float.parseFloat(rs.getString("u_money")));
                users.add(temp);
            }
            state.close();
        } catch (Exception e) {
            e.printStackTrace();
        }
        if (users.size() == 0)
            return null;
        else
            return users;
    }

    /**
     * 根据名称获取User实体
     *
     * @param name 名称
     * @return User对象列表
     */
    public static ArrayList<User> getUsersByName(String name) {
        String sql = "select * from t_user where u_name ='" + name + "'";
        Statement state = null;
        ResultSet rs = null;
        ArrayList<User> users = new ArrayList<>();
        try {
            state = connection.createStatement();
            rs = state.executeQuery(sql);
            while (rs.next()) {
                User temp = new User();
                temp.setUId(rs.getString("u_id"));
                temp.setUName(rs.getString("u_name"));
                temp.setUPassword(rs.getString("u_password"));
                temp.setUAccess(Byte.parseByte(rs.getString("u_access")));
                temp.setUMoney(Float.parseFloat(rs.getString("u_money")));
                users.add(temp);
            }
            state.close();
        } catch (Exception e) {
            e.printStackTrace();
        }
        if (users.size() == 0)
            return null;
        else
            return users;
    }

    /**
     * 添加User实体
     *
     * @param user 人员对象
     * @return 添加结果
     */
    public static boolean insertUser(User user) {
        //判断主码不存在
        if (getUserById(user.getUId()) != null)
            return false;
        String sql = "insert into T_User values('"
                + user.getUId() + "','"
                + user.getUName() + "','"
                + user.getUPassword() + "','"
                + user.getUAccess() + "','"
                + user.getUMoney() + "')";
        Statement state = null;
        int a = 0;
        try {
            state = connection.createStatement();
            a = state.executeUpdate(sql);
            state.close();
        } catch (Exception e) {
            e.printStackTrace();
        }
        return a == 1;
    }

    /**
     * 删除User实体
     *
     * @param id 用户号
     * @return 删除结果
     */
    public static boolean deleteUser(String id) {
        String sql = "delete from t_user where u_id='" + id + "'";
        Statement state = null;
        int a = 0;
        try {
            state = connection.createStatement();
            a = state.executeUpdate(sql);
            state.close();
        } catch (Exception e) {
            e.printStackTrace();
        }
        return a == 1;
    }

    /**
     * 修改User名称
     *
     * @param id   人员号
     * @param name 新名称
     * @return 修改结果
     */
    public static boolean updateUName(String id, String name) {
        String sql = "update t_user set u_name='" + name + "'" + " where u_id='" + id + "'";
        Statement state = null;
        int a = 0;
        try {
            state = connection.createStatement();
            a = state.executeUpdate(sql);
            state.close();
        } catch (Exception e) {
            e.printStackTrace();
        }
        return a == 1;
    }

    /**
     * 修改USer密码
     *
     * @param id       人员号
     * @param password 新密码
     * @return 修改结果
     */
    public static boolean updatePassword(String id, String password) {
        String sql = "update t_user set u_password='" + password + "'"
                + " where u_id='" + id + "'";
        Statement state = null;
        int a = 0;
        try {
            state = connection.createStatement();
            a = state.executeUpdate(sql);
            state.close();
        } catch (Exception e) {
            e.printStackTrace();
        }
        return a == 1;
    }

    /**
     * 修改User权限
     *
     * @param id     人员号
     * @param access 新权限
     * @return 修改结果
     */
    public static boolean updateAccess(String id, byte access) {
        String sql = "update t_user set u_access='" + access + "'" + " where u_id='" + id + "'";
        Statement state = null;
        int a = 0;
        try {
            state = connection.createStatement();
            a = state.executeUpdate(sql);
            state.close();
        } catch (Exception e) {
            e.printStackTrace();
        }
        return a == 1;
    }

    /**
     * User账户充值
     *
     * @param id    人员号
     * @param money 充值金额
     * @return 充值结果
     */
    public static boolean fundMoney(String id, Float money) {
        String sql = "update t_user set u_money=u_money+'" + money + "'" + " where u_id='" + id + "'";
        Statement state = null;
        int a = 0;
        try {
            state = connection.createStatement();
            a = state.executeUpdate(sql);
            state.close();
        } catch (Exception e) {
            e.printStackTrace();
        }
        return a == 1;
    }

    /**
     * User账户支付
     *
     * @param id    用户号
     * @param money 支付的金额
     * @return 支付结果
     */
    public static boolean payMoney(String id, Float money) {
        if (getUserById(id).getUMoney() < money)
            return false;
        String sql = "update t_user set u_money=u_money-'" + money + "'" + " where u_id='" + id + "'";
        Statement state = null;
        int a = 0;
        try {
            state = connection.createStatement();
            a = state.executeUpdate(sql);
            state.close();
        } catch (Exception e) {
            e.printStackTrace();
        }
        return a == 1;
    }

    /*----影片表T_Movie接口----*/

    /**
     * 获取所有Movie实体
     *
     * @return Movie对象列表
     */
    public static ArrayList<Movie> getAllMovies() {
        String sql = "select * from t_movie";
        Statement state = null;
        ResultSet rs = null;
        ArrayList<Movie> movies = new ArrayList<>();
        try {
            state = connection.createStatement();
            rs = state.executeQuery(sql);
            while (rs.next()) {
                Movie temp = new Movie();
                temp.setMId(rs.getString("m_id"));
                temp.setMName(rs.getString("m_name"));
                temp.setMType(rs.getString("m_type"));
                temp.setMTime(Integer.parseInt(rs.getString("m_time")));
                temp.setMComment(Float.parseFloat(rs.getString("m_comment")));
                temp.setDescription(rs.getString("m_description"));
                movies.add(temp);
            }
            state.close();
        } catch (Exception e) {
            e.printStackTrace();
        }
        if (movies.size() == 0)
            return null;
        else
            return movies;
    }

    /**
     * 通过电影号获取Movie实体
     *
     * @param id 电影号
     * @return Movie对象
     */
    public static Movie getMovieById(String id) {
        String sql = "select * from t_movie where m_id ='" + id + "'";
        Statement state = null;
        ResultSet rs;
        Movie movie = null;
        try {
            state = connection.createStatement();
            rs = state.executeQuery(sql);
            if (rs.next()) {
                movie = new Movie();
                movie.setMId(rs.getString("m_id"));
                movie.setMName(rs.getString("m_name"));
                movie.setMType(rs.getString("m_type"));
                movie.setMTime(Integer.parseInt(rs.getString("m_time")));
                movie.setMComment(Float.parseFloat(rs.getString("m_comment")));
                movie.setDescription(rs.getString("m_description"));
            }
            state.close();
        } catch (Exception e) {
            e.printStackTrace();
        }
        return movie;
    }

    /**
     * 通过名称获取Movie实体
     *
     * @param name 名称
     * @return Movie对象列表
     */
    public static ArrayList<Movie> getMoviesByName(String name) {
        String sql = "select * from t_Movie where m_name ='" + name + "'";
        Statement state = null;
        ResultSet rs = null;
        ArrayList<Movie> movies = new ArrayList<>();
        try {
            state = connection.createStatement();
            rs = state.executeQuery(sql);
            while (rs.next()) {
                Movie temp = new Movie();
                temp.setMId(rs.getString("m_id"));
                temp.setMName(rs.getString("m_name"));
                temp.setMType(rs.getString("m_type"));
                temp.setMTime(Integer.parseInt(rs.getString("m_time")));
                temp.setMComment(Float.parseFloat(rs.getString("m_comment")));
                temp.setDescription(rs.getString("m_description"));
                movies.add(temp);
            }
            state.close();
        } catch (Exception e) {
            e.printStackTrace();
        }
        if (movies.size() == 0)
            return null;
        else
            return movies;
    }

    /**
     * 添加Movie实体
     *
     * @param movie Movie实体
     * @return 添加结果
     */
    public static boolean insertMovie(Movie movie) {
        //判断主码不存在
        if (getMovieById(movie.getMId()) != null)
            return false;
        String sql = "insert into t_movie values('" + movie.getMId() + "','"
                + movie.getMName() + "','"
                + movie.getMType() + "','"
                + movie.getMTime() + "','"
                + movie.getMComment() + "','"
                + movie.getDescription() + "')";
        Statement state = null;
        int a = 0;
        try {
            state = connection.createStatement();
            a = state.executeUpdate(sql);
            state.close();
        } catch (Exception e) {
            e.printStackTrace();
        }
        return a == 1;
    }

    /**
     * 删除Movie实体
     *
     * @param id 电影号
     * @return 删除结果
     */
    public static boolean deleteMovie(String id) {
        String sql = "delete from t_movie where m_id='" + id + "'";
        Statement state = null;
        int a = 0;
        try {
            state = connection.createStatement();
            a = state.executeUpdate(sql);
            state.close();
        } catch (Exception e) {
            e.printStackTrace();
        }
        return a == 1;
    }


    /*----影厅表T_Theater接口----*/

    /**
     * 获取所有Theater实体
     *
     * @return Theater对象列表
     */
    public static ArrayList<Theater> getAllTheaters() {
        String sql = "select * from T_Theater";
        Statement state = null;
        ResultSet rs = null;
        ArrayList<Theater> users = new ArrayList<>();
        try {
            state = connection.createStatement();
            rs = state.executeQuery(sql);
            while (rs.next()) {
                Theater temp = new Theater();
                temp.setTId(rs.getString("t_id"));
                temp.setTType(Byte.parseByte(rs.getString("t_type")));
                temp.setTSize(Integer.parseInt(rs.getString("t_size")));
                users.add(temp);
            }
            state.close();
        } catch (Exception e) {
            e.printStackTrace();
        }
        if (users.size() == 0)
            return null;
        else
            return users;
    }

    /**
     * 通过影厅号获取Theater实体
     *
     * @param id 影厅号
     * @return Theater对象列表
     */
    public static Theater getTheaterById(String id) {
        String sql = "select * from T_Theater where t_id='" + id + "'";
        Statement state = null;
        ResultSet rs = null;
        Theater theater = null;
        try {
            state = connection.createStatement();
            rs = state.executeQuery(sql);
            if (rs.next()) {
                theater = new Theater();
                theater.setTId(rs.getString("t_id"));
                theater.setTType(Byte.parseByte(rs.getString("t_type")));
                theater.setTSize(Integer.parseInt(rs.getString("t_size")));
            }
            state.close();
        } catch (Exception e) {
            e.printStackTrace();
        }
        return theater;
    }

    /**
     * 通过类型获取Theater实体
     *
     * @param type 类型
     * @return Theater对象列表
     */
    public static ArrayList<Theater> getTheatersByType(byte type) {
        String sql = "select * from T_Theater where t_type='" + type + "'";
        Statement state = null;
        ResultSet rs = null;
        ArrayList<Theater> theaters = new ArrayList<>();
        try {
            state = connection.createStatement();
            rs = state.executeQuery(sql);
            while (rs.next()) {
                Theater temp = new Theater();
                temp.setTId(rs.getString("t_id"));
                temp.setTType(Byte.parseByte(rs.getString("t_type")));
                temp.setTSize(Integer.parseInt(rs.getString("t_size")));
                theaters.add(temp);
            }
            state.close();
        } catch (Exception e) {
            e.printStackTrace();
        }
        if (theaters.size() == 0)
            return null;
        else
            return theaters;
    }

    /**
     * 添加Theater实体
     *
     * @param theater Theater实体
     * @return 添加结果
     */
    public static boolean insertTheater(Theater theater) {
        //判断主码不存在
        if (getTheaterById(theater.getTId()) != null)
            return false;
        if (theater.getTSize() % 16 != 0)
            theater.setTSize(theater.getTSize() % 16);
        String sql = "insert into t_theater values('"
                + theater.getTId() + "','"
                + theater.getTType() + "','"
                + theater.getTSize() + "')";
        Statement state = null;
        int a = 0;
        try {
            state = connection.createStatement();
            a = state.executeUpdate(sql);
            state.close();
        } catch (Exception e) {
            e.printStackTrace();
        }
        return a == 1;
    }

    /**
     * 删除Theater实体
     *
     * @param id 影厅号
     * @return 删除结果
     */
    public static boolean deleteTheater(String id) {
        String sql = "delete from t_theater where t_id='" + id + "'";
        Statement state = null;
        int a = 0;
        try {
            state = connection.createStatement();
            a = state.executeUpdate(sql);
            state.close();
        } catch (Exception e) {
            e.printStackTrace();
        }
        return a == 1;
    }

    /*----场次表T_OnMovie接口----*/

    /**
     * 获取所有OnMovie实体
     *
     * @return OnMovie对象列表
     */
    public static ArrayList<OnMovie> getAllOnMovies() {
        String sql = "select * from T_OnMovie";
        Statement state = null;
        ResultSet rs = null;
        ArrayList<OnMovie> onMovies = new ArrayList<>();
        try {
            state = connection.createStatement();
            rs = state.executeQuery(sql);
            while (rs.next()) {
                OnMovie temp = new OnMovie();
                temp.setOId(rs.getString("o_id"));
                temp.setMId(rs.getString("m_id"));
                temp.setTId(rs.getString("t_id"));
                temp.setOBegin(DatePhaser.dateStrToDate(rs.getString("o_beginTime")));
                temp.setOEnd(DatePhaser.dateStrToDate(rs.getString("o_endTime")));
                temp.setOPrice(Float.parseFloat(rs.getString("o_price")));
                onMovies.add(temp);
            }
            state.close();
        } catch (Exception e) {
            e.printStackTrace();
        }
        if (onMovies.size() == 0)
            return null;
        else
            return onMovies;
    }

    /**
     * 通过场次号获取OnMovie实体
     *
     * @param oid 场次号
     * @return OnMovie对象
     */
    public static OnMovie getOnMovieByOId(String oid) {
        String sql = "select * from T_OnMovie where o_id='" + oid + "'";
        Statement state = null;
        ResultSet rs;
        OnMovie onMovie = null;
        try {
            state = connection.createStatement();
            rs = state.executeQuery(sql);
            while (rs.next()) {
                onMovie = new OnMovie();
                onMovie.setOId(rs.getString("o_id"));
                onMovie.setMId(rs.getString("m_id"));
                onMovie.setTId(rs.getString("t_id"));
                onMovie.setOBegin(DatePhaser.dateStrToDate(rs.getString("o_beginTime")));
                onMovie.setOEnd(DatePhaser.dateStrToDate(rs.getString("o_endTime")));
                onMovie.setOPrice(Float.parseFloat(rs.getString("o_price")));
            }
            state.close();
        } catch (Exception e) {
            e.printStackTrace();
        }
        return onMovie;
    }

    /**
     * 通过影片号获取OnMovie实体
     *
     * @param mid 影片号
     * @return OnMovie对象列表
     */
    public static ArrayList<OnMovie> getOnMoviesByMId(String mid) {
        String sql = "select * from T_OnMovie where m_id='" + mid + "'";
        Statement state = null;
        ResultSet rs = null;
        ArrayList<OnMovie> onMovies = new ArrayList<>();
        try {
            state = connection.createStatement();
            rs = state.executeQuery(sql);
            while (rs.next()) {
                OnMovie temp = new OnMovie();
                temp.setOId(rs.getString("o_id"));
                temp.setMId(rs.getString("m_id"));
                temp.setTId(rs.getString("t_id"));
                temp.setOBegin(DatePhaser.dateStrToDate(rs.getString("o_beginTime")));
                temp.setOEnd(DatePhaser.dateStrToDate(rs.getString("o_endTime")));
                temp.setOPrice(Float.parseFloat(rs.getString("o_price")));
                onMovies.add(temp);
            }
            state.close();
        } catch (Exception e) {
            e.printStackTrace();
        }
        if (onMovies.size() == 0)
            return null;
        else
            return onMovies;
    }

    /**
     * 通过影厅号获取OnMovie实体
     *
     * @param tid 影厅号
     * @return OnMovie对象列表
     */
    public static ArrayList<OnMovie> getOnMoviesByTId(String tid) {
        String sql = "select * from T_OnMovie where t_id='" + tid + "'";
        Statement state = null;
        ResultSet rs = null;
        ArrayList<OnMovie> onMovies = new ArrayList<>();
        try {
            state = connection.createStatement();
            rs = state.executeQuery(sql);
            while (rs.next()) {
                OnMovie temp = new OnMovie();
                temp.setOId(rs.getString("o_id"));
                temp.setMId(rs.getString("m_id"));
                temp.setTId(rs.getString("t_id"));
                temp.setOBegin(DatePhaser.dateStrToDate(rs.getString("o_beginTime")));
                temp.setOEnd(DatePhaser.dateStrToDate(rs.getString("o_endTime")));
                temp.setOPrice(Float.parseFloat(rs.getString("o_price")));
                onMovies.add(temp);
            }
            state.close();
        } catch (Exception e) {
            e.printStackTrace();
        }
        if (onMovies.size() == 0)
            return null;
        else
            return onMovies;
    }

    /**
     * 数字补零转换字符串
     *
     * @param i 数字值
     * @return 字符串
     */
    private static String getString(int i) {
        if (i > 0 && i < 10) {
            return "0" + i;
        }
        if (i >= 10 && i <= 99) {
            return Integer.toString(i);
        }
        return null;
    }

    /**
     * 添加OnMovie实体
     *
     * @param onMovie OnMovie对象
     * @return 添加结果
     */
    public static boolean insertOnMovie(OnMovie onMovie) {
        //判断外码存在
        if ((getMovieById(onMovie.getMId()) == null) ||
                (getTheaterById(onMovie.getTId()) == null))
            return false;
        //判断主码不存在
        if (getOnMovieByOId(onMovie.getOId()) != null)
            return false;
        //判断时间的正确性
        if (onMovie.getOEnd().before(onMovie.getOBegin()))
            return false;
        List<OnMovie> onMovies = getOnMoviesByTId(onMovie.getTId());
        //判断是否有时间冲突
        if (onMovies != null)
            for (OnMovie temp : onMovies)
                if (onMovie.getOBegin().before(DatePhaser.addDateMinutes(temp.getOEnd(), 10)))
                    return false;
        //添加场次
        String sql = "insert into t_onMovie values('" + onMovie.getOId() + "','"
                + onMovie.getMId() + "','"
                + onMovie.getTId() + "','"
                + DatePhaser.dateToDateStr(onMovie.getOBegin()) + "','"
                + DatePhaser.dateToDateStr(onMovie.getOEnd()) + "','"
                + onMovie.getOPrice() + "')";
        Statement state = null;
        int a = 0;
        try {
            state = connection.createStatement();
            a = state.executeUpdate(sql);
            state.close();
        } catch (Exception e) {
            e.printStackTrace();
        }
        //添加座位
        int size = getTheaterById(onMovie.getTId()).getTSize();
        int row = size / 16;
        try {
            for (int i = 1; i <= row; i++)
                for (int j = 1; j <= 16; j++)
                    if (!insertSeat(new Seat(getString(i) + getString(j),
                            onMovie.getOId(),
                            EnumSeatStatus.Unselected)))
                        throw new Exception("座位添加异常");
        } catch (Exception e) {
            e.printStackTrace();
        }
        return a == 1;
    }

    /**
     * 删除场次
     *
     * @param oid 场次号
     * @return 删除结果
     */
    public static boolean deleteOnMovie(String oid) {
        String sql = "delete from t_onMovie where o_id='" + oid + "'";
        Statement state = null;
        int a = 0;
        try {
            state = connection.createStatement();
            a = state.executeUpdate(sql);
            state.close();
        } catch (Exception e) {
            e.printStackTrace();
        }
        return a == 1;
    }


    /*----座位表T_Seat接口----*/

    /**
     * 获取所有Seat实体
     *
     * @return Seat信息
     */
    public static ArrayList<Seat> getAllSeat() {
        String sql = "select * from T_Seat";
        Statement state = null;
        ResultSet rs;
        ArrayList<Seat> seats = new ArrayList<>();
        try {
            state = connection.createStatement();
            rs = state.executeQuery(sql);
            while (rs.next()) {
                Seat f = new Seat();
                f.setOId(rs.getString("o_id"));
                f.setSId(rs.getString("s_id"));
                f.setSStatus(Byte.parseByte(rs.getString("s_status")));
                seats.add(f);
            }
            state.close();
        } catch (Exception e) {
            e.printStackTrace();
        }
        if (seats.size() == 0)
            return null;
        else
            return seats;
    }

    /**
     * 通过场次号获取Seat实体
     *
     * @param oid 场次号
     * @return Seat对象列表
     */
    public static ArrayList<Seat> getSeatsByOid(String oid) {
        String sql = "select * from T_Seat where o_id='" + oid + "'";
        Statement state = null;
        ResultSet rs;
        ArrayList<Seat> users = new ArrayList<Seat>();
        try {
            state = connection.createStatement();
            rs = state.executeQuery(sql);
            while (rs.next()) {
                Seat f = new Seat();
                f.setOId(rs.getString("o_id"));
                f.setSId(rs.getString("s_id"));
                f.setSStatus(Byte.parseByte(rs.getString("s_status")));
                users.add(f);
            }
            state.close();
        } catch (Exception e) {
            e.printStackTrace();
        }
        if (users.size() == 0)
            return null;
        else
            return users;
    }

    /**
     * 通过场次号获取Seat实体
     *
     * @param oid 场次号
     * @return Seat对象列表
     */
    public static Seat getSeatsByOidSid(String oid, String sid) {
        String sql = "select * from T_Seat where o_id='" + oid + "'"
                + "and s_id='" + sid + "'";
        Statement state = null;
        ResultSet rs = null;
        Seat seat = null;
        try {
            state = connection.createStatement();
            rs = state.executeQuery(sql);
            if (rs.next()) {
                seat = new Seat();
                seat.setOId(rs.getString("o_id"));
                seat.setSId(rs.getString("s_id"));
                seat.setSStatus(Byte.parseByte(rs.getString("s_status")));
            }
            state.close();
        } catch (Exception e) {
            e.printStackTrace();
        }
        return seat;
    }

    /**
     * 添加Seat实体
     *
     * @param seat Seat对象
     * @return 添加结果
     */
    private static boolean insertSeat(Seat seat) {
        //判断主码不存在
        if (getSeatsByOidSid(seat.getOId(), seat.getSId()) != null)
            return false;
        //判断外码存在
        if (getOnMovieByOId(seat.getOId()) == null)
            return false;
        String sql = "insert into T_Seat values('" + seat.getOId() + "','"
                + seat.getSId() + "','"
                + seat.getSStatus() + "')";
        Statement state = null;
        int a = 0;
        try {
            state = connection.createStatement();
            a = state.executeUpdate(sql);
            state.close();
        } catch (Exception e) {
            e.printStackTrace();
        }
        return a == 1;
    }

    /**
     * 修改座位状态
     *
     * @param oid       场次号
     * @param sid       座位号
     * @param newStatus 新座位状态
     * @return 修改结果
     */
    public static boolean updateSeatStatus(String oid, String sid, byte newStatus) {
        String sql = "update T_Seat set s_status = '" + newStatus + "'"
                + " where o_id = '" + oid
                + "' and s_id = '" + sid + "'";
        Statement state = null;
        int a = 0;
        try {
            state = connection.createStatement();
            a = state.executeUpdate(sql);
            state.close();
        } catch (Exception e) {
            e.printStackTrace();
        }
        return a == 1;
    }


    /*----购票记录表T_Record接口----*/

    /**
     * 获取所有Record实体
     *
     * @return Record对象列表
     */
    public static ArrayList<Record> getAllRecord() {
        String sql = "select * from T_Record";
        Statement state = null;
        ResultSet rs = null;
        ArrayList<Record> records = new ArrayList<>();
        try {
            state = connection.createStatement();
            rs = state.executeQuery(sql);
            while (rs.next()) {
                Record temp = new Record();
                temp.setUId(rs.getString("u_id"));
                temp.setOId(rs.getString("o_id"));
                temp.setSId(rs.getString("s_id"));
                temp.setRTime(DatePhaser.dateStrToDate(rs.getString("r_time")));
                temp.setRPrice(Float.parseFloat(rs.getString("r_price")));
                temp.setStatus(Byte.parseByte(rs.getString("r_status")));
                records.add(temp);
            }
            state.close();
        } catch (Exception e) {
            e.printStackTrace();
        }
        if (records.size() == 0)
            return null;
        else
            return records;
    }

    /**
     * 通过用户号、场次号、座位号获取Record实体
     *
     * @param uid 用户号
     * @param oid 场次号
     * @param sid 座位号
     * @return Record对象
     */
    public static Record getRecordByUidOidSid(String uid, String oid, String sid) {
        String sql = "select * from T_Record where u_id='" + uid + "'"
                + "and o_id='" + oid + "'"
                + "and s_id='" + sid + "'";
        Statement state = null;
        ResultSet rs = null;
        Record record = null;
        try {
            state = connection.createStatement();
            rs = state.executeQuery(sql);
            if (rs.next()) {
                record = new Record();
                record.setUId(rs.getString("u_id"));
                record.setOId(rs.getString("o_id"));
                record.setSId(rs.getString("s_id"));
                record.setRTime(DatePhaser.dateStrToDate(rs.getString("r_time")));
                record.setRPrice(Float.parseFloat(rs.getString("r_price")));
                record.setStatus(Byte.parseByte(rs.getString("r_status")));
            }
            state.close();
        } catch (Exception e) {
            e.printStackTrace();
        }
        return record;
    }

    /**
     * 通过用户号获取Record实体
     *
     * @param uid 用户号
     * @return Record对象列表
     */
    public static ArrayList<Record> getRecordByUid(String uid) {
        String sql = "select * from T_Record where u_id='" + uid + "'";
        Statement state = null;
        ResultSet rs = null;
        ArrayList<Record> records = new ArrayList<>();
        try {
            state = connection.createStatement();
            rs = state.executeQuery(sql);
            while (rs.next()) {
                DataUtils.Record temp = new DataUtils.Record();
                temp.setUId(rs.getString("u_id"));
                temp.setOId(rs.getString("o_id"));
                temp.setSId(rs.getString("s_id"));
                temp.setRTime(DatePhaser.dateStrToDate(rs.getString("r_time")));
                temp.setRPrice(Float.parseFloat(rs.getString("r_price")));
                temp.setStatus(Byte.parseByte(rs.getString("r_status")));
                records.add(temp);
            }
            state.close();
        } catch (Exception e) {
            e.printStackTrace();
        }
        if (records.size() == 0)
            return null;
        else
            return records;
    }

    /**
     * 根据场次号获取Record实体
     *
     * @param oid 场次号
     * @return Record对象列表
     */
    public static ArrayList<Record> getRecordByOid(String oid) {
        String sql = "select * from T_Record where o_id='" + oid + "'";
        Statement state = null;
        ResultSet rs = null;
        ArrayList<Record> records = new ArrayList<>();
        try {
            state = connection.createStatement();
            rs = state.executeQuery(sql);
            while (rs.next()) {
                DataUtils.Record temp = new DataUtils.Record();
                temp.setUId(rs.getString("u_id"));
                temp.setOId(rs.getString("o_id"));
                temp.setSId(rs.getString("s_id"));
                temp.setRTime(DatePhaser.dateStrToDate(rs.getString("r_time")));
                temp.setRPrice(Float.parseFloat(rs.getString("r_price")));
                temp.setStatus(Byte.parseByte(rs.getString("r_status")));
                records.add(temp);
            }
            state.close();
        } catch (Exception e) {
            e.printStackTrace();
        }
        if (records.size() == 0)
            return null;
        else
            return records;
    }

    /**
     * 修改Record状态
     *
     * @param uid      用户号
     * @param sid      座位号
     * @param oid      场次号
     * @param newStatus 新状态
     * @return 修改结果
     */
    public static boolean updateRecordStatus(String uid, String sid, String oid, String newStatus) {
        String sql = "update T_Record set r_status = '" + newStatus + "'"
                + " where u_id = '" + uid
                + "' and s_id = '" + sid
                + "' and o_id = '" + oid + "'";
        Statement state = null;
        int a = 0;
        try {
            state = connection.createStatement();
            a = state.executeUpdate(sql);
            state.close();
        } catch (Exception e) {
            e.printStackTrace();
        }
        return a == 1;
    }

    /**
     * 添加Record实体
     *
     * @param record Record对象
     * @return 添加结果
     */
    public static boolean insertRecord(Record record) {
        //判断外码存在
        if (getSeatsByOidSid(record.getOId(), record.getSId()) == null)
            return false;
        if (getUserById(record.getUId()) == null)
            return false;
        //判断主码不存在
        if (getRecordByUidOidSid(record.getUId(), record.getOId(), record.getSId()) != null)
            return false;
        String sql = "insert into T_Record values('" + record.getUId() + "','"
                + record.getOId() + "','"
                + record.getSId() + "','"
                + DatePhaser.dateToDateStr(record.getRTime()) + "','"
                + record.getRPrice() + "','"
                + record.getStatus() + "')";
        Statement state = null;
        int a = 0;
        try {
            state = connection.createStatement();
            a = state.executeUpdate(sql);
            state.close();
        } catch (Exception e) {
            e.printStackTrace();
        }
        return a == 1;
    }


    /*----密钥表T_IDK接口----*/

    /**
     * 通过机器号获取对应密钥
     *
     * @param id 机器号
     * @return 密钥
     */
    public static String getKeyById(String id) {
        String sql = "select * from T_IDK where Id='" + id + "'";
        Statement state = null;
        ResultSet rs = null;
        String key = null;
        try {
            state = connection.createStatement();
            rs = state.executeQuery(sql);
            if (rs.next())
                key = rs.getString("I_Key");
            state.close();
        } catch (Exception e) {
            e.printStackTrace();
        }
        return key;
    }
}