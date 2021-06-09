using CommonUser.Entity;
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
    /// MovieInfoWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MovieInfoWindow : Window
    {
        private User user;
        public MovieInfoWindow(User user,Movie movie)
        {
            this.user = user;
            InitializeComponent();
            Grid_Info.DataContext = movie;
            if (movie.Mcomment >= 2)
                Image_Star1.Opacity = 1;
            if (movie.Mcomment >= 4)
                Image_Star2.Opacity = 1;
            if (movie.Mcomment >= 6)
                Image_Star3.Opacity = 1;
            if (movie.Mcomment >= 8)
                Image_Star4.Opacity = 1;
            if (movie.Mcomment >= 10)
                Image_Star5.Opacity = 1;
        }

        private void Button_Buy_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            Movie movie = button.DataContext as Movie;
            new SelectOnMovieWindow(user, movie).Show();
            Close();
        }

        private void Button_Back_Click(object sender, RoutedEventArgs e)
        {
            Close();
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
