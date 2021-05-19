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

namespace UI_simplify_version
{
    /// <summary>
    /// Register.xaml 的交互逻辑
    /// </summary>
    public partial class Register : Window
    {
        public Register()
        {
            InitializeComponent();
        }

		private void Register_(object sender, RoutedEventArgs e)
		{

		}

		private void Back(object sender, RoutedEventArgs e)
		{
			Window a = new MainWindow();
			this.Hide();
			a.Show();
		}
	}
}
