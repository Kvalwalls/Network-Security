using AdminUser.Entity;
using AdminUser.AppService;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace AdminUser
{
    /// <summary>
    /// Login.xaml 的交互逻辑
    /// </summary>
    public partial class Login : Window
    {
		private static AUVHandler handler;
		private string id = "";
		private string password = "";

		public Login()
        {
            InitializeComponent();
			handler = AUVHandler.GetInstance();
            string path1 = System.IO.Directory.GetCurrentDirectory();
            string path2 = System.IO.Directory.GetParent(path1).ToString();
            string path3 = System.IO.Directory.GetParent(path2).ToString();
            mediaElement.Source = new Uri(path3 + "//ImageResources//背景_登录动态.gif");
        }
		private void MediaElement_MediaEnded(object sender, RoutedEventArgs e)
		{
			((MediaElement)sender).Position = ((MediaElement)sender).Position.Add(TimeSpan.FromMilliseconds(1));
		}

		private void Button_Login_Click(object sender, RoutedEventArgs e)
		{
			User u = new User();
			if (string.Empty.Equals(TextBox_id.Text) && string.Empty.Equals(TextBox_pwd.Text))
			{
				MessageBox.Show("请输入账号和密码！", "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
				return;
			}
			if (string.Empty.Equals(TextBox_id.Text))
			{
				MessageBox.Show("请输入账号！", "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
				return;
			}
			if (string.Empty.Equals(TextBox_pwd.Text))
			{
				MessageBox.Show("请输入密码！", "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
				return;
			}
			u.Uid = TextBox_id.Text;
			u.Upassword = TextBox_pwd.Text;
			handler.loginRequest(u);
			string[] reply = handler.loginReply();
			if (reply[0] == "登录成功")
			{
				MessageBox.Show("登录成功！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
				u.Uname = reply[1];
				u.Umoney = Convert.ToSingle(reply[2]);
				switch (reply[3])
				{
					case "0":
						{
							u.Uaccess = 0;
							break;
						}
					case "1":
						{
							u.Uaccess = 1;
							break;
						}
					case "2":
						{
							u.Uaccess = 2;
							break;
						}
					case "3":
						{
							u.Uaccess = 3;
							break;
						}
					case "4":
						{
							u.Uaccess = 4;
							break;
						}
					default:
						{
							break;
						}
				}
			
				new MainWindow(u).Show();
				Close();
			}
			else
			{
				MessageBox.Show("登录失败！"+ reply[0], "提示", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}
		}

		private void TextBlock_MouseEnter(object sender, MouseEventArgs e)
		{
			TextBlock T = sender as TextBlock;
			T.Foreground = Brushes.Gold;
			Cursor = Cursors.Hand;
		}

		private void TextBlock_MouseLeave(object sender, MouseEventArgs e)
		{
			TextBlock T = sender as TextBlock;
			T.Foreground = Brushes.White;
			Cursor = Cursors.Arrow;
		}

		private void Button_MouseEnter(object sender, MouseEventArgs e)
		{
			Cursor = Cursors.Hand;
		}

		private void Button_MouseLeave(object sender, MouseEventArgs e)
		{
			Cursor = Cursors.Arrow;
		}

		private void TextBox_pwd_TextChanged(object sender, TextChangedEventArgs e)
		{
			if (TextBox_pwd.Text.Length <= password.Length)
			{
				password = password.Substring(0, TextBox_pwd.Text.Length);
				return;
			}
			password += TextBox_pwd.Text[TextBox_pwd.Text.Length - 1];
			string temp = "";
			for (int i = 0; i < TextBox_pwd.Text.Length; i++)
				temp += "*";
			TextBox_pwd.Text = temp;
			TextBox_pwd.Select(TextBox_pwd.Text.Length, 0);
		}
	}
}
