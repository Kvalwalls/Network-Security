﻿using System;
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
    /// RefindWindow.xaml 的交互逻辑
    /// </summary>
    public partial class RefindWindow : Window
    {
        public RefindWindow()
        {
            InitializeComponent();
        }

        private void Button_MouseEnter(object sender, MouseEventArgs e)
        {
            Cursor = Cursors.Hand;
        }

        private void Button_MouseLeave(object sender, MouseEventArgs e)
        {
            Cursor = Cursors.Arrow;
        }

        private void Button_Refind_Click(object sender, RoutedEventArgs e)
        {
			if(TextBox_id.Text!=string.Empty&&TextBox_name.Text!=string.Empty)
			{

			}
			else
			{
				MessageBox.Show("还有项目未填！", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
			}
        }

        private void Button_Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}