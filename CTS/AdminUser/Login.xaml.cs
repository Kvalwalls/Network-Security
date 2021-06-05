using AdminUser.Entity;
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
		private string id = "";
		private string password = "";

		public Login()
        {
            InitializeComponent();
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
			id = TextBox_id.Text;
			if (id == "")
			{
				MessageBox.Show("请输入账号！", "输入错误");
				return;
			}
			if (password == "")
			{
				MessageBox.Show("请输入密码！", "输入错误");
				return;
			}
			MessageBox.Show("账号：" + id + "\n密码：" + password);
			User user = new User();
			user.Uid = id;
			user.Upassword = password;
			user.Uname = "汪帮传";
			user.Umoney = 1000;
			user.Uaccess = 3;
			new MainWindow(user).Show();
			Close();
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
