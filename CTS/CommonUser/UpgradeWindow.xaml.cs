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
            TextBlock_Id.Text += user.id;
            switch (user.access)
            {
                case "01":
                    TextBlock_Access.Text += "VIP用户";
                    break;
                case "02":
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
            int newAccess = ComboBox_Access.SelectedIndex + 1;
            if (newAccess == 0)
            {
                MessageBox.Show("请选择正确的权限！", "选择错误");
                return;
            }
            if (newAccess <= int.Parse(user.access))
            {
                MessageBox.Show("请选择更高的权限！", "选择错误");
                return;
            }
            new MyPayWIndow(newAccess*199).ShowDialog();
            /*发送请求*/
            MessageBox.Show("升级成功！", "提示");
            Close();
        }
    }
}
