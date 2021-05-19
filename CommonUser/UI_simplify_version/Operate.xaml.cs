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
	/// 个人操作.xaml 的交互逻辑
	/// </summary>
	public partial class Operation : Window
	{
		public Operation()
		{
			InitializeComponent();
		}
		public struct Refund
		{
			public string O_ID { set; get; }
			public string P_ID { set; get; }
			public string O_BeginTime { set; get; }
			public string O_EndTime { set; get; }
			public string O_Price { set; get; }
			public string T_Type { set; get; }
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			Window a = new Change();
			this.Hide();
			a.Show();
		}

		private void Button_Click_1(object sender, RoutedEventArgs e)
		{
			Window a = new Recharge();
			this.Hide();
			a.Show();
		}

		private void Button_Click_2(object sender, RoutedEventArgs e)
		{
			Window a = new UpdateAuthority();
			this.Hide();
			a.Show();
		}
	}
}
