using AdminUser.Transmission;
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

namespace AdminUser
{
    /// <summary>
    /// PackageInfo.xaml 的交互逻辑
    /// </summary>
    public partial class PackageInfo : Window
    {
        private static TransMessage r;

        public PackageInfo(TransMessage t)
        {
            InitializeComponent();
            r = t;
            ShowPackage();
        }

        private void Button_MouseEnter(object sender, MouseEventArgs e)
        {
            Cursor = Cursors.Hand;
        }

        private void Button_MouseLeave(object sender, MouseEventArgs e)
        {
            Cursor = Cursors.Arrow;
        }

        private void Button_Return_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ShowPackage()
        {
            TextBlock_Sid.Text += System.Text.Encoding.UTF8.GetString(r.fromAddress);
            TextBlock_Did.Text += System.Text.Encoding.UTF8.GetString(r.toAddress);
            TextBlock_AppType.Text += r.serviceType;
            TextBlock_ConType.Text += r.specificType;
            TextBlock_Error.Text += r.errorCode;
            TextBlock_Crypt.Text += r.cryptCode;
            //TextBlock_MLength.Text += r.SLength;
            //TextBlock_CLength.Text += r.CLength;
            //if (true)
            //{
            //    Text_Encrypt.Text = r.content;
            //}
            //else
            //{
            //    Text_Encrypt.Text = r.content;

            //    Text_Decrypt.Text = r.content;
            //}
        }
    }
}
