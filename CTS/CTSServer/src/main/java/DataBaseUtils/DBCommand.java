package DataBaseUtils;

import Mapper.Element.*;
import Services.*;

import java.sql.Timestamp;
import java.util.List;

public class DBCommand {
    private static IdkService idkService=new IdkService();
    private static MovieService movieService=new MovieService();
    private static OnMovieService onMovieService=new OnMovieService();
    private static RecordService recordService=new RecordService();
    private static SeatService seatService=new SeatService();
    private static TheaterService theaterService=new TheaterService();
    private static UserService userService=new UserService();

    /*--------------------------------User----------------------------------------------*/
    public static List<User> getAllUsers(){
        return userService.getAllUsers();
    };
    public static List<User> getAllUsersByAccess(String var1){
        return userService.getAllUsersByAccess(var1);
    };
    public static List<User> getAllUsersByNames(String var1){
        return userService.getAllUsersByNames(var1);
    };
    public static User getUserById(String var1){
        return userService.getUserById(var1);
    };
    public static boolean updateUID(String var1, String var2) {
        return userService.updateUID(var1,var2);
    };
    public static boolean updatePassword(String var1, String var2){
        return userService.updatePassword(var1,var2);
    };
    public static boolean updateAccess(String var1, String var2){
        return userService.updateAccess(var1,var2);
    };
    public static boolean deleteUser(String var1) {
        return userService.deleteUser(var1);
    };

    /*--------------------------------movie----------------------------------------------*/
    public static List<Movie> getAllMovies(){
        return movieService.getAllMovies();
    };
    public static List<Movie> getAllMoviesByName(String var1){
        return movieService.getAllMoviesByName(var1);
    };
    public static List<Movie> getAllMoviesById(String var1) {
        return movieService.getAllMoviesById(var1);
    };
    public static boolean AddMovie(Movie var1){
        return movieService.AddMovie(var1);
    };
    public static boolean deleteMovieById(String var1){
        return movieService.deleteMovieById(var1);
    };

    /*--------------------------------theater----------------------------------------------*/
    public static List<Theater> getAllTheater() {
        return theaterService.getAllTheater();
    };
    public static List<Theater> getAllTheaterByID(String var1){
        return theaterService.getAllTheaterByID(var1);
    };
    public static List<Theater> getAllTheaterByType(String var1){
        return theaterService.getAllTheaterByType(var1);
    };
    public static boolean addTheater(Theater var1){
        return theaterService.addTheater(var1);
    };
    public static boolean deleteTheater(String var1){
        return theaterService.deleteTheater(var1);
    };

    /*--------------------------------OnMovie----------------------------------------------*/

    public static List<OnMovie> getAllOnMovies(){
        return onMovieService.getAllOnMovies();
    };
    public static List<OnMovie> getAllOnMoviesByOnId(String var1){
        return onMovieService.getAllOnMoviesByOnId(var1);
    };
    public static List<OnMovie> getAllOnMoviesByMovieId(String var1){
        return onMovieService.getAllOnMoviesByMovieId(var1);
    };
    public static List<OnMovie> getAllOnMoviesByTheaterId(String var1){
        return onMovieService.getAllOnMoviesByTheaterId(var1);
    };
    public static List<OnMovie> getAllOnMoviesByBeginTime(Timestamp var1){
        return onMovieService.getAllOnMoviesByBeginTime(var1);
    };
    public static boolean addOnMovie(OnMovie var){
        return onMovieService.addOnMovie(var);
    };
    public static boolean deleteOnMovie(String var1){
        return onMovieService.deleteOnMovie(var1);
    };

    /*--------------------------------Seat----------------------------------------------*/
    public static List<Seat> getAllSeat(){
        return seatService.getAllSeat();
    };
    public static List<Seat> getAllSeatBySid(String var1){
        return seatService.getAllSeatBySid(var1);
    };
    public static List<Seat> getAllSeatByOid(String var1){
        return seatService.getAllSeatByOid(var1);
    };


    /*--------------------------------Record----------------------------------------------*/
    public static List<Mapper.Element.Record> getAllRecord(){
        return recordService.getAllRecord();
    };
    public static List<Mapper.Element.Record> getAllRecordByUid(String var1){
        return recordService.getAllRecordByUid(var1);
    };
    public static List<Mapper.Element.Record> getAllRecordByOid(String var1){
        return recordService.getAllRecordByOid(var1);
    };
    public static boolean updateStatus(String var1, String var2, String var3, String var4){
        return recordService.updateStatus(var1,var2,var3,var4);
    };
    public static boolean addRecord(Mapper.Element.Record var1){
        return recordService.addRecord(var1);
    };

    /*--------------------------------IDK----------------------------------------------*/

    public static List<Idk> getIDKById(String var1){
        return idkService.getIDKById(var1);
    };
    public static boolean addIDK(Idk var1){
        return idkService.addIDK(var1);
    };

}
