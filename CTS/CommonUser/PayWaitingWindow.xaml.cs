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
using CommonUser.AppServices;
using CommonUser.Entity;

namespace CommonUser
{
    /// <summary>
    /// PayWaitingWindow.xaml 的交互逻辑
    /// </summary>
    public partial class PayWaitingWindow : Window
    {
		private float price;
		private User user;
		private string[] sids;
		private Movie movie;
		private OnMovie onMovie;
		private Theater theater;
		private int countSecond = 5;
		private CUVHandler handler;
		public PayWaitingWindow(User user, Movie movie, Theater theater, OnMovie onMovie, string[] sids, float price)
		{
			this.price = price;
			this.user = user;
			this.onMovie = onMovie;
			this.movie = movie;
			this.sids = sids;
			this.theater = theater;
			handler = CUVHandler.GetInstance();
			InitializeComponent();
			InitInfo();
			InitCounter();
		}

		private void InitInfo()
		{
			Grid_PayWaiting.DataContext = movie;
			TextBlock_Mname.Text = movie.Mname;
			TextBlock_Obegin.Text = onMovie.Obegin;
			TextBlock_Oend.Text = onMovie.Oend;
			TextBlock_price.Text = price.ToString() + " 元";
			TextBlock_Tid.Text = theater.Tid;
			for (int i = 0; i < sids.Length; i++)
				TextBox_Sid.Text += "第" + sids[i].Substring(0, 2) + "行第" + sids[i].Substring(2, 2) + "列；";
		}

		void InitCounter()
		{
			DispatcherTimer timeCounter = new DispatcherTimer();
			timeCounter.Interval = new TimeSpan(0, 0, 0, 1);
			timeCounter.Tick += new EventHandler(
				(object o, EventArgs e) =>
				{
					if (countSecond == 0)
					{
						if (handler.PayTimeout(user.Uid, onMovie.Oid, sids))
							MessageBox.Show("支付超时！", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
						timeCounter.Stop();
						Close();
						return;
					}
					else
					{
						if (Label_Time.Dispatcher.CheckAccess())
							Label_Time.Content = countSecond.ToString();
						else
							Label_Time.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (Action)(
								() =>
								{
									Label_Time.Content = countSecond.ToString();
								}));
						countSecond--;
					}
				});
			timeCounter.Start();
		}

		private void Button_MouseEnter(object sender, MouseEventArgs e)
		{
			Cursor = Cursors.Hand;
		}

		private void Button_MouseLeave(object sender, MouseEventArgs e)
		{
			Cursor = Cursors.Arrow;
		}

		private void Button_Payment_Click(object sender, RoutedEventArgs e)
		{
			if(handler.PayMoney(user.Uid,onMovie.Oid,sids,price))
				MessageBox.Show("支付成功！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
			else
				MessageBox.Show("支付失败！", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
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

		private void Button_Cancel_Click(object sender, RoutedEventArgs e)
		{
			Close();
		}
	}
}
