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
    /// SelectSeatWindow.xaml 的交互逻辑
    /// </summary>
    public partial class SelectSeatWindow : Window
    {
        private OnMovie OnMovie;
        public SelectSeatWindow(OnMovie onMovie)
        {
            this.OnMovie = onMovie;
            InitializeComponent();
        }

        private void Button_Buy_Click(object sender, RoutedEventArgs e)
        {

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
