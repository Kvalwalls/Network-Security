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

        public RechargeWindow(User user)
        {
            this.user = user;
            InitializeComponent();
            TextBlock_Id.Text += user.id;
            TextBlock_Money.Text += (user.money + "元");
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
                MessageBox.Show("请输入正确的数字！", "输入错误");
                return;
            }
            else
            {
                new AliPayWindow(number).ShowDialog();
                /*发送请求*/
                MessageBox.Show("充值成功！", "提示");
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
