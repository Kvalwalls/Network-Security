using CommonUser.AppServices;
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
        private CUVHandler handler;
        public UpgradeWindow(User user)
        {
            this.user = user;
            handler = CUVHandler.GetInstance();
            InitializeComponent();
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
            int newAccess = ComboBox_Access.SelectedIndex + 3;
            if (newAccess == 1)
            {
                MessageBox.Show("请选择正确的权限！", "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (newAccess <= user.Uaccess)
            {
                MessageBox.Show("请选择更高的权限！", "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            new MyPayWIndow(newAccess * 199).ShowDialog();
            if (handler.UpgradeAccess(user.Uid, (byte)newAccess))
                MessageBox.Show("升级权限成功！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
            else
                MessageBox.Show("升级权限失败！", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
            Close();
        }
    }
}
