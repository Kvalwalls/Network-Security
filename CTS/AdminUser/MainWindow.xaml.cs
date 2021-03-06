using AdminUser;
using AdminUser.AppService;
using AdminUser.Entity;
using AdminUser.Transmission;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace AdminUser
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private static User user;
        private static AUVHandler handler;
        //private bool isEyeOpen;
        //private bool modNameSure;
        //private bool modPwdSure;
        private DispatcherTimer showTimer;
        private static List<User> users = new List<User>();
        private static List<Movie> movies = new List<Movie>();
        private static List<Theater> theaters = new List<Theater>();
        private static List<OnMovie> onmovies = new List<OnMovie>();
        private static List<Record> records = new List<Record>();
        public static List<TransMessage> trans = new List<TransMessage>();
        public static Package package;

        public MainWindow(User u,Package p)
        {
            InitializeComponent();
            handler = AUVHandler.GetInstance();
            InitTextBlock_Time();
            InitLists();
            user = u;
            package = p;
            handler.SetPackage(package);
            InitTextBlock_Hello();
            //Monitor.PartEvent += OnStep;//将该类中的函数注册到Monitor静态类的PartEvent事件中。
        }

        public MainWindow()
        {
            InitializeComponent();
        }

        private void InitTextBlock_Time()
        {
            showTimer = new System.Windows.Threading.DispatcherTimer();
            showTimer.Tick += new EventHandler
                (
                    (object sender, EventArgs e) =>
                    {
                        this.TextBlock_Time.Text = DateTime.Now.ToString("yyyy年MM月dd日 dddd HH:mm:ss");
                    }
                );
            showTimer.Interval = new TimeSpan(0, 0, 0, 1, 0);
            showTimer.Start();
        }

        private void InitLists()
        {

            /*User u1 = new User("1", "dx", "123", 2, 100);
            u1.UAccess = "普通用户";
            User u2 = new User("3", "xz", "123", 3, 100);
            u2.UAccess = "VIP用户";
            User u3 = new User("2", "wbc", "123", 4, 100);
            u3.UAccess = "SVIP用户";
            User u4 = new User("4", "zr", "123", 1, 100);
            u4.UAccess = "普通管理员";
            Theater t1 = new Theater("1", 1, 30);
            Theater t2 = new Theater("2", 1, 10);
            Movie m1 = new Movie("1", "x", "1", 60, 30, "1");
            Movie m2 = new Movie("2", "xz", "1", 60, 30, "1");
            OnMovie o1 = new OnMovie("1", "1", "1", DateTime.Now, DateTime.Now.AddMinutes(60), 90);
            OnMovie o2 = new OnMovie("2", "2", "2", DateTime.Now, DateTime.Now, 90);
            Record r1 = new Record("1", "1", "1", DateTime.Now, 80, 1);
            Record r2 = new Record("2", "2", "2", DateTime.Now, 90, 1);
            users.Add(u1);
            users.Add(u2);
            users.Add(u3);
            users.Add(u4);
            movies.Add(m1);
            movies.Add(m2);
            theaters.Add(t1);
            theaters.Add(t2);
            onmovies.Add(o1);
            onmovies.Add(o2);
            records.Add(r1);
            records.Add(r2);
            UserList.Items.Add(users[0]);
            UserList.Items.Add(users[1]);
            UserList.Items.Add(users[2]);
            UserList.Items.Add(users[3]);
            TheaterList.Items.Add(theaters[0]);
            TheaterList.Items.Add(theaters[1]);
            MovieList.Items.Add(movies[0]);
            MovieList.Items.Add(movies[1]);
            OnMovieList.Items.Add(onmovies[0]);
            OnMovieList.Items.Add(onmovies[1]);
            TicketList.Items.Add(records[0]);
            TicketList.Items.Add(records[1]);*/
            //handler.GetMoviePictures();
            handler.getUserRequest();
            users = handler.getUserReply();
            for(int i = 0; i < users.Count; i++)
            {
                switch (users[i].Uaccess)
                {
                    case 0:
                        {
                            users[i].UAccess = "超级管理员";
                            break;
                        }
                    case 1:
                        {
                            users[i].UAccess = "普通管理员";
                            break;
                        }
                    case 2:
                        {
                            users[i].UAccess = "普通用户";
                            break;
                        }
                    case 3:
                        {
                            users[i].UAccess = "VIP用户";
                            break;
                        }
                    case 4:
                        {
                            users[i].UAccess = "SVIP用户";
                            break;
                        }
                    default:
                        {
                            break;
                        }
                }
                    
                UserList.Items.Add(users[i]);
            }

            handler.getMovieRequest();
            movies = handler.getMovieReply();
            for (int i = 0; i < movies.Count; i++)
            {
                MovieList.Items.Add(movies[i]);
            }

            handler.getTheaterRequest();
            theaters = handler.getTheaterReply();
            for (int i = 0; i < theaters.Count; i++)
            {
                switch (theaters[i].Ttype)
                {
                    case 0:
                        {
                            theaters[i].TType = "普通影厅";
                            break;
                        }
                    case 1:
                        {
                            theaters[i].TType = "VIP影厅";
                            break;
                        }
                    case 2:
                        {
                            theaters[i].TType = "SVIP影厅";
                            break;
                        }
                    default:
                        {
                            break;
                        }
                }
                TheaterList.Items.Add(theaters[i]);
            }

            handler.getOnMovieRequest();
            onmovies = handler.getOnMovieReply();
            for (int i = 0; i < onmovies.Count; i++)
            {
                OnMovieList.Items.Add(onmovies[i]);
            }

            handler.getRecordRequest();
            records = handler.getRecordReply();
            for (int i = 0; i < records.Count; i++)
            {
                switch (records[i].Rstatus)
                {
                    case 0:
                        {
                            records[i].RStatus = "等待支付";
                            break;
                        }
                    case 1:
                        {
                            records[i].RStatus = "购票成功";
                            break;
                        }
                    case 2:
                        {
                            records[i].RStatus = "购票失败";
                            break;
                        }
                    default:
                        {
                            break;
                        }
                }
                TicketList.Items.Add(records[i]);
            }

        }

        private void InitTextBlock_Hello()
        {
            TextBlock_Hello.Text = "欢迎您！";
            switch (user.Uaccess)
            {
                case 3:
                    {
                        Image_vip.Opacity = 1;
                        TextBlock_Hello.Text += "VIP用户：";
                        break;
                    }
                case 4:
                    {
                        Image_svip.Opacity = 1;
                        TextBlock_Hello.Text += "SVIP用户：";
                        break;
                    }
                case 2:
                    {
                        Image_common.Opacity = 1;
                        TextBlock_Hello.Text += "普通用户：";
                        break;
                    }
                case 1:
                    {
                        Image_common.Opacity = 1;
                        TextBlock_Hello.Text += "普通管理员：";
                        break;
                    }
                case 0:
                    {
                        Image_common.Opacity = 1;
                        TextBlock_Hello.Text += "超级管理员：";
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
            TextBlock_Hello.Text += user.Uname;
        }

        private void X_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Cursor = Cursors.Hand;
        }

        private void X_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Cursor = Cursors.Arrow;
        }

        private void UserSearch_Click(object sender, RoutedEventArgs e)
        {
            byte acc = 0;
            UserList.Items.Clear();
            if(TextBox_UserSearchid.Text == "" && (ComboBox_Access.SelectedItem == null || ComboBox_Access.Text == ""))
            {
                for(int i = 0; i < users.Count; i++)
                {
                    UserList.Items.Add(users[i]);
                }
                UserList.Items.Refresh();
            }
            if (TextBox_UserSearchid.Text != "" && (ComboBox_Access.SelectedItem == null || ComboBox_Access.Text == ""))
            {
                string sid = TextBox_UserSearchid.Text;
                for(int i = 0; i < users.Count; i++)
                {
                    if(users[i].Uid == sid)
                    {
                        UserList.Items.Add(users[i]);
                    }
                }
                UserList.Items.Refresh();
            }

            if (TextBox_UserSearchid.Text == "" && (ComboBox_Access.SelectedItem != null || ComboBox_Access.Text != ""))
            {
                string sacc = ComboBox_Access.Text;
                if(sacc == "普通用户")
                {
                    acc = 2;
                }
                else if (sacc == "VIP用户")
                {
                    acc = 3;
                }
                else if(sacc == "SVIP用户")
                {
                    acc = 4;
                }
                else if(sacc == "普通管理员")
                {
                    acc = 1;
                }

                for (int i = 0; i < users.Count; i++)
                {
                    if (users[i].Uaccess == acc)
                    {
                        UserList.Items.Add(users[i]);
                    }
                }
                UserList.Items.Refresh();
            }
            
            if (TextBox_UserSearchid.Text != "" && (ComboBox_Access.SelectedItem != null || ComboBox_Access.Text != ""))
            {
                string sid = TextBox_UserSearchid.Text;
                string sacc = ComboBox_Access.Text;
                if (sacc == "普通用户")
                {
                    acc = 2;
                }
                else if (sacc == "VIP用户")
                {
                    acc = 3;
                }
                else if (sacc == "SVIP用户")
                {
                    acc = 4;
                }
                else if (sacc == "普通管理员")
                {
                    acc = 1;
                }
                for (int i = 0; i < users.Count; i++)
                {
                    if (users[i].Uid == sid && users[i].Uaccess == acc)
                    {
                        UserList.Items.Add(users[i]);
                    }
                }
                UserList.Items.Refresh();
            }
        }

        private void UserList_Click(object sender, RoutedEventArgs e)
        {
            //获得点击的列
            GridViewColumn clickedColumn = (e.OriginalSource as GridViewColumnHeader).Column;
            if (clickedColumn.Header.ToString() == "用户号")
            {
                UserList.Items.SortDescriptions.Add(new SortDescription("id", ListSortDirection.Ascending));
            }
        }

        private void UserAdd_Click(object sender, RoutedEventArgs e)
        {
            Hide();
            AddUser add = new AddUser(users,package);
            add.ShowDialog();
            if (add.Tag != null)
            {
                User model = add.Tag as User;
                byte sacc = model.Uaccess;
                if (sacc == 2)
                {
                    model.UAccess = "普通用户";
                }
                else if (sacc == 3)
                {
                    
                    model.UAccess = "VIP用户";
                }
                else if (sacc == 4)
                {
                    
                    model.UAccess = "SVIP用户";
                }
                else if (sacc == 1)
                {
                    
                    model.UAccess = "普通管理员";
                }
                if (model != null)
                {
                    users.Add(model);
                    UserList.Items.Add(model);
                    UserList.Items.Refresh();
                }
            }
            Show();
        }
        
        private void UserDelete_Click(object sender, RoutedEventArgs e)
        {
            if (UserList.SelectedItem == null)
            {
                MessageBox.Show("请选择删除项", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                //选中行数据
                User u = UserList.SelectedItem as User;
                //MessageBox.Show("姓名：" + u.name + "\n\n" + "密码：" + u.password);

                //send 发送删除请求
                handler.delUserRequest(u);
                //删除成功
                if (handler.delUserReply() == "删除成功")
                {
                    MessageBox.Show("删除成功！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    //删除选中元素
                    users.Remove(u);
                    UserList.Items.Remove(UserList.SelectedItem);
                }
                else
                {
                    MessageBox.Show("删除失败！", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                //刷新listview
                UserList.Items.Refresh();
            }
        }

        private void TheaterSearch_Click(object sender, RoutedEventArgs e)
        {
            TheaterList.Items.Clear();
            byte type = 0;
            if (TextBox_TheaterSearchid.Text == "" && (ComboBox_TheaterType.SelectedItem == null || ComboBox_TheaterType.Text == "") && (ComboBox_TheaterSize.SelectedItem == null || ComboBox_TheaterSize.Text == ""))
            {
                for (int i = 0; i < theaters.Count; i++)
                {
                    TheaterList.Items.Add(theaters[i]);
                }
                TheaterList.Items.Refresh();
            }
            if (TextBox_TheaterSearchid.Text == "" && (ComboBox_TheaterType.SelectedItem == null || ComboBox_TheaterType.Text == "") && (ComboBox_TheaterSize.SelectedItem != null || ComboBox_TheaterSize.Text != ""))
            {
                string size = ComboBox_TheaterSize.Text;
                int ssize = 0;
                if (size == "16")
                {
                    ssize = 16;
                }
                else if (size == "32")
                {
                    ssize = 32;
                }
                else if (size == "64")
                {
                    ssize = 64;
                }
                for (int i = 0; i < theaters.Count; i++)
                {
                    if (theaters[i].Tsize == ssize)
                    {
                        TheaterList.Items.Add(theaters[i]);
                    }
                }
                TheaterList.Items.Refresh();
            }

            if (TextBox_TheaterSearchid.Text == "" && (ComboBox_TheaterType.SelectedItem != null || ComboBox_TheaterType.Text != "") && (ComboBox_TheaterSize.SelectedItem == null || ComboBox_TheaterSize.Text == ""))
            { 
                string stype = ComboBox_TheaterType.Text;
                if(stype == "普通影厅")
                {
                    type = 0;
                }
                else if (stype == "VIP影厅")
                {
                    type = 1;
                }
                else if (stype == "SVIP影厅")
                {
                    type = 2;
                }
                for (int i = 0; i < theaters.Count; i++)
                {
                    if (theaters[i].Ttype == type)
                    {
                        TheaterList.Items.Add(theaters[i]);
                    }
                }
                TheaterList.Items.Refresh();
            }

            if (TextBox_TheaterSearchid.Text != "" && (ComboBox_TheaterType.SelectedItem == null || ComboBox_TheaterType.Text == "") && (ComboBox_TheaterSize.SelectedItem == null || ComboBox_TheaterSize.Text == ""))
            {
                string sid = TextBox_TheaterSearchid.Text;
                for (int i = 0; i < theaters.Count; i++)
                {
                    if (theaters[i].Tid == sid)
                    {
                        TheaterList.Items.Add(theaters[i]);
                    }
                }
                TheaterList.Items.Refresh();
            }

            if (TextBox_TheaterSearchid.Text != "" && (ComboBox_TheaterType.SelectedItem != null || ComboBox_TheaterType.Text != "") && (ComboBox_TheaterSize.SelectedItem == null || ComboBox_TheaterSize.Text == ""))
            {
                string sid = TextBox_TheaterSearchid.Text;
                string stype = ComboBox_TheaterType.Text;
                if (stype == "普通影厅")
                {
                    type = 0;
                }
                else if (stype == "VIP影厅")
                {
                    type = 1;
                }
                else if (stype == "SVIP影厅")
                {
                    type = 2;
                }
                for (int i = 0; i < theaters.Count; i++)
                {
                    if (theaters[i].Tid == sid && theaters[i].Ttype == type)
                    {
                        TheaterList.Items.Add(theaters[i]);
                    }
                }
                TheaterList.Items.Refresh();
            }

            if (TextBox_TheaterSearchid.Text != "" && (ComboBox_TheaterType.SelectedItem == null || ComboBox_TheaterType.Text == "") && (ComboBox_TheaterSize.SelectedItem != null || ComboBox_TheaterSize.Text != ""))
            {
                string sid = TextBox_TheaterSearchid.Text;
                string size = ComboBox_TheaterSize.Text;
                int ssize = 0;
                if (size == "16")
                {
                    ssize = 16;
                }
                else if (size == "32")
                {
                    ssize = 32;
                }
                else if (size == "64")
                {
                    ssize = 64;
                }
                for (int i = 0; i < theaters.Count; i++)
                {
                    if (theaters[i].Tid == sid && theaters[i].Tsize == ssize)
                    {
                        TheaterList.Items.Add(theaters[i]);
                    }
                }
                TheaterList.Items.Refresh();
            }

            if (TextBox_TheaterSearchid.Text == "" && (ComboBox_TheaterType.SelectedItem != null || ComboBox_TheaterType.Text != "") && (ComboBox_TheaterSize.SelectedItem != null || ComboBox_TheaterSize.Text != ""))
            {
                string size = ComboBox_TheaterSize.Text;
                int ssize = 0;
                if (size == "16")
                {
                    ssize = 16;
                }
                else if (size == "32")
                {
                    ssize = 32;
                }
                else if (size == "64")
                {
                    ssize = 64;
                }
                string stype = ComboBox_TheaterType.Text;
                if (stype == "普通影厅")
                {
                    type = 0;
                }
                else if (stype == "VIP影厅")
                {
                    type = 1;
                }
                else if (stype == "SVIP影厅")
                {
                    type = 2;
                }
                for (int i = 0; i < theaters.Count; i++)
                {
                    if (theaters[i].Ttype == type && theaters[i].Tsize == ssize)
                    {
                        TheaterList.Items.Add(theaters[i]);
                    }
                }
                TheaterList.Items.Refresh();
            }

            if (TextBox_TheaterSearchid.Text != "" && (ComboBox_TheaterType.SelectedItem != null || ComboBox_TheaterType.Text != "") && (ComboBox_TheaterSize.SelectedItem != null || ComboBox_TheaterSize.Text != ""))
            {
                string sid = TextBox_TheaterSearchid.Text;
                string size = ComboBox_TheaterSize.Text;
                int ssize = 0;
                if (size == "16")
                {
                    ssize = 16;
                }
                else if (size == "32")
                {
                    ssize = 32;
                }
                else if (size == "64")
                {
                    ssize = 64;
                }
                string stype = ComboBox_TheaterType.Text;
                if (stype == "普通影厅")
                {
                    type = 0;
                }
                else if (stype == "VIP影厅")
                {
                    type = 1;
                }
                else if (stype == "SVIP影厅")
                {
                    type = 2;
                }
                for (int i = 0; i < theaters.Count; i++)
                {
                    if (theaters[i].Tid == sid && theaters[i].Ttype == type && theaters[i].Tsize == ssize)
                    {
                        TheaterList.Items.Add(theaters[i]);
                    }
                }
                TheaterList.Items.Refresh();
            }

        }

        private void TheaterList_Click(object sender, RoutedEventArgs e)
        {
            //获得点击的列
            GridViewColumn clickedColumn = (e.OriginalSource as GridViewColumnHeader).Column;
            if (clickedColumn.Header.ToString() == "影厅号")
            {
                TheaterList.Items.SortDescriptions.Add(new SortDescription("id", ListSortDirection.Ascending));
            }
        }

        private void TheaterAdd_Click(object sender, RoutedEventArgs e)
        {
            Hide();
            AddTheater add = new AddTheater(theaters,package);
            add.ShowDialog();
            if (add.Tag != null)
            {
                Theater model = add.Tag as Theater;
                byte stype = model.Ttype;
                if (stype == 0)
                {
                    model.TType = "普通影厅";
                }
                else if (stype == 1)
                {
                    model.TType = "VIP影厅";
                }
                else if (stype == 2)
                {
                    model.TType = "SVIP影厅";
                }
                if (model != null)
                {
                    theaters.Add(model);
                    TheaterList.Items.Add(model);
                    TheaterList.Items.Refresh();
                }
            }
            Show();

        }

        private void TheaterDelete_Click(object sender, RoutedEventArgs e)
        {
            if (TheaterList.SelectedItem == null)
            {
                MessageBox.Show("请选择删除项", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {

                //选中行数据
                Theater t = TheaterList.SelectedItem as Theater;
                bool flag = false;
                //MessageBox.Show("姓名：" + u.name + "\n\n" + "密码：" + u.password);
                for (int i = 0; i < onmovies.Count; i++)
                {
                    if (t.Tid == onmovies[i].Tid)
                    {
                        MessageBox.Show("无法删除！该影厅仍有关联场次"+ onmovies[i].Oid + "，请先删除关联场次！", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else
                        flag = true;
                }
                if (flag)
                {
                    //send 发送删除请求
                    handler.delTheaterRequest(t);
                    //删除成功
                    if (handler.delTheaterReply() == "删除成功")
                    {
                        MessageBox.Show("删除成功！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                        //删除选中元素
                        theaters.Remove(t);
                        TheaterList.Items.Remove(TheaterList.SelectedItem);
                    }
                    else
                    {
                        MessageBox.Show("删除失败！", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
                    }

                    //刷新listview
                    TheaterList.Items.Refresh();
                }
            }
        }

        private void MovieSearch_Click(object sender, RoutedEventArgs e)
        {
            MovieList.Items.Clear();
            if (TextBox_MovieSearchid.Text == "" && TextBox_MovieSearchname.Text == "")
            {
                for (int i = 0; i < movies.Count; i++)
                {
                    
                    MovieList.Items.Add(movies[i]);
                }
                MovieList.Items.Refresh();
            }
            if (TextBox_MovieSearchid.Text != "" && TextBox_MovieSearchname.Text == "")
            {
                string sid = TextBox_MovieSearchid.Text;

                for (int i = 0; i < movies.Count; i++)
                {
                    if (movies[i].Mid == sid)
                    {
                        
                        MovieList.Items.Add(movies[i]);
                    }
                }
                MovieList.Items.Refresh();
            }

            if (TextBox_MovieSearchid.Text == "" && TextBox_MovieSearchname.Text != "")
            {
                string sname = TextBox_MovieSearchname.Text;
                for (int i = 0; i < movies.Count; i++)
                {
                    if (movies[i].Mname == sname)
                    {
                        //MovieForShow m = new MovieForShow(movies[i].id, movies[i].name, movies[i].type, movies[i].time, movies[i].comment);
                        MovieList.Items.Add(movies[i]);
                    }
                }
                MovieList.Items.Refresh();
            }

            if (TextBox_MovieSearchid.Text != "" && TextBox_MovieSearchname.Text != "")
            {
                string sid = TextBox_MovieSearchid.Text;
                string sname = TextBox_MovieSearchname.Text;
                for (int i = 0; i < theaters.Count; i++)
                {
                    if (movies[i].Mid == sid && movies[i].Mname == sname)
                    {
                        //MovieForShow m = new MovieForShow(movies[i].id, movies[i].name, movies[i].type, movies[i].time, movies[i].comment);
                        MovieList.Items.Add(movies[i]);
                    }
                }
                MovieList.Items.Refresh();
            }
        }

        private void MovieList_Click(object sender, RoutedEventArgs e)
        {
            //获得点击的列
            GridViewColumn clickedColumn = (e.OriginalSource as GridViewColumnHeader).Column;
            if (clickedColumn.Header.ToString() == "影片号")
            {
                MovieList.Items.SortDescriptions.Add(new SortDescription("id", ListSortDirection.Ascending));
            }
        }

        private void MovieList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Movie select = MovieList.SelectedItem as Movie;
            //var select = txtBox.DataContext as Movie;
            //ListBox listBox = sender as ListBox;
            //if (listBox == null || listBox.SelectedItem == null)
            //{
            //    MessageBox.Show("ListBox1双击对象为空...");
            //}
            //else
            //{
            Movie tem = new Movie();
            for(int i = 0; i < movies.Count; i++)
            {
                if (movies[i].Mid == select.Mid)
                    tem = movies[i];
            }
            Hide();
            MovieInfo info = new MovieInfo(tem);
            info.ShowDialog();
            Show();
            //}
        }

        private void MovieAdd_Click(object sender, RoutedEventArgs e)
        {

            Hide();
            AddMovie add = new AddMovie(movies,package);
            add.ShowDialog();
            if (add.Tag != null)
            {
                Movie model = add.Tag as Movie;
                if (model != null)
                {
                    movies.Add(model);
                    MovieList.Items.Add(model);
                    MovieList.Items.Refresh();
                }
            }
            Show();

        }

        private void MovieDelete_Click(object sender, RoutedEventArgs e)
        {
            if (MovieList.SelectedItem == null)
            {
                MessageBox.Show("请选择删除项", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                //选中行数据
                Movie m = MovieList.SelectedItem as Movie;
                bool flag = false;
                //MessageBox.Show("姓名：" + u.name + "\n\n" + "密码：" + u.password);
                for (int i = 0; i < onmovies.Count; i++)
                {
                    if (m.Mid == onmovies[i].Mid)
                    {
                        MessageBox.Show("无法删除！该影片仍有关联场次" + onmovies[i].Oid + "，请先删除关联场次！", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else
                        flag = true;
                }
                if (flag)
                {
                    //send 发送删除请求
                    handler.delMovieRequest(m);
                    //删除成功
                    if (handler.delMovieReply() == "删除成功")
                    {
                        MessageBox.Show("删除成功！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                        //删除选中元素
                        movies.Remove(m);
                        MovieList.Items.Remove(MovieList.SelectedItem);
                    }
                    else
                    {
                        MessageBox.Show("删除失败！", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
                    }

                    //刷新listview
                    MovieList.Items.Refresh();
                }
            }
        }

        private void OnMovieSearch_Click(object sender, RoutedEventArgs e)
        {
            OnMovieList.Items.Clear();
            if (TextBox_OnMovieSearchoid.Text == "" && TextBox_OnMovieSearchmid.Text == "" && TextBox_OnMovieSearchpid.Text == "")
            {
                for (int i = 0; i < onmovies.Count; i++)
                {
                    OnMovieList.Items.Add(onmovies[i]);
                }
                OnMovieList.Items.Refresh();
            }
            if (TextBox_OnMovieSearchoid.Text != "" && TextBox_OnMovieSearchmid.Text == "" && TextBox_OnMovieSearchpid.Text == "")
            {
                string soid = TextBox_OnMovieSearchoid.Text;
               
                for (int i = 0; i < onmovies.Count; i++)
                {
                    if (onmovies[i].Oid == soid)
                    {
                        OnMovieList.Items.Add(onmovies[i]);
                    }
                }

                OnMovieList.Items.Refresh();
            }

            if (TextBox_OnMovieSearchoid.Text == "" && TextBox_OnMovieSearchmid.Text != "" && TextBox_OnMovieSearchpid.Text == "")
            {
                string smid = TextBox_OnMovieSearchmid.Text;
                
                for (int i = 0; i < onmovies.Count; i++)
                {
                    if (onmovies[i].Mid == smid)
                    {
                        OnMovieList.Items.Add(onmovies[i]);
                    }
                }
                OnMovieList.Items.Refresh();
            }

            if (TextBox_OnMovieSearchoid.Text == "" && TextBox_OnMovieSearchmid.Text == "" && TextBox_OnMovieSearchpid.Text != "")
            {
               
                string stid = TextBox_OnMovieSearchpid.Text;
                for (int i = 0; i < onmovies.Count; i++)
                {
                    if (onmovies[i].Tid == stid)
                    {
                        OnMovieList.Items.Add(onmovies[i]);
                    }
                }
                OnMovieList.Items.Refresh();
            }

            if (TextBox_OnMovieSearchoid.Text != "" && TextBox_OnMovieSearchmid.Text != "" && TextBox_OnMovieSearchpid.Text == "")
            {
                string soid = TextBox_OnMovieSearchoid.Text;
                string smid = TextBox_OnMovieSearchmid.Text;
                
                for (int i = 0; i < onmovies.Count; i++)
                {
                    if (onmovies[i].Oid == soid && onmovies[i].Mid == smid)
                    {
                        OnMovieList.Items.Add(onmovies[i]);
                    }
                }
                OnMovieList.Items.Refresh();
            }

            if (TextBox_OnMovieSearchoid.Text != "" && TextBox_OnMovieSearchmid.Text == "" && TextBox_OnMovieSearchpid.Text != "")
            {
                string soid = TextBox_OnMovieSearchoid.Text;
               
                string stid = TextBox_OnMovieSearchpid.Text;
                for (int i = 0; i < onmovies.Count; i++)
                {
                    if (onmovies[i].Oid == soid && onmovies[i].Tid == stid)
                    {
                        OnMovieList.Items.Add(onmovies[i]);
                    }
                }
                OnMovieList.Items.Refresh();
            }

            if (TextBox_OnMovieSearchoid.Text == "" && TextBox_OnMovieSearchmid.Text != "" && TextBox_OnMovieSearchpid.Text != "")
            {
                
                string smid = TextBox_OnMovieSearchmid.Text;
                string stid = TextBox_OnMovieSearchpid.Text;
                for (int i = 0; i < onmovies.Count; i++)
                {
                    if (onmovies[i].Mid == smid && onmovies[i].Tid == stid)
                    {
                        OnMovieList.Items.Add(onmovies[i]);
                    }
                }
                OnMovieList.Items.Refresh();
            }

            if (TextBox_OnMovieSearchoid.Text != "" && TextBox_OnMovieSearchmid.Text != "" && TextBox_OnMovieSearchpid.Text != "")
            {
                string soid = TextBox_OnMovieSearchoid.Text;
                string smid = TextBox_OnMovieSearchmid.Text;
                string stid = TextBox_OnMovieSearchpid.Text;
                for (int i = 0; i < onmovies.Count; i++)
                {
                    if (onmovies[i].Oid == soid && onmovies[i].Mid == smid && onmovies[i].Tid == stid)
                    {
                        OnMovieList.Items.Add(onmovies[i]);
                    }
                }
                OnMovieList.Items.Refresh();
            }
        }

        private void OnMovieList_Click(object sender, RoutedEventArgs e)
        {
            //获得点击的列
            GridViewColumn clickedColumn = (e.OriginalSource as GridViewColumnHeader).Column;
            if (clickedColumn.Header.ToString() == "场次号")
            {
                OnMovieList.Items.SortDescriptions.Add(new SortDescription("id", ListSortDirection.Ascending));
            }
        }

        private void OnMovieAdd_Click(object sender, RoutedEventArgs e)
        {
            Hide();
            AddOnMovie add = new AddOnMovie(onmovies,movies,theaters,package);
            add.ShowDialog();
            if (add.Tag != null)
            {
                OnMovie model = add.Tag as OnMovie;
                if (model != null)
                {
                    onmovies.Add(model);
                    OnMovieList.Items.Add(model);
                    OnMovieList.Items.Refresh();
                }
            }
            Show();

            
        }

        private void OnMovieDelete_Click(object sender, RoutedEventArgs e)
        {
            if (OnMovieList.SelectedItem == null)
            {
                MessageBox.Show("请选择删除项", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                //选中行数据
                OnMovie o = OnMovieList.SelectedItem as OnMovie;
                //MessageBox.Show("姓名：" + u.name + "\n\n" + "密码：" + u.password);

                //删除选中元素
                onmovies.Remove(o);
                OnMovieList.Items.Remove(OnMovieList.SelectedItem);

                //send 发送删除请求
                handler.delOnMovieRequest(o);
                //删除成功
                if (handler.delOnMovieReply() == "删除成功")
                {
                    MessageBox.Show("删除成功！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    //删除选中元素
                    onmovies.Remove(o);
                    OnMovieList.Items.Remove(OnMovieList.SelectedItem);
                }
                else
                {
                    MessageBox.Show("删除成功！", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
                    onmovies.Remove(o);
                    OnMovieList.Items.Remove(OnMovieList.SelectedItem);
                }

                //刷新listview
                OnMovieList.Items.Refresh();
            }
        }

        private void TicketSearch_Click(object sender, RoutedEventArgs e)
        {
            byte status = 0;
            TicketList.Items.Clear();
            if (TextBox_TicketSearchid.Text == "" && TextBox_TicketSearchoid.Text == "" && (ComboBox_TicketStatus.SelectedItem == null || ComboBox_TicketStatus.Text == ""))
            { 
                for (int i = 0; i < records.Count; i++)
                {
                    TicketList.Items.Add(records[i]);
                }
                TicketList.Items.Refresh();
            }
            if (TextBox_TicketSearchid.Text != "" && TextBox_TicketSearchoid.Text == "" && (ComboBox_TicketStatus.SelectedItem == null || ComboBox_TicketStatus.Text == ""))
            {
                string sid = TextBox_TicketSearchid.Text;
               
                for (int i = 0; i < records.Count; i++)
                {
                    if (records[i].Uid == sid)
                    {
                        TicketList.Items.Add(records[i]);
                    }
                }
                TicketList.Items.Refresh();
            }

            if (TextBox_TicketSearchid.Text == "" && TextBox_TicketSearchoid.Text != "" && (ComboBox_TicketStatus.SelectedItem == null || ComboBox_TicketStatus.Text == ""))
            {
                
                string oid = TextBox_TicketSearchoid.Text;
                
                for (int i = 0; i < records.Count; i++)
                {
                    if (records[i].Oid == oid)
                    {
                        TicketList.Items.Add(records[i]);
                    }
                }
                TicketList.Items.Refresh();
            }

            if (TextBox_TicketSearchid.Text == "" && TextBox_TicketSearchoid.Text == "" && (ComboBox_TicketStatus.SelectedItem != null || ComboBox_TicketStatus.Text != ""))
            {
                string sid = TextBox_TicketSearchid.Text;
                string oid = TextBox_TicketSearchoid.Text;
                string ssta = ComboBox_TicketStatus.Text;
                if (ssta == "等待支付")
                {
                    status = 0;
                }
                else if (ssta == "购票成功")
                {
                    status = 1;
                }
                else if (ssta == "购票失败")
                {
                    status = 2;
                }
                for (int i = 0; i < records.Count; i++)
                {
                    if (records[i].Rstatus == status)
                    {
                        TicketList.Items.Add(records[i]);
                    }
                }
                TicketList.Items.Refresh();
            }

            if (TextBox_TicketSearchid.Text != "" && TextBox_TicketSearchoid.Text != "" && (ComboBox_TicketStatus.SelectedItem == null || ComboBox_TicketStatus.Text == ""))
            {
                string sid = TextBox_TicketSearchid.Text;
                string oid = TextBox_TicketSearchoid.Text;
                
                for (int i = 0; i < records.Count; i++)
                {
                    if (records[i].Uid == sid && records[i].Oid == oid)
                    {
                        TicketList.Items.Add(records[i]);
                    }
                }
                TicketList.Items.Refresh();
            }

            if (TextBox_TicketSearchid.Text != "" && TextBox_TicketSearchoid.Text == "" && (ComboBox_TicketStatus.SelectedItem != null || ComboBox_TicketStatus.Text != ""))
            {
                string sid = TextBox_TicketSearchid.Text;
                
                string ssta = ComboBox_TicketStatus.Text;
                for (int i = 0; i < records.Count; i++)
                {
                    if (records[i].Uid == sid && records[i].Rstatus == status)
                    {
                        TicketList.Items.Add(records[i]);
                    }
                }
                TicketList.Items.Refresh();
            }

            if (TextBox_TicketSearchid.Text == "" && TextBox_TicketSearchoid.Text != "" && (ComboBox_TicketStatus.SelectedItem != null || ComboBox_TicketStatus.Text != ""))
            {
                string sid = TextBox_TicketSearchid.Text;
                string oid = TextBox_TicketSearchoid.Text;
                string ssta = ComboBox_TicketStatus.Text;
                for (int i = 0; i < records.Count; i++)
                {
                    if (records[i].Oid == oid && records[i].Rstatus == status)
                    {
                        TicketList.Items.Add(records[i]);
                    }
                }
                TicketList.Items.Refresh();
            }

            if (TextBox_TicketSearchid.Text != "" && TextBox_TicketSearchoid.Text != "" && (ComboBox_TicketStatus.SelectedItem != null || ComboBox_TicketStatus.Text != ""))
            {
                string sid = TextBox_TicketSearchid.Text;
                string oid = TextBox_TicketSearchoid.Text;
                string ssta = ComboBox_TicketStatus.Text;
                for (int i = 0; i < records.Count; i++)
                {
                    if (records[i].Uid == sid && records[i].Oid == oid && records[i].Rstatus == status)
                    {
                        TicketList.Items.Add(records[i]);
                    }
                }
                TicketList.Items.Refresh();
            }

        }

        private void TicketList_Click(object sender, RoutedEventArgs e)
        {
            //获得点击的列
            GridViewColumn clickedColumn = (e.OriginalSource as GridViewColumnHeader).Column;
            if (clickedColumn.Header.ToString() == "用户号")
            {
                OnMovieList.Items.SortDescriptions.Add(new SortDescription("uid", ListSortDirection.Ascending));
            }
            if (clickedColumn.Header.ToString() == "影片号")
            {
                OnMovieList.Items.SortDescriptions.Add(new SortDescription("oid", ListSortDirection.Ascending));
            }
            if (clickedColumn.Header.ToString() == "场次号")
            {
                OnMovieList.Items.SortDescriptions.Add(new SortDescription("sid", ListSortDirection.Ascending));
            }
        }

        private void TicketUserSta_Click(object sender, RoutedEventArgs e)
        {
            if (TicketList.SelectedItem != null)
            {
                Hide();
                Record r = TicketList.SelectedItem as Record;
                new UserSta(r.Uid, records).ShowDialog();
                Show();
            }
            else
            {
                MessageBox.Show("未选择用户，无法统计");
            }
        }

        private void TicketOnMovieSta_Click(object sender, RoutedEventArgs e)
        {
            if (TicketList.SelectedItem != null)
            {
                Hide();
                Record r = TicketList.SelectedItem as Record;
                new OnMovieSta(r.Oid, records).ShowDialog();
                Show();
            }
            else
            {
                MessageBox.Show("未选择场次，无法统计");
            }
        }

        private void PackageSearch_Click(object sender, RoutedEventArgs e)
        {
            PackageList.Items.Clear();
            if (TextBox_PackageSearchSid.Text == "" && TextBox_PackageSearchDid.Text == "")
            {
                for (int i = 0; i < trans.Count; i++)
                {
                    PackageList.Items.Add(trans[i]);
                }
                PackageList.Items.Refresh();
            }

            if (TextBox_PackageSearchSid.Text != "" && TextBox_PackageSearchDid.Text == "")
            {
                for (int i = 0; i < trans.Count; i++)
                {
                    if(Encoding.UTF8.GetString(trans[i].fromAddress) == TextBox_PackageSearchSid.Text)
                    {
                        PackageList.Items.Add(trans[i]);
                    }
            
                }
                PackageList.Items.Refresh();
            }

            if (TextBox_PackageSearchSid.Text == "" && TextBox_PackageSearchDid.Text != "")
            {
                for (int i = 0; i < trans.Count; i++)
                {
                    if (Encoding.UTF8.GetString(trans[i].toAddress) == TextBox_PackageSearchDid.Text)
                    {
                        PackageList.Items.Add(trans[i]);
                    }
                }
                PackageList.Items.Refresh();
            }

            if (TextBox_PackageSearchSid.Text != "" && TextBox_PackageSearchDid.Text != "")
            {
                for (int i = 0; i < records.Count; i++)
                {
                    if (Encoding.UTF8.GetString(trans[i].fromAddress) == TextBox_PackageSearchSid.Text && Encoding.UTF8.GetString(trans[i].toAddress) == TextBox_PackageSearchDid.Text)
                    {
                        PackageList.Items.Add(trans[i]);
                    }
                  
                }
                PackageList.Items.Refresh();
            }
        }

        private void PackageList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

            TransMessage select = PackageList.SelectedItem as TransMessage;
            //var select = txtBox.DataContext as TransMessage;
            //ListBox listBox = sender as ListBox;
            //if (listBox == null || listBox.SelectedItem == null)
            //{
            //    MessageBox.Show("ListBox1双击对象为空...");
            //}
            //else
            //{
            Hide();
            PackageInfo info = new PackageInfo(select);
            info.ShowDialog();
            //}
        }
        
        public void CatchPackage()
        {

        }

       
    }
}
