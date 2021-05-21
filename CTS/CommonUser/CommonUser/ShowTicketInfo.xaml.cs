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

namespace CommonUser
{
	/// <summary>
	/// ShowTicketInfo.xaml 的交互逻辑
	/// </summary>
	public partial class ShowTicketInfo : Window
	{
		public ShowTicketInfo(Ticket ticket)
		{
			InitializeComponent();
			numofTicket.Text = ticket.O_Id + ticket.T_Id + ticket.S_Id;
			nameofmovie.Text = ticket.M_Name;
			numofyingting.Text = ticket.T_Id;
			numofseats.Text = ticket.S_Id;
			begintime.Text = ticket.O_BeginTime;
			endtime.Text = ticket.O_EndTime;
		}

		private void Button_Print_Click(object sender, RoutedEventArgs e)
		{

		}
	}
}
