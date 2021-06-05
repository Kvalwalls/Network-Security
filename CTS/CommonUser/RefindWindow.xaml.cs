using CommonUser.AppServices;
using System.Windows;
using System.Windows.Input;

namespace CommonUser
{
    /// <summary>
    /// RefindWindow.xaml 的交互逻辑
    /// </summary>
    public partial class RefindWindow : Window
    {
        private CUVHandler handler;

        public RefindWindow()
        {
            InitializeComponent();
            handler = CUVHandler.GetInstance();
        }

        private void Button_Refind_Click(object sender, RoutedEventArgs e)
        {
            if (TextBox_id.Text != string.Empty && TextBox_name.Text != string.Empty)
            {
                string password = handler.Refind(TextBox_id.Text, TextBox_name.Text);
                if (password != null)
                {
                    MessageBox.Show("查询成功！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    TextBox_pwd.Text = password;
                }
                else
                {
                    MessageBox.Show("查询失败！请重新输入！", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
                    TextBox_id.Text = string.Empty;
                    TextBox_name.Text = string.Empty;
                    TextBox_pwd.Text = string.Empty;
                }
            }
            else
            {
                MessageBox.Show("存在空白项！", "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void Button_MouseEnter(object sender, MouseEventArgs e)
        {
            Cursor = Cursors.Hand;
        }

        private void Button_MouseLeave(object sender, MouseEventArgs e)
        {
            Cursor = Cursors.Arrow;
        }

        private void Button_Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
