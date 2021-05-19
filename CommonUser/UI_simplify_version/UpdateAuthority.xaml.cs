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

namespace UI_simplify_version
{
	/// <summary>
	/// 升级权限.xaml 的交互逻辑
	/// </summary>
	public partial class UpdateAuthority : Window
	{
		public UpdateAuthority()
		{
			InitializeComponent();
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{

		}

		private void Button_Click_1(object sender, RoutedEventArgs e)
		{
			Window a = new Operation();
			this.Hide();
			a.Show();
		}
	}
}
