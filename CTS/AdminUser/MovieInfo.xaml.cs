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
    /// MovieInfo.xaml 的交互逻辑
    /// </summary>
    public partial class MovieInfo : Window
    {
        private static Movie M;
        public MovieInfo(Movie m)
        {
            InitializeComponent();
            M = m;
            TextBlock_Mid.Text += M.id;
            TextBlock_Name.Text += M.name;
            TextBlock_Type.Text += M.type;
            TextBlock_Time.Text += M.time;
            TextBlock_Score.Text += M.comment;
            TextBlock_Des.Text += M.description;
            MovieImage.Source = new BitmapImage(new Uri(@"C:\Users\dell\Desktop\网安课设\MoviePictures\" + M.id + ".png"));
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
