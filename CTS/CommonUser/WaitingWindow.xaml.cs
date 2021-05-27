using CommonUser.Entity;
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
        public WaitingWindow(Seat[] seat)
        {
            InitializeComponent();
            mediaElement.Source = new Uri(
                GetParentDirectory(System.AppDomain.CurrentDomain.BaseDirectory, 3)
                + "\\ImageResources\\装饰(黑)_等待动态.gif"
                );
            InitTips();
        }

        private string GetParentDirectory(string path, int parentCount)
        {
            for (int i = 0; i < parentCount; i++)
                path = System.IO.Path.GetDirectoryName(path);
            return path;
        }

        private void InitTips()
        {
            string strTips = "购票加载中";
            int length = 0;
            DispatcherTimer timer = new DispatcherTimer();
            timer.Tick += new EventHandler(
                (Object o, EventArgs e) =>
                {
                    Label_Tips.Content = strTips.Substring(0, length);
                    length = (length + 1) % (strTips.Length + 1);
                }
            );
            timer.Interval = new TimeSpan(5000000);
            timer.Start();
        }

        private void MediaElement_MediaEnded(object sender, RoutedEventArgs e)
        {
            ((MediaElement)sender).Position = ((MediaElement)sender).Position.Add(TimeSpan.FromMilliseconds(1));
        }
    }
}
