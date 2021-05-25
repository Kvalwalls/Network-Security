using CommonUser.Entity;
using System.Windows;
using System.Windows.Input;

namespace CommonUser
{
	/// <summary>
	/// ShowTicketInfo.xaml 的交互逻辑
	/// </summary>
	public partial class RecordInfoWindow : Window
	{
		private Record record;
		public RecordInfoWindow(Record record)
		{
			this.record = record;
			InitializeComponent();
			InitData();
		}

		private void InitData()
		{
			Label_ID.Content = record.Uid + record.Oid + record.Sid;
			Label_Mname.Content = record.Mname;
			Label_Begin.Content = record.Obegin;
			Label_End.Content = record.Oend;
			Label_Tid.Content = "影厅" + record.Tid;
			Label_Sid.Content = "第" + record.Sid.Substring(0, 3) + "行 第" + record.Sid.Substring(3, 3) + "列";
			Label_Price.Content = record.Rprice + "元";
		}

		private void Button_Print_Click(object sender, RoutedEventArgs e)
		{

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
