using System;
using System.Windows;
using System.Windows.Controls;

namespace CommonUser
{
    /// <summary>
    /// LockingWindow.xaml 的交互逻辑
    /// </summary>
    public partial class WaitingWindow : Window
    {
        public WaitingWindow()
        {
            InitializeComponent();
            mediaElement.Source = new Uri(
                GetParentDirectory(System.AppDomain.CurrentDomain.BaseDirectory, 3)
                + "\\ImageResources\\装饰(黑)_等待动态.gif"
                );
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
