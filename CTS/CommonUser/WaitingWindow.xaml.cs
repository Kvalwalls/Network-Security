using CommonUser.AppServices;
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
        private User user;
        private Movie movie;
        private OnMovie onMovie;
        private string[] sids;
        private CUVHandler handler;
        public WaitingWindow(User user, Movie movie, OnMovie onMovie, string[] sids)
        {
            this.user = user;
            this.movie = movie;
            this.onMovie = onMovie;
            this.sids = sids;
            handler = CUVHandler.GetInstance();
            InitializeComponent();
            mediaElement.Source = new Uri(GetParentDirectory(System.AppDomain.CurrentDomain.BaseDirectory, 3)
                + "\\ImageResources\\装饰(黑)_等待动态.gif");
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
            int countSeconds = 5;
            float price = handler.SelectSeat(user.Uid,onMovie.Oid, sids);
            Theater theater = handler.GetTheater(onMovie.Tid);
            string strTips = "座位锁定中";
            DispatcherTimer timer = new DispatcherTimer();
            timer.Tick += new EventHandler(
                (object o, EventArgs e) =>
                {
                    if (countSeconds == 0)
                    {
                        timer.Stop();
                        Close();
                        if (price == 0)
                            MessageBox.Show("购票失败！", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
                        else
                            new PayWaitingWindow(user, movie, theater, onMovie, sids, price).Show();
                        return;
                    }
                    else
                    {
                        Label_Tips.Content = strTips.Substring(0, 6 - countSeconds);
                        countSeconds--;
                    }
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
