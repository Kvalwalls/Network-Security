using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using CommonUser.Entity;
using System.IO;
using CommonUser.AppServices;

namespace CommonUser
{
	/// <summary>
	/// MainWindow.xaml 的交互逻辑
	/// </summary>
	public partial class LoginWindow : Window
	{
		//工作路径
		private readonly string workPath = Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).ToString()).ToString();
		//CUVHandler实例
		private CUVHandler handler;

		//构造函数
		public LoginWindow()
		{
			InitializeComponent();
			handler = CUVHandler.GetInstance();
			mediaElement.Source = new Uri(workPath + "//ImageResources//背景_登录动态.gif");
		}

		//设置GIF动画的循环播放
		private void MediaElement_MediaEnded(object sender, RoutedEventArgs e)
		{
			((MediaElement)sender).Position = ((MediaElement)sender).Position.Add(TimeSpan.FromMilliseconds(1));
		}

		//登录按钮函数
		private void Button_Login_Click(object sender, RoutedEventArgs e)
		{
			if (string.Empty.Equals(TextBox_id.Text) && string.Empty.Equals(PasswordBox_pwd.Password))
			{
				MessageBox.Show("请输入账号和密码！", "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
				return;
			}
			if (string.Empty.Equals(TextBox_id.Text))
			{
				MessageBox.Show("请输入账号！", "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
				return;
			}
			if (string.Empty.Equals(PasswordBox_pwd.Password))
			{
				MessageBox.Show("请输入密码！", "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
				return;
			}
			if (handler.Login(TextBox_id.Text, PasswordBox_pwd.Password))
			{
				MessageBox.Show("登录成功！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
				new MainWindow(TextBox_id.Text).Show();
				Close();
			}
			else
			{
				MessageBox.Show("登录失败！", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}
		}

		//用户注册点击函数
		private void Register(object sender, RoutedEventArgs e)
		{
			Hide();
			new RegisterWindow().ShowDialog();
			Show();
		}

		//找回密码点击函数
		private void Refind(object sender, RoutedEventArgs e)
		{
			Hide();
			new RefindWindow().ShowDialog();
			Show();
		}

		//鼠标样式
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
	}
}