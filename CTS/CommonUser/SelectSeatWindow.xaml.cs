using CommonUser;
using CommonUser.AppServices;
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
        private User user;
        private Movie movie;
        private Theater theater;
        private OnMovie onMovie;
        private CUVHandler handler;
        public SelectSeatWindow(User user, Movie movie, OnMovie onMovie)
        {
            handler = CUVHandler.GetInstance();
            this.user = user;
            this.movie = movie;
            this.onMovie = onMovie;
            theater = handler.GetTheater(onMovie.Tid);
            seats = handler.GetSeats(onMovie.Oid);
            selectedSeats = new HashSet<Seat>();
            seats.Sort(new SIDComparer());
            InitializeComponent();
            InitInfo();
            InitSeats();
        }

        private void InitInfo()
        {
            TextBlock_Info.Text = onMovie.Tid + "号" +TtypeSwitch(theater.Ttype) +"厅  " + movie.Mname;
            BitmapImage bmp = new BitmapImage();
            bmp.BeginInit();
            bmp.UriSource = new Uri(movie.Mpicture, UriKind.Absolute);
            bmp.EndInit();
            Image_Picture.Source = bmp;
        }

        private string TtypeSwitch(byte type)
        {
            if (type == EnumTheaterType.VIP)
                return "VIP厅";
            else if (type == EnumTheaterType.SVIP)
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
                    tempCS = new ControlSeat(seats[i * 16 + j].Sstatus);
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
            string strRowCol = controlSeat.Name.Substring(2, 4);
            if (controlSeat.status == EnumSeatStatus.Selecting)
            {  
                selectedSeats.Add(new Seat(strRowCol, onMovie.Oid, EnumSeatStatus.Selecting));
            }
            else
            {
                selectedSeats.RemoveWhere((Seat s) => s.Sid.Equals(strRowCol));
            }
            string textTB = null;
            foreach (Seat temp in selectedSeats)
                textTB += "第" + temp.Sid.Substring(0, 2) + "行第" + temp.Sid.Substring(2, 2) + "列；";
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
            new WaitingWindow(user, movie, onMovie, temps).Show();
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
