using CommonUser;
using CommonUser.Entity;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
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
        private HashSet<Seat> selectedSeats;
        private Theater theater;
        private Movie movie;
        private OnMovie onMovie;
        public SelectSeatWindow(OnMovie onMovie, Movie movie, Theater theater)
        {
            this.theater = theater;
            this.movie = movie;
            this.onMovie = onMovie;
            seats = new List<Seat>(theater.Tsize);
            selectedSeats = new HashSet<Seat>();
            for (int i = 1; i < 5; i++)
            {
                string strRow = string.Format("{0:d3}", i);
                for (int j = 1; j < 17; j++)
                {
                    string strCol = string.Format("{0:d3}", j);
                    Seat temp = new Seat(strRow + strCol, "O000001", SeatStatus.Unselected);
                    seats.Add(temp);
                }
            }
            seats.Sort(new SIDComparer());
            InitializeComponent();
            InitInfo();
            InitSeats();
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
            TextBlock tempTB = null;
            ControlSeat tempCS = null;
            int column = 16;//列
            int row = seats.Count / column;//行
            double spanHeight = (280 - row * 40) / (row + 1);
            double spanWidth = (850 - column * 40) / (column + 1);
            for (int i = 0; i < row; i++)
            {
                tempTB = new TextBlock();
                tempTB.VerticalAlignment = VerticalAlignment.Center;
                tempTB.Text = string.Format("第{0}行", i + 1);
                tempTB.Margin = new Thickness(10, 10 + i * (40 + spanHeight) , 850, 220 - i * (40 + spanHeight));
                Grid_Seats.Children.Add(tempTB);
                for (int j = 0; j < column; j++)
                {
                    tempCS = new ControlSeat(seats[i].Sstatus);
                    tempCS.Margin = new Thickness(90 + j * (40 + spanWidth), 10 + i * (40 + spanHeight), 680 - j * (40 * spanWidth), 220 - i * (40 + spanHeight));
                    tempCS.Name = "CS" + seats[i * 16 + j].Sid;
                    tempCS.MouseUp += TempCS_MouseUp;
                    Grid_Seats.Children.Add(tempCS);
                }
            }
        }

        private void TempCS_MouseUp(object sender, MouseButtonEventArgs e)
        {
            ControlSeat controlSeat = sender as ControlSeat;
            string strRowCol = controlSeat.Name.Substring(2, 6);
            if (controlSeat.status == SeatStatus.Selecting)
            {  
                selectedSeats.Add(new Seat(strRowCol, onMovie.Oid, SeatStatus.Selecting));
            }
            else
            {
                selectedSeats.RemoveWhere((Seat s) => s.Sid.Equals(strRowCol));
            }
            string textTB = null;
            foreach (Seat temp in selectedSeats)
                textTB += "第" + temp.Sid.Substring(0, 3) + "行第" + temp.Sid.Substring(3, 3) + "列；";
            TextBox_Selected.Text = textTB;
        }

        private void Button_Buy_Click(object sender, RoutedEventArgs e)
        {
            int i = 0;
            Seat[] temps = new Seat[selectedSeats.Count];
            foreach (Seat temp in selectedSeats)
            {
                temps[i] = temp;
                i++;
            }
			Array.Sort(temps, new SIDComparer());
			//new WaitingWindow(temps).Show();
			new PayWaitingWindow(onMovie, movie, temps , theater).Show();
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
