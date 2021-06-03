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

namespace 服务器UI
{
    /// <summary>
    /// PackageInfo.xaml 的交互逻辑
    /// </summary>
    public partial class PackageInfo : Window
    {
        //public static TransMessage r;
        public PackageInfo()
        {
            InitializeComponent();
            //r = t;
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
            //TextBlock_Sid.Text += r.Sid;
            //TextBlock_Did.Text += r.Did;
            //TextBlock_AppType.Text += r.SerType;
            //TextBlock_ConType.Text += r.SpeType;
            //TextBlock_Error.Text += r.Error;
            /*TextBlock_Crypt.Text += r.Crypt;
            TextBlock_MLength.Text += r.SLength;
            TextBlock_CLength.Text += r.CLength;
            if (true)
            {
                Text_Encrypt.Text = r.content;
            }
            else
            {
                Text_Encrypt.Text = r.content;

                Text_Decrypt.Text = r.content;
            }*/



        }
    }
}
