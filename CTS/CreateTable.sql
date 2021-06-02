/*drop TABLE t_Record;
drop TABLE t_Seat;
drop TABLE t_onmovie;
drop TABLE T_User;
drop TABLE T_Movie;
drop TABLE T_Theater;
drop TABLE t_IDK;*/

create table T_User (
	U_Id varchar(20) not null primary key, 
	U_Name varchar(20) not null,
	U_Password varchar(20) not null,
	U_Access varchar(1) not null, 
	U_Money float not null
);
create table T_Movie (
	M_Id varchar(20) not null primary key,   
	M_Name varchar(40) not null,
	M_Type varchar(8) not null,
	M_Time int not null,
	M_Comment float not null,
	M_Description Varchar(800) not null
);
create table T_Theater (
	T_Id Varchar(20) not null primary key,
	T_Type Varchar(1) not null,
	T_Size Int not null
);
create table T_OnMovie (
	O_Id Varchar(20) not null primary key,
	M_Id Varchar(20) not null,
	T_Id Varchar(20) not null,
	O_BeginTime Varchar(25) not null,
	O_EndTime Varchar(25) not null,
	O_Price Float not null,
	foreign key(M_Id) references t_movie(M_Id),
	foreign key(T_Id) references t_theater(T_Id),
	index(O_Id)
);
create table T_Seat (
	O_Id Varchar(20) not null,
	S_Id Varchar(20) not null,
	S_Status Varchar(1) not null,
	primary key(O_Id,S_Id),
	foreign key(O_Id) references T_OnMovie(O_Id),
	index (O_Id,S_Id)
);
create table T_Record (
	U_Id Varchar(20) not null,
	O_Id Varchar(20) not null,
	S_Id Varchar(20) not null,
	R_Time Varchar(25)  not null,
	R_Price Float not null,
	R_Status Varchar(1) not null,
	primary key(U_Id,S_Id,O_Id),
	foreign key(U_Id) references T_User (U_Id),
	foreign key(O_Id,S_Id) references T_Seat (O_Id,S_Id)
);
create table T_IDK ( 
	Id varchar(20) not null primary key,
	I_Key varchar(8) not null
);