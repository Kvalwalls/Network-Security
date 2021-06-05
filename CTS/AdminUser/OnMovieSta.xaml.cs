using AdminUser.Entity;
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
    /// OnMovieSta.xaml 的交互逻辑
    /// </summary>
    public partial class OnMovieSta : Window
    {
        public OnMovieSta(string oid,List<Record> records)
        {
            InitializeComponent();
            float total = 0;
            OnMovie.Text += oid;
            for (int i = 0; i < records.Count; i++)
            {
                if (records[i].Oid == oid)
                {
                    OnMovieStaList.Items.Add(records[i]);
                    total += records[i].Rprice;
                }
            }
            OnMovieStaList.Items.Refresh();
            Money.Text += total;
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
    }
}
