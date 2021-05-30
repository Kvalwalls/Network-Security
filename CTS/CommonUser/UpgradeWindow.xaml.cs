using CommonUser.Entity;
using System.Windows;
using System.Windows.Input;

namespace CommonUser
{
    /// <summary>
    /// UpgradeWindow.xaml 的交互逻辑
    /// </summary>
    public partial class UpgradeWindow : Window
    {
        private User user;
        public UpgradeWindow(User user)
        {
            InitializeComponent();
            this.user = user;
            TextBlock_Id.Text += user.Uid;
            switch (user.Uaccess)
            {
                case EnumUserAccess.U_VIP:
                    TextBlock_Access.Text += "VIP用户";
                    break;
                case EnumUserAccess.U_SVIP:
                    TextBlock_Access.Text += "SVIP用户";
                    break;
                default:
                    TextBlock_Access.Text += "普通用户";
                    break;
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

        private void Button_Upgrade_Click(object sender, RoutedEventArgs e)
        {
            int newAccess = ComboBox_Access.SelectedIndex + 2;
            if (newAccess == 1)
            {
                MessageBox.Show("请选择正确的权限！", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (newAccess <= user.Uaccess)
            {
                MessageBox.Show("请选择更高的权限！", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            new MyPayWIndow(newAccess * 199).ShowDialog();
            /*发送请求*/
            MessageBox.Show("升级成功！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
            Close();
        }
    }
}
