using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using System.Windows.Media.Imaging;
using System.Windows.Controls;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using CommonUser.Entity;
using CommonUser.AppServices;

namespace CommonUser
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private string uid;
        private User user;
        private List<Movie> movies;
        private List<Record> records;
        private bool isEyeOpen;
        private bool modNameSure;
        private bool modPwdSure;
        private CUVHandler handler;
        public MainWindow(string uid)
        {
            this.uid = uid;
            handler = CUVHandler.GetInstance();
            InitializeComponent();
            SetPersonTabInfo();
            SetMovieTabInfo();
            SetTimeInfo();
            SetHelloInfo();
        }

        //设置时间信息
        private void SetTimeInfo()
        {
            DispatcherTimer  showTimer = new System.Windows.Threading.DispatcherTimer();
            showTimer.Tick += new EventHandler
                (
                    (object sender, EventArgs e) =>
                        TextBlock_Time.Text = DateTime.Now.ToString("yyyy/MM/dd dddd HH:mm:ss")
                );
            showTimer.Interval = new TimeSpan(0, 0, 0, 1, 0);
            showTimer.Start();
        }

        //设置欢迎信息
        private void SetHelloInfo()
        {
            Image_common.Opacity = 0;
            Image_vip.Opacity = 0;
            Image_svip.Opacity = 0;
            TextBlock_Hello.Text = "欢迎您！";
            switch (user.Uaccess)
            {
                case EnumUserAccess.U_VIP:
                    {
                        Image_vip.Opacity = 1;
                        TextBlock_Hello.Text += "VIP用户：";
                        break;
                    }
                case EnumUserAccess.U_SVIP:
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
            TextBlock_Hello.Text += user.Uname;
        }

        private void SetPersonTabInfo()
        {
            user = handler.GetUser(uid);
            TextBox_Id.Text = user.Uid;
            TextBox_Name.Text = user.Uname;
            string temp = string.Empty;
            for (int i = 0; i < user.Upassword.Length; i++)
                temp += "*";
            TextBox_Pwd.Text = temp;
            TextBox_Money.Text = user.Umoney + "元";
            switch (user.Uaccess)
            {
                case EnumUserAccess.U_VIP:
                    TextBox_Access.Text = "VIP用户";
                    break;
                case EnumUserAccess.U_SVIP:
                    TextBox_Access.Text = "SVIP用户";
                    break;
                default:
                    TextBox_Access.Text = "普通用户";
                    break;
            }
        }

        private void SetMovieTabInfo()
        {
            movies = handler.GetMovies();
            handler.GetMoviePictures();
            ListView_Movies.DataContext = movies;
            ListView_Movies.Items.Refresh();
        }

        private void SetRecordTabInfo()
        {
            records = handler.GetRecords(user.Uid);
            ListView_Records.ItemsSource = records;
            ListView_Movies.Items.Refresh();
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
                if (TextBox_Name.Text.Length > 20)
                {
                    MessageBox.Show("用户名长度不得超过20位！", "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if (handler.ModifyName(TextBox_Id.Text, TextBox_Name.Text))
                {
                    MessageBox.Show("修改成功！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    user.Uname = TextBox_Name.Text;
                    SetPersonTabInfo();
                    SetHelloInfo();
                    return;
                }
                else
                {
                    MessageBox.Show("修改失败！", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
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
                if (TextBox_Pwd.Text.Length > 20)
                {
                    MessageBox.Show("密码长度不得超过20位！", "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if (handler.ModifyPassword(TextBox_Id.Text, TextBox_Pwd.Text))
                {
                    MessageBox.Show("修改成功！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    user.Upassword = TextBox_Pwd.Text;
                    SetPersonTabInfo();
                    return;
                }
                else
                {
                    MessageBox.Show("修改失败！", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }
            else
            {
                modPwdSure = true;
                Image_PwdTrue.Opacity = 1;
                Image_Eye.Opacity = 0;
                TextBox_Pwd.Text = user.Upassword;
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
            SetPersonTabInfo();
            SetHelloInfo();
            Show();
        }

        private void Button_ModMoney_Click(object sender, RoutedEventArgs e)
        {
            Hide();
            new RechargeWindow(user).ShowDialog();
            SetPersonTabInfo();
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
                TextBox_Name.Text = user.Uname;
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
                for (int i = 0; i < user.Upassword.Length; i++)
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
                for (int i = 0; i < user.Upassword.Length; i++)
                    temp += "*";
                TextBox_Pwd.Text = temp;
            }
            else
            {
                isEyeOpen = true;
                ChangeImageSource(Image_Eye, "ImageResources\\图标(黑)_眼睛.png", true);
                TextBox_Pwd.Text = user.Upassword;
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
                if (TextBox_Pwd.Text.Length > 20 || TextBox_Pwd.Text.Length == 0 || Regex.IsMatch(TextBox_Pwd.Text, @"[\u4e00-\u9fa5]"))
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

        private void Button_Search_Click(object sender, RoutedEventArgs e)
        {

            if (string.Empty.Equals(TextBox_Search.Text))
            {
                ListView_Movies.DataContext = movies;
                ListView_Movies.Items.Refresh();
                return;
            }
            List<Movie> tempList = new List<Movie>();
            foreach (Movie temp in movies)
            {
                if (temp.Mname.Contains(TextBox_Search.Text) || temp.Mtype.Contains(TextBox_Search.Text))
                    tempList.Add(temp);
            }
            ListView_Movies.DataContext = tempList;
            ListView_Movies.Items.Refresh();
        }

        private void Button_Buy_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            Movie movie = button.DataContext as Movie;
            new SelectOnMovieWindow(user,movie).Show(); 
        }

        private void MouseDown_GetMoreMovieInfo(object sender, MouseButtonEventArgs e)
        {
            Movie movie = ListView_Movies.SelectedItem as Movie;
            if (e.ClickCount == 2)
            {
                new MovieInfoWindow(user,movie).Show();
            }
        }

        private void Button_RefreshMList_Click(object sender, RoutedEventArgs e)
        {
            ListView_Movies.DataContext = null;
            ListView_Movies.Items.Refresh();
            SetMovieTabInfo();
        }

        private void Button_RefreshRList_Click(object sender, RoutedEventArgs e)
        {
            ListView_Records.DataContext = null;
            ListView_Records.Items.Refresh();
            SetRecordTabInfo();
        }

        private void Button_Refund_Click(object sender, RoutedEventArgs e)
		{
			Record u = ListView_Records.SelectedItem as Record;
			DateTime dt1 = Convert.ToDateTime(u.Rtime);
			DateTime dt2 = DateTime.Now;
			if(DateTime.Compare(dt1,dt2)<0)
			{
				MessageBox.Show("电影已开场，无法退票！", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
			}
			else
			{
				MessageBox.Show("已成功为您退票，退款会在十五分钟内退回您的账户！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
			}
		}

		private void Button_ShowTicketInfo_Click(object sender, RoutedEventArgs e)
		{
			Record u = ListView_Records.SelectedItem as Record;
			Window a = new RecordInfoWindow(u);
			a.ShowDialog();
		}

        


        private void PersonTabItem_Selected(object sender, MouseButtonEventArgs e)
        {

        }

        private void MovieTabItem_Selected(object sender, MouseButtonEventArgs e)
        {

        }

        private void RecordTabItem_Selected(object sender, MouseButtonEventArgs e)
        {

        }

        private void X_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Cursor = Cursors.Hand;
        }
        private void X_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Cursor = Cursors.Arrow;
        }
    }
}