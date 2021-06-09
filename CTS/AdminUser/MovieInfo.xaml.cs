using AdminUser.AppService;
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
    /// MovieInfo.xaml 的交互逻辑
    /// </summary>
    public partial class MovieInfo : Window
    {
        private static Movie M;
        private static AUVHandler handler;
        public MovieInfo(Movie m)
        {
            InitializeComponent();
            M = m;
            handler = AUVHandler.GetInstance();
            TextBlock_Mid.Text += M.Mid;
            TextBlock_Name.Text += M.Mname;
            TextBlock_Type.Text += M.Mtype;
            TextBlock_Time.Text += M.Mtime;
            TextBlock_Score.Text += M.Mcomment;
            TextBlock_Des.Text += M.Mdescription;
            string path = System.IO.Path.GetFullPath("../../MoviePictures/" + M.Mid + ".jpg");
            BitmapImage bt = new BitmapImage(new Uri(path, UriKind.Absolute));
            MovieImage.Source = bt;
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
