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

namespace 服务器UI
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        //private User user;
        //private bool isEyeOpen;
        //private bool modNameSure;
        //private bool modPwdSure;
        private DispatcherTimer showTimer;
        private static List<User> users = new List<User>();
        private static List<Movie> movies = new List<Movie>();
        private static List<Theater> theaters = new List<Theater>();
        private static List<OnMovie> onmovies = new List<OnMovie>();
        private static List<Record> records = new List<Record>();

        public MainWindow()
        {
            InitializeComponent();
            InitTextBlock_Time();
            InitLists();
            InitTextBlock_Hello();
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

            User u1 = new User("1", "dx", "123", "普通用户", 100);
            User u2 = new User("3", "xz", "123", "VIP用户", 100);
            User u3 = new User("2", "wbc", "123", "SVIP用户", 100);
            User u4 = new User("4", "zr", "123", "普通管理员", 100);
            Theater t1 = new Theater("1", "1", 30);
            Theater t2 = new Theater("2", "1", 10);
            Movie m1 = new Movie("1", "x", "1", 60, 30, "1", "1");
            Movie m2 = new Movie("2", "xz", "1", 60, 30, "1", "1");
            OnMovie o1 = new OnMovie("1", "1", "1", DateTime.Now, DateTime.Now.AddMinutes(60), 90);
            OnMovie o2 = new OnMovie("2", "2", "2", DateTime.Now, DateTime.Now, 90);
            Record r1 = new Record("1", "1", "1", DateTime.Now, 80, "1");
            Record r2 = new Record("2", "2", "2", DateTime.Now, 90, "1");
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
            TicketList.Items.Add(records[1]);
        }

        private void InitTextBlock_Hello()
        {
            TextBlock_Hello.Text = "欢迎您！";
            /*switch (user.access)
            {
                case "01":
                    {
                        Image_vip.Opacity = 1;
                        TextBlock_Hello.Text += "VIP用户：";
                        break;
                    }
                case "02":
                    {
                        Image_svip.Opacity = 1;
                        TextBlock_Hello.Text += "SVIP用户：";
                        break;
                    }
                default:
                    {
                        Image_common.Opacity = 1;
                        TextBlock_Hello.Text += "普通用户：";
                        break;
                    }
            }
            TextBlock_Hello.Text += user.name;
            */
        }

        /*private void InitPersonalInfo()
        {
            TextBox_Id.Text = user.id;
            TextBox_Name.Text = user.name;
            string temp = "";
            for (int i = 0; i < user.password.Length; i++)
                temp += "*";
            TextBox_Pwd.Text = temp;
            TextBox_Money.Text = user.money + "元";
            switch (user.access)
            {
                case "01":
                    TextBox_Access.Text = "VIP用户";
                    break;
                case "02":
                    TextBox_Access.Text = "SVIP用户";
                    break;
                default:
                    TextBox_Access.Text = "普通用户";
                    break;
            }
        }*/

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
                    if(users[i].id == sid)
                    {
                        UserList.Items.Add(users[i]);
                    }
                }
                UserList.Items.Refresh();
            }

            if (TextBox_UserSearchid.Text == "" && (ComboBox_Access.SelectedItem != null || ComboBox_Access.Text != ""))
            {
                string sacc = ComboBox_Access.Text;
                for (int i = 0; i < users.Count; i++)
                {
                    if (users[i].access == sacc)
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
                for (int i = 0; i < users.Count; i++)
                {
                    if (users[i].id == sid && users[i].access == sacc)
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
            AddUser add = new AddUser(users);
            add.ShowDialog();
            if (add.Tag != null)
            {
                User model = add.Tag as User;
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
            //选中行数据
            User u = UserList.SelectedItem as User;
            //MessageBox.Show("姓名：" + u.name + "\n\n" + "密码：" + u.password);

            //删除选中元素
            users.Remove(u);
            UserList.Items.Remove(UserList.SelectedItem);
           
            //send 发送删除请求
            //删除成功

            //刷新listview
            UserList.Items.Refresh();
        }

        private void TheaterSearch_Click(object sender, RoutedEventArgs e)
        {
            TheaterList.Items.Clear();
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
                int ssize = int.Parse(ComboBox_TheaterSize.Text);
                for (int i = 0; i < theaters.Count; i++)
                {
                    if (theaters[i].size == ssize)
                    {
                        TheaterList.Items.Add(theaters[i]);
                    }
                }
                TheaterList.Items.Refresh();
            }

            if (TextBox_TheaterSearchid.Text == "" && (ComboBox_TheaterType.SelectedItem != null || ComboBox_TheaterType.Text != "") && (ComboBox_TheaterSize.SelectedItem == null || ComboBox_TheaterSize.Text == ""))
            { 
                string stype = ComboBox_TheaterType.Text;
                for (int i = 0; i < theaters.Count; i++)
                {
                    if (theaters[i].type == stype)
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
                    if (theaters[i].id == sid)
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
                for (int i = 0; i < theaters.Count; i++)
                {
                    if (theaters[i].id == sid && theaters[i].type == stype)
                    {
                        TheaterList.Items.Add(theaters[i]);
                    }
                }
                TheaterList.Items.Refresh();
            }

            if (TextBox_TheaterSearchid.Text != "" && (ComboBox_TheaterType.SelectedItem == null || ComboBox_TheaterType.Text == "") && (ComboBox_TheaterSize.SelectedItem != null || ComboBox_TheaterSize.Text != ""))
            {
                string sid = TextBox_TheaterSearchid.Text;
                int ssize = int.Parse(ComboBox_TheaterSize.Text);
                
                for (int i = 0; i < theaters.Count; i++)
                {
                    if (theaters[i].id == sid && theaters[i].size == ssize)
                    {
                        TheaterList.Items.Add(theaters[i]);
                    }
                }
                TheaterList.Items.Refresh();
            }

            if (TextBox_TheaterSearchid.Text == "" && (ComboBox_TheaterType.SelectedItem != null || ComboBox_TheaterType.Text != "") && (ComboBox_TheaterSize.SelectedItem != null || ComboBox_TheaterSize.Text != ""))
            {
                
                int ssize = int.Parse(ComboBox_TheaterSize.Text);
                string stype = ComboBox_TheaterType.Text;
                for (int i = 0; i < theaters.Count; i++)
                {
                    if (theaters[i].type == stype && theaters[i].size == ssize)
                    {
                        TheaterList.Items.Add(theaters[i]);
                    }
                }
                TheaterList.Items.Refresh();
            }

            if (TextBox_TheaterSearchid.Text != "" && (ComboBox_TheaterType.SelectedItem != null || ComboBox_TheaterType.Text != "") && (ComboBox_TheaterSize.SelectedItem != null || ComboBox_TheaterSize.Text != ""))
            {
                string sid = TextBox_TheaterSearchid.Text;
                int ssize = int.Parse(ComboBox_TheaterSize.Text);
                string stype = ComboBox_TheaterType.Text;
                for (int i = 0; i < theaters.Count; i++)
                {
                    if (theaters[i].id == sid && theaters[i].type == stype && theaters[i].size == ssize)
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
            AddTheater add = new AddTheater(theaters);
            add.ShowDialog();
            if (add.Tag != null)
            {
                Theater model = add.Tag as Theater;
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
            //选中行数据
            Theater t = TheaterList.SelectedItem as Theater;
            //MessageBox.Show("姓名：" + u.name + "\n\n" + "密码：" + u.password);

            //删除选中元素
            theaters.Remove(t);
            TheaterList.Items.Remove(TheaterList.SelectedItem);

            //send 发送删除请求
            //删除成功

            //刷新listview
            TheaterList.Items.Refresh();
        }

        private void MovieSearch_Click(object sender, RoutedEventArgs e)
        {
            MovieList.Items.Clear();
            if (TextBox_MovieSearchid.Text == "" && TextBox_MovieSearchname.Text == "")
            {
                for (int i = 0; i < movies.Count; i++)
                {
                    MovieForShow m = new MovieForShow(movies[i].id, movies[i].name, movies[i].type, movies[i].time, movies[i].comment);
                    MovieList.Items.Add(m);
                }
                MovieList.Items.Refresh();
            }
            if (TextBox_MovieSearchid.Text != "" && TextBox_MovieSearchname.Text == "")
            {
                string sid = TextBox_MovieSearchid.Text;

                for (int i = 0; i < movies.Count; i++)
                {
                    if (movies[i].id == sid)
                    {
                        MovieForShow m = new MovieForShow(movies[i].id, movies[i].name, movies[i].type, movies[i].time, movies[i].comment);
                        MovieList.Items.Add(m);
                    }
                }
                MovieList.Items.Refresh();
            }

            if (TextBox_MovieSearchid.Text == "" && TextBox_MovieSearchname.Text != "")
            {
                string sname = TextBox_MovieSearchname.Text;
                for (int i = 0; i < movies.Count; i++)
                {
                    if (movies[i].name == sname)
                    {
                        MovieForShow m = new MovieForShow(movies[i].id, movies[i].name, movies[i].type, movies[i].time, movies[i].comment);
                        MovieList.Items.Add(m);
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
                    if (movies[i].id == sid && movies[i].name == sname)
                    {
                        MovieForShow m = new MovieForShow(movies[i].id, movies[i].name, movies[i].type, movies[i].time, movies[i].comment);
                        MovieList.Items.Add(m);
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
                Hide();
                MovieInfo info = new MovieInfo(select);
                info.ShowDialog();
            //}
        }

        private void MovieAdd_Click(object sender, RoutedEventArgs e)
        {

            Hide();
            AddMovie add = new AddMovie(movies);
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
            //选中行数据
            Movie m = MovieList.SelectedItem as Movie;
            //MessageBox.Show("姓名：" + u.name + "\n\n" + "密码：" + u.password);

            //删除选中元素
            movies.Remove(m);
            MovieList.Items.Remove(MovieList.SelectedItem);

            //send 发送删除请求
            //删除成功

            //刷新listview
            MovieList.Items.Refresh();
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
                    if (onmovies[i].oid == soid)
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
                    if (onmovies[i].mid == smid)
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
                    if (onmovies[i].tid == stid)
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
                    if (onmovies[i].oid == soid && onmovies[i].mid == smid)
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
                    if (onmovies[i].oid == soid && onmovies[i].tid == stid)
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
                    if (onmovies[i].mid == smid && onmovies[i].tid == stid)
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
                    if (onmovies[i].oid == soid && onmovies[i].mid == smid && onmovies[i].tid == stid)
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
            AddOnMovie add = new AddOnMovie(onmovies,movies);
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
            //选中行数据
            OnMovie o = OnMovieList.SelectedItem as OnMovie;
            //MessageBox.Show("姓名：" + u.name + "\n\n" + "密码：" + u.password);

            //删除选中元素
            onmovies.Remove(o);
            OnMovieList.Items.Remove(OnMovieList.SelectedItem);

            //send 发送删除请求
            //删除成功

            //刷新listview
            OnMovieList.Items.Refresh();
        }

        private void TicketSearch_Click(object sender, RoutedEventArgs e)
        {
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
                    if (records[i].uid == sid)
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
                    if (records[i].oid == oid)
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
                for (int i = 0; i < records.Count; i++)
                {
                    if (records[i].status == ssta)
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
                    if (records[i].uid == sid && records[i].oid == oid)
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
                    if (records[i].uid == sid && records[i].status == ssta)
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
                    if (records[i].oid == oid && records[i].status == ssta)
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
                    if (records[i].uid == sid && records[i].oid == oid && records[i].status == ssta)
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
                new UserSta(r.uid, records).ShowDialog();
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
                new OnMovieSta(r.oid, records).ShowDialog();
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
                for (int i = 0; i < records.Count; i++)
                {
                    //PackageList.Items.Add(records[i]);
                }
                PackageList.Items.Refresh();
            }

            if (TextBox_PackageSearchSid.Text != "" && TextBox_PackageSearchDid.Text == "")
            {
                for (int i = 0; i < records.Count; i++)
                {
                    //PackageList.Items.Add(records[i]);
                }
                PackageList.Items.Refresh();
            }

            if (TextBox_PackageSearchSid.Text == "" && TextBox_PackageSearchDid.Text != "")
            {
                for (int i = 0; i < records.Count; i++)
                {
                    //PackageList.Items.Add(records[i]);
                }
                PackageList.Items.Refresh();
            }

            if (TextBox_PackageSearchSid.Text != "" && TextBox_PackageSearchDid.Text != "")
            {
                for (int i = 0; i < records.Count; i++)
                {
                    //PackageList.Items.Add(records[i]);
                }
                PackageList.Items.Refresh();
            }
        }

        private void PackageList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

            //Movie select = MovieList.SelectedItem as Movie;
            //var select = txtBox.DataContext as Movie;
            //ListBox listBox = sender as ListBox;
            //if (listBox == null || listBox.SelectedItem == null)
            //{
            //    MessageBox.Show("ListBox1双击对象为空...");
            //}
            //else
            //{
            Hide();
            PackageInfo info = new PackageInfo();
            info.ShowDialog();
            //}
        }
    }
}
