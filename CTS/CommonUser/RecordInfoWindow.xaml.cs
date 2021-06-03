using CommonUser.Entity;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;

namespace CommonUser
{
	/// <summary>
	/// ShowTicketInfo.xaml 的交互逻辑
	/// </summary>
	public partial class RecordInfoWindow : Window
	{
		private Record record;
		private OnMovie onMovie;
		private Movie movie;
		private Seat[] seats;
		public RecordInfoWindow(Record record)
		{
			this.record = record;
			InitializeComponent();
			Grid_RecordInfo.DataContext = movie;
			InitData();
			InitData();
		}

		private void InitData()
		{
			Label_ID.Content = record.Uid + record.Oid + record.Sid;
			/*这里需要对服务器发送请求*/
			/*需要几个sql语句进行查询，通过record.Oid到OnMovie表查询影片的开始时间OBegintime和结束时间OEndtime*/
			/*通过Record.Oid到OnMovie表查询影片号Mid，再根据查询到的Mid到Movie表中查询电影名Mname*/
			/*Label_Mname.Content = *.Mame;
			Label_Begin.Content = *.Obegin;
			Label_End.Content = *.Oend;
			Label_Tid.Content = "影厅" + *.Tid;*/
			Label_Sid.Content = "第" + record.Sid.Substring(0, 3) + "行 第" + record.Sid.Substring(3, 3) + "列";
			Label_Price.Content = record.Rprice + "元";
		}

		private void Button_Print_Click(object sender, RoutedEventArgs e)
		{
			PrintDialog dialog = new PrintDialog();
			Label_Info.Content = "             票据信息：" + "票据编号：" + Label_ID.Content + "\n\n" + "电影名称：" + Label_Mname.Content + "\n\n"
				+ "开始时间：" + Label_Begin.Content + "\n\n" + "结束时间" + Label_End.Content + "\n\n" + "影厅号：" + Label_Tid + "\n\n"
				+ "座位号：" + Label_Sid.Content + "\n\n" + "付款金额：" + Label_Price.Content + "\n";
			if (dialog.ShowDialog() == true)
			{
				dialog.PrintVisual(Label_Info, "票据信息打印");
			}
		}

		private void Button_Back_Click(object sender, RoutedEventArgs e)
		{
			Close();
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
