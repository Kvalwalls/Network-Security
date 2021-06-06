using CommonUser.AppServices;
using CommonUser.Entity;
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
    /// RechargeWindow.xaml 的交互逻辑
    /// </summary>
    public partial class RechargeWindow : Window
    {
        private User user;
        private CUVHandler handler;

        public RechargeWindow(User user)
        {
            this.user = user;
            handler = CUVHandler.GetInstance();
            InitializeComponent();
            TextBlock_Id.Text += user.Uid;
            TextBlock_Money.Text += (user.Umoney + "元");
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

        private void Button_Recharge_Click(object sender, RoutedEventArgs e)
        {
            float number = 0.0f;
            if (!IsNumber(TextBox_Input.Text, out number))
            {
                MessageBox.Show("请输入正确的数字！", "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                TextBox_Input.Text = string.Empty;
                return;
            }
            else
            {
                new MyPayWIndow(number).ShowDialog();
                if (handler.Recharge(user.Uid, number))
                    MessageBox.Show("充值成功！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                else
                    MessageBox.Show("充值失败！", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
                Close();
            }
        }

        private bool IsNumber(string str, out float number)
        {
            try
            {
                number = float.Parse(str);
                if (number <= 0)
                    return false;
                return true;
            }
            catch
            {
                number = 0.0f;
                return false;
            }
        }
    }
}
