using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using System.Windows.Media.Imaging;
using System.Windows.Controls;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;

namespace CommonUser
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private User user;
        private List<Movie> movies = new List<Movie>();
        private bool isEyeOpen;
        private bool modNameSure;
        private bool modPwdSure;
        private DispatcherTimer showTimer;
        public MainWindow(User user)
        {
            this.user = user;
            InitializeComponent();
            InitTextBlock_Time();
            InitTextBlock_Hello();
            InitPersonalInfo();
            InitMovies();
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

        private void InitTextBlock_Hello()
        {
            TextBlock_Hello.Text = "欢迎您！";
            switch (user.access)
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
        }

        private void InitPersonalInfo()
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
        }

        private void InitMovies()
        {
            Movie temp = null;
            temp = ReadMovie("..\\..\\FileInfo\\M00001.txt");
            movies.Add(temp);
            temp = ReadMovie("..\\..\\FileInfo\\M00002.txt");
            movies.Add(temp);
            temp = ReadMovie("..\\..\\FileInfo\\M00003.txt");
            movies.Add(temp);
            temp = ReadMovie("..\\..\\FileInfo\\M00004.txt");
            movies.Add(temp);
            temp = ReadMovie("..\\..\\FileInfo\\M00005.txt");
            movies.Add(temp);
            temp = ReadMovie("..\\..\\FileInfo\\M00006.txt");
            movies.Add(temp);
            temp = ReadMovie("..\\..\\FileInfo\\M00007.txt");
            movies.Add(temp);
            temp = ReadMovie("..\\..\\FileInfo\\M00008.txt");
            movies.Add(temp);
            ListView_Movies.DataContext = movies;
        }

        private Movie ReadMovie(string fileName)
        {
            Movie movie = new Movie();
            try
            {
                using (StreamReader sr = new StreamReader(File.OpenRead(fileName)))
                {
                    movie.id = sr.ReadLine();
                    movie.name = sr.ReadLine();
                    movie.type = sr.ReadLine();
                    movie.time = int.Parse(sr.ReadLine());
                    movie.comment = float.Parse(sr.ReadLine());
                    movie.picture = "MoviePictures\\" + movie.id + ".jpg";
                    movie.description = sr.ReadLine();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return movie;
        }

        private void X_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Cursor = Cursors.Hand;
        }

        private void X_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Cursor = Cursors.Arrow;
        }

        private void Button_ModName_Click(object sender, RoutedEventArgs e)
        {
            if (modNameSure)
            {
                modNameSure = false;
                Image_NameTrue.Opacity = 0;
                Image_NameFalse.Opacity = 0;
                TextBox_Name.IsReadOnly = true;
                Button_ModName.Content = "修改用户名";
                Button_ModPwd.Opacity = 1;
                Button_ModPwd.IsEnabled = true;
                Button_ModAccess.Opacity = 1;
                Button_ModAccess.IsEnabled = true;
                Button_ModMoney.Opacity = 1;
                Button_ModMoney.IsEnabled = true;
                Button_Cancel.Opacity = 0;
                Button_Cancel.IsEnabled = false;
                /*发送修改请求*/
                /*更新页面*/
                InitTextBlock_Hello();
            }
            else
            {
                modNameSure = true;
                Image_NameTrue.Opacity = 1;
                TextBox_Name.IsReadOnly = false;
                TextBox_Name.Focus();
                TextBox_Name.SelectAll();
                Button_ModName.Content = "确定";
                Button_ModPwd.Opacity = 0;
                Button_ModPwd.IsEnabled = false;
                Button_ModAccess.Opacity = 0;
                Button_ModAccess.IsEnabled = false;
                Button_ModMoney.Opacity = 0;
                Button_ModMoney.IsEnabled = false;
                Button_Cancel.Opacity = 1;
                Button_Cancel.IsEnabled = true;
            }
        }

        private void Button_ModPwd_Click(object sender, RoutedEventArgs e)
        {
            if (modPwdSure)
            {
                modPwdSure = false;
                Image_Eye.Opacity = 1;
                Image_PwdTrue.Opacity = 0;
                Image_PwdFalse.Opacity = 0;
                TextBox_Pwd.IsReadOnly = true;
                Button_ModPwd.Content = "修改密码";
                Button_ModName.Opacity = 1;
                Button_ModName.IsEnabled = true;
                Button_ModAccess.Opacity = 1;
                Button_ModAccess.IsEnabled = true;
                Button_ModMoney.Opacity = 1;
                Button_ModMoney.IsEnabled = true;
                Button_Cancel.Opacity = 0;
                Button_Cancel.IsEnabled = false;
                /*发送修改请求*/
                /*更新页面*/
                string temp = "";
                for (int i = 0; i < TextBox_Pwd.Text.Length; i++)
                    temp += "*";
                TextBox_Pwd.Text = temp;
            }
            else
            {
                modPwdSure = true;
                Image_PwdTrue.Opacity = 1;
                Image_Eye.Opacity = 0;
                TextBox_Pwd.Text = user.password;
                TextBox_Pwd.IsReadOnly = false;
                TextBox_Pwd.Focus();
                TextBox_Pwd.SelectAll();
                Button_ModPwd.Content = "确定";
                Button_ModName.Opacity = 0;
                Button_ModName.IsEnabled = false;
                Button_ModAccess.Opacity = 0;
                Button_ModAccess.IsEnabled = false;
                Button_ModMoney.Opacity = 0;
                Button_ModMoney.IsEnabled = false;
                Button_Cancel.Opacity = 1;
                Button_Cancel.IsEnabled = true;
            }
        }

        private void Button_ModAccess_Click(object sender, RoutedEventArgs e)
        {
            Hide();
            new UpgradeWindow(user).ShowDialog();
            Show();
        }

        private void Button_ModMoney_Click(object sender, RoutedEventArgs e)
        {
            Hide();
            new RechargeWindow(user).ShowDialog();
            Show();
        }

        private void Button_Cancel_Click(object sender, RoutedEventArgs e)
        {
            if(modNameSure)
            {
                modNameSure = false;
                Image_NameTrue.Opacity = 0;
                Image_NameFalse.Opacity = 0;
                TextBox_Name.IsReadOnly = true;
                Button_ModName.Content = "修改用户名";
                Button_ModPwd.Opacity = 1;
                Button_ModPwd.IsEnabled = true;
                Button_ModAccess.Opacity = 1;
                Button_ModAccess.IsEnabled = true;
                Button_ModMoney.Opacity = 1;
                Button_ModMoney.IsEnabled = true;
                Button_Cancel.Opacity = 0;
                Button_Cancel.IsEnabled = false;
                TextBox_Name.Text = user.name;
            }
            else if(modPwdSure)
            {
                modPwdSure = false;
                Image_Eye.Opacity = 1;
                Image_PwdTrue.Opacity = 0;
                Image_PwdFalse.Opacity = 0;
                TextBox_Pwd.IsReadOnly = true;
                Button_ModPwd.Content = "修改密码";
                Button_ModName.Opacity = 1;
                Button_ModName.IsEnabled = true;
                Button_ModAccess.Opacity = 1;
                Button_ModAccess.IsEnabled = true;
                Button_ModMoney.Opacity = 1;
                Button_ModMoney.IsEnabled = true;
                Button_Cancel.Opacity = 0;
                Button_Cancel.IsEnabled = false;
                string temp = "";
                for (int i = 0; i < user.password.Length; i++)
                    temp += "*";
                TextBox_Pwd.Text = temp;
            }
        }

        private void ChangeImageSource(Image image, string path, bool isRelative)
        {
            BitmapImage bmp = new BitmapImage();
            bmp.BeginInit();
            if (isRelative)
                bmp.UriSource = new Uri(path, UriKind.Relative);
            else
                bmp.UriSource = new Uri(path, UriKind.Absolute);
            bmp.EndInit();
            image.Source = bmp;
        }

        private void Image_Pwd(object sender, MouseButtonEventArgs e)
        {
            if (isEyeOpen)
            {
                isEyeOpen = false;
                ChangeImageSource(Image_Eye, "ImageResources\\图标(黑)_闭眼.png", true);
                string temp = "";
                for (int i = 0; i < user.password.Length; i++)
                    temp += "*";
                TextBox_Pwd.Text = temp;
            }
            else
            {
                isEyeOpen = true;
                ChangeImageSource(Image_Eye, "ImageResources\\图标(黑)_眼睛.png", true);
                TextBox_Pwd.Text = user.password;
            }
        }

        private void TextBox_Name_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(modNameSure)
            {
                Image_NameTrue.Opacity = 1;
                if (TextBox_Name.Text.Length > 20 || TextBox_Name.Text.Length == 0)
                {
                    Image_NameTrue.Opacity = 0;
                    Image_NameFalse.Opacity = 1;
                }
                else
                {
                    Image_NameTrue.Opacity = 1;
                    Image_NameFalse.Opacity = 0;
                }
            }
            else
            {
                Image_NameTrue.Opacity = 0;
                Image_NameFalse.Opacity = 0;
            }
        }

        private void TextBox_Pwd_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (modPwdSure)
            {
                Image_PwdTrue.Opacity = 1;
                if (TextBox_Pwd.Text.Length > 20 || TextBox_Pwd.Text.Length == 0 || HasChinese(TextBox_Pwd.Text))
                {
                    Image_PwdTrue.Opacity = 0;
                    Image_PwdFalse.Opacity = 1;
                }
                else
                {
                    Image_PwdTrue.Opacity = 1;
                    Image_PwdFalse.Opacity = 0;
                }
            }
            else
            {
                Image_PwdTrue.Opacity = 0;
                Image_PwdFalse.Opacity = 0;
            }
        }

        private static bool HasChinese(string str)
        {
            return Regex.IsMatch(str, @"[\u4e00-\u9fa5]");
        }

        private void Button_Search_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Commit_Click(object sender, RoutedEventArgs e)
        {
            if(TextBox_Feedback.Text.Equals(""))
            {
                MessageBox.Show("请您输入反馈意见内容！", "输入错误");
                return;
            }
            else
            {
                /*发送请求*/
                MessageBox.Show("感谢您的反馈！", "提示");
                TextBox_Feedback.Text = null;
            }
        }

        private void Button_Flush_Click(object sender, RoutedEventArgs e)
        {
            TextBox_Feedback.Text = null;
        }

        private void Button_Buy_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            Movie movie = button.DataContext as Movie;
            MessageBox.Show(movie.id);
        }

        private void MouseDown_GetMoreMovieInfo(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                new MovieInfoWindow().Show();
            }
        }
    }
}