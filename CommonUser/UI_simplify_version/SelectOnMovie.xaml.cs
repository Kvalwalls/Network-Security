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
	/// 选择影厅.xaml 的交互逻辑
	/// </summary>
	public partial class SelectOnMovie : Window
	{
		public SelectOnMovie()
		{
			InitializeComponent();
			Data ps1 = new Data();//单行数据
			ps1.O_ID = "000001";
			ps1.P_ID = "000001";
			ps1.O_BeginTime = "10:00";
			ps1.O_EndTime = "11:55";
			ps1.O_Price = "33";
			ps1.T_Type = "艺术厅";
			infoList.Add(ps1);
			DataGrid1.AutoGenerateColumns = false;
			DataGrid1.ItemsSource = infoList;
		}

		public struct Data
		{
			public string O_ID { set; get; }
			public string P_ID { set; get; }
			public string O_BeginTime { set; get; }
			public string O_EndTime { set; get; }
			public string O_Price{ set; get; }
			public string T_Type { set; get; }
		}
		List<Data> infoList = new List<Data>();
		private void Button_Click(object sender, RoutedEventArgs e)
		{
			
		}
	}
}
