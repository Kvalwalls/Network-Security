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
    /// Package.xaml 的交互逻辑
    /// </summary>
    public partial class Package : Window
    {
        public Package()
        {
            InitializeComponent();
        }


        public ListView PList
        {
            get { return PackageList; }
            set { PackageList = value; }
        }

        public void CatchPackage(Transmission.TransMessage message)
        {
            message.Time = DateTime.Now;
            message.toA = AddressPhaser.BytesToString(message.toAddress);
            message.fromA = AddressPhaser.BytesToString(message.fromAddress);
            PackageList.Items.Add(message);
            PackageList.Items.Refresh();
        }

        private void PackageList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Transmission.TransMessage select = PackageList.SelectedItem as Transmission.TransMessage;
            //var select = txtBox.DataContext as Movie;
            //ListBox listBox = sender as ListBox;
            //if (listBox == null || listBox.SelectedItem == null)
            //{
            //    MessageBox.Show("ListBox1双击对象为空...");
            //}
            //else
            //{
            //Transmission.TransMessage tem = new Transmission.TransMessage();

            Hide();
            PackageInfo info = new PackageInfo(select);
            info.ShowDialog();
            Show();
            //}
        }

    }
}
