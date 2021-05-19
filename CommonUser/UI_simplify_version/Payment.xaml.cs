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

namespace UI_simplify_version
{
	/// <summary>
	/// 支付购票.xaml 的交互逻辑
	/// </summary>
	public partial class Payment : Window
	{
		private int countSecond = 300; //记录秒数
		private DispatcherTimer disTimer = new DispatcherTimer();

		public Payment()
		{
			InitializeComponent();
			disTimer.Interval = new TimeSpan(0, 0, 0, 1);
			disTimer.Tick += new EventHandler(disTimer_Tick);
			disTimer.Start();
		}
		
	    void disTimer_Tick(object sender, EventArgs e)
　　 {
　　　　if(countSecond==0)
　　　　{
　　　　　　MessageBox.Show("结束");
　　　　}
　　　　else
　　　　{
　　　　　　//判断lblSecond是否处于UI线程上
　　　　　　if (lblSecond.Dispatcher.CheckAccess())
　　　　　　{
　　　　　　　　lblSecond.Content=countSecond.ToString();
　　　　　　}
　　　　　　else
　　　　　　{
　　　　　　　　lblSecond.Dispatcher.BeginInvoke(DispatcherPriority.Normal,(Action)(() =>{
　　　　　　　　　　lblSecond.Content=countSecond.ToString();
　　　　　　　　}));　　
　　　　　　}
　　　　　　countSecond--;
　　　　}
　　}

		private void Button_Click(object sender, RoutedEventArgs e)
		{

		}
	}
}
