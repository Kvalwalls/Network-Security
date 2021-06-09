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
        private Seat[] seats;
        public WaitingWindow(User user, Movie movie, OnMovie onMovie, Seat[] seats)
        {
            this.user = user;
            this.movie = movie;
            this.onMovie = onMovie;
            this.seats = seats;
            InitializeComponent();
            mediaElement.Source = new Uri(
                GetParentDirectory(System.AppDomain.CurrentDomain.BaseDirectory, 3)
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
            string strTips = "座位锁定中";
            DispatcherTimer timer = new DispatcherTimer();
            timer.Tick += new EventHandler(
                (object o, EventArgs e) =>
                {
                    if (countSeconds == 0)
                    {
                        new PayWaitingWindow(user, movie, onMovie, seats).Show();
                        Close();
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
