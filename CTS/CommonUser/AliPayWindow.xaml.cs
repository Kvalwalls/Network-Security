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
    /// AliPayWindow.xaml 的交互逻辑
    /// </summary>
    public partial class AliPayWindow : Window
    {
        public AliPayWindow(float number)
        {
            InitializeComponent();
            TextBlock_Pay.Text += number + "元";
        }
        private void Button_MouseEnter(object sender, MouseEventArgs e)
        {
            Cursor = Cursors.Hand;
        }

        private void Button_MouseLeave(object sender, MouseEventArgs e)
        {
            Cursor = Cursors.Arrow;
        }

        private void Button_Ok_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
