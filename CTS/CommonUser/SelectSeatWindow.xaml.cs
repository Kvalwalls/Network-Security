using CommonUser;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace CommonUser
{
    /// <summary>
    /// SelectSeatWindow.xaml 的交互逻辑
    /// </summary>
    public partial class SelectSeatWindow : Window
    {
        private List<Seat> seats;
        private Theater theater;
        private Movie movie;
        private OnMovie onMovie;
        public SelectSeatWindow(OnMovie onMovie, Movie movie, Theater theater)
        {
            this.theater = theater;
            this.movie = movie;
            this.onMovie = onMovie;
            seats = new List<Seat>(theater.Tsize);
            InitializeComponent();
            InitInfo();
            seats.Sort(new SIDComparer());
/*            ControlSeat temp = new ControlSeat(SeatStatus.Selected);
            temp.Margin = new Thickness(10, 10, 940 - 10 - 70, 300 - 10 - 70);
            Grid_Seats.Children.Add(temp);
            temp = new ControlSeat(SeatStatus.Unselected);
            temp.Margin = new Thickness(10+35, 10, 940 - 10 - 105, 300 - 10 - 105);
            Grid_Seats.Children.Add(temp);*/
        }

        private void InitInfo()
        {
            TextBlock_Info.Text = theater.Tid + "号" + TtypeSwitch(theater.Ttype) + " " + movie.Mname;
            BitmapImage bmp = new BitmapImage();
            bmp.BeginInit();
            bmp.UriSource = new Uri(movie.Mpicture, UriKind.Relative);
            bmp.EndInit();
            Image_Picture.Source = bmp;
        }

        private string TtypeSwitch(TheaterType type)
        {
            if (type == TheaterType.VIP)
                return "VIP厅";
            else if (type == TheaterType.SVIP)
                return "SVIP厅";
            else
                return "普通厅";
        }

        private void InitSeats()
        {
            int column = 10;
            int row = theater.Tsize / 10;
            double spanHeight = (Grid_Seats.Height - row * 70) / (row + 1);
            double spanWidth = (Grid_Seats.Width - column * 70 - 200) / (column + 1) ;
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < column; j++)
                {

                }
            }
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

        private void Image_Back_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Close();
        }
    }

    public class SIDComparer : IComparer<Seat>
    {
        public int Compare(Seat x, Seat y)
        {
            return x.Sid.CompareTo(y.Sid);
        }
    }
}
