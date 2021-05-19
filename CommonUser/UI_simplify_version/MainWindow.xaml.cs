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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace UI_simplify_version
{
	/// <summary>
	/// MainWindow.xaml 的交互逻辑
	/// </summary>
	public partial class MainWindow : Window
	{
		string password = null;
		string id = null;
		public MainWindow()
		{
			InitializeComponent();
			
		}
		private void MediaElement_MediaEnded(object sender, RoutedEventArgs e)
		{
			((MediaElement)sender).Position = ((MediaElement)sender).Position.Add(TimeSpan.FromMilliseconds(1));
		}

		private void Comfirm(object sender, RoutedEventArgs e)
		{
			Window a = new Operation();
			MessageBox.Show("登录成功！","congratulations",MessageBoxButton.OK,MessageBoxImage.Information);
			this.Hide();
			a.Show();
		}

		private void Register(object sender, RoutedEventArgs e)
		{
			Window a = new Register();
			this.Hide();
			a.Show();
		}

		private void Reback(object sender, RoutedEventArgs e)
		{
			Window a = new Reback();
			this.Hide();
			a.Show();
		}

		private void Pwd_KeyUp(object sender, KeyEventArgs e)
		{
			password += this.pwd.Text[pwd.Text.Length- 1];
			string temp = "";
			for (int i = 0; i < pwd.Text.Length; i++)
				temp += "*";
			pwd.Text = temp;
			pwd.Select(pwd.Text.Length, 0);
		}

		private void TextBlock_MouseEnter(object sender, MouseEventArgs e)
		{
			TextBlock T = sender as TextBlock;
			T.Foreground= Brushes.Gold;
		}

		private void TextBlock_MouseLeave(object sender, MouseEventArgs e)
		{
			TextBlock T = sender as TextBlock;
			T.Foreground = Brushes.White;
		}
	}
}