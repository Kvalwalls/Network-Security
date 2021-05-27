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
using System.Windows.Threading;
using CommonUser.Entity;

namespace CommonUser
{
    /// <summary>
    /// PayWaitingWindow.xaml 的交互逻辑
    /// </summary>
    public partial class PayWaitingWindow : Window
    {
		private Seat[] seat;
		private Movie movie;
		private OnMovie onMovie;
		private Theater theater;
		private int countSecond = 300; //记录秒数
		public PayWaitingWindow(OnMovie onMovie, Movie movie, Seat[] seat,Theater theater)
        {
			this.onMovie=onMovie;
			this.movie=movie;
			this.seat = seat;
			this.theater=theater;
            InitializeComponent();
			Grid_PayWaiting.DataContext = movie;
			InitData();
			DispatcherTimer disTimer = new DispatcherTimer();
			disTimer.Interval = new TimeSpan(0, 0, 0, 1); //参数分别为：天，小时，分，秒。此方法有重载，可根据实际情况调用。
			disTimer.Tick += new EventHandler(disTimer_Tick); //每一秒执行的方法
			disTimer.Start();
		}

		private void InitData()
		{
			M_name.Text = movie.Mname;
			O_begin.Text = onMovie.Obegin;
			O_end.Text = onMovie.Oend;
			T_id.Text = theater.Tid;
			for (int i = 0; i < seat.Length; i++)
			{
				S_id.Text +=  "第" + seat[i].Sid.Substring(0, 3) + "行第" + seat[i].Sid.Substring(3, 3) + "列；";
			}
			O_price.Text = (seat.Length * onMovie.Oprice).ToString();
		}
		void disTimer_Tick(object sender, EventArgs e)
		{
			if (countSecond == 0)
			{
				MessageBox.Show("支付超时！本次支付已取消！", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
			}
			else
			{
				//判断lblSecond是否处于UI线程上
				if (timelabel.Dispatcher.CheckAccess())
				{
					timelabel.Content = countSecond.ToString();
				}
				else
				{
					timelabel.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (Action)(() => {
						timelabel.Content = countSecond.ToString();
					}));
				}
				countSecond--;
			}
		}

		private void Button_Payment_Click(object sender, RoutedEventArgs e)
		{

		}

		private void X_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
		{
			Cursor = Cursors.Hand;
		}

		private void X_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
		{
			Cursor = Cursors.Arrow;
		}

		private void Button_Cancel_Click(object sender, RoutedEventArgs e)
		{
			Close();
		}
	}
}
