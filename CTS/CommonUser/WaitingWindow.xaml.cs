using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace CommonUser
{
    /// <summary>
    /// LockingWindow.xaml 的交互逻辑
    /// </summary>
    public partial class WaitingWindow : Window
    {
        private Seat[] selectedSeats;
        public WaitingWindow(Seat[] selectedSeats)
        {
            this.selectedSeats = selectedSeats;
            InitializeComponent();
            mediaElement.Source = new Uri(
                GetParentDirectory(System.AppDomain.CurrentDomain.BaseDirectory, 3)
                + "\\ImageResources\\装饰(黑)_等待动态.gif"
                );
            InitTip();
        }

        private void InitTip()
        {
            int length = 1;
            string strTip = "购票加载中";
            DispatcherTimer timer = new DispatcherTimer();
            timer.Tick += new EventHandler(
                (object sender, EventArgs e) =>
                {
                    Label_Tip.Content = strTip.Substring(0, length);
                    length = (length + 1) % (strTip.Length + 1);
                }
            );
            timer.Interval = new TimeSpan(5000000);
            timer.Start();
        }

        private string GetParentDirectory(string path, int parentCount)
        {
            for (int i = 0; i < parentCount; i++)
                path = System.IO.Path.GetDirectoryName(path);
            return path;
        }

        private void MediaElement_MediaEnded(object sender, RoutedEventArgs e)
        {
            ((MediaElement)sender).Position = ((MediaElement)sender).Position.Add(TimeSpan.FromMilliseconds(1));
        }
    }
}
