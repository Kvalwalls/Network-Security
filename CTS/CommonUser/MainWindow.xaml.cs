using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace CommonUser
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private User user;
        private DispatcherTimer showTimer;
        public MainWindow(User user)
        {
            this.user = user;
            InitializeComponent();
            InitTextBlock_Time();
            InitTextBlock_Hello();
        }

        private void InitTextBlock_Time()
        {
            showTimer = new System.Windows.Threading.DispatcherTimer();
            showTimer.Tick += new EventHandler
                (
                    (object sender, EventArgs e) =>
                    {
                        this.TextBlock_Time.Text = DateTime.Now.ToString("yyyy年MM月dd日 dddd HH:mm:ss");
                    }
                );
            showTimer.Interval = new TimeSpan(0, 0, 0, 1, 0);
            showTimer.Start();
        }

        private void InitTextBlock_Hello()
        {
            this.TextBlock_Hello.Text += user.name;
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
