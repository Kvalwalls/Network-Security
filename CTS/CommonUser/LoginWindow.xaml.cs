using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Configuration;
using System.Collections.Generic;

namespace CommonUser
{
	/// <summary>
	/// MainWindow.xaml 的交互逻辑
	/// </summary>
	public partial class LoginWindow : Window
	{
		private string id = "";
		private string password = "";

		public LoginWindow()
		{
			InitializeComponent();
			mediaElement.Source = new Uri(
				GetParentDirectory(System.AppDomain.CurrentDomain.BaseDirectory, 3)
				+ "\\ImageResources\\背景_登录动态.gif"
				);
		}

		private string GetParentDirectory(string path, int parentCount)
		{
			for (int i = 0; i < parentCount; i++)
				path = System.IO.Path.GetDirectoryName(path);
			return path;
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
			user.Uaccess = "01";
			new MainWindow(user).Show();
			Close();
		}

		private void Register(object sender, RoutedEventArgs e)
		{
			Hide();
			new RegisterWindow().ShowDialog();
			Show();
		}

		private void Refind(object sender, RoutedEventArgs e)
		{
			Hide();
			new RefindWindow().ShowDialog();
			Show();
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