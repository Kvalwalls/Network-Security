using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

namespace CommonUser
{
    /// <summary>
    /// SelectOnMovieWindow.xaml 的交互逻辑
    /// </summary>
    public partial class SelectOnMovieWindow : Window
    {
        private Movie movie;
        private List<OnMovie> onMovies;
        public SelectOnMovieWindow(Movie movie)
        {
            this.movie = movie;
            this.onMovies = new List<OnMovie>();
            InitializeComponent();
            Grid_SelectOnMovie.DataContext = movie;
            InitTabItem();
            InitOnMovies();
        }

        private void InitTabItem()
        {
            Tab_Today.Header += "\n(" + DateTime.Now.ToString("MM/dd") + ")";
            Tab_Tomorrow.Header += "\n(" + DateTime.Now.AddDays(1).ToString("MM/dd") + ")";
            Tab_AfterTom.Header += "\n(" + DateTime.Now.AddDays(2).ToString("MM/dd") + ")";
        }

        private void InitOnMovies()
        {
            OnMovie temp = new OnMovie("O00001", movie.Mid, "T000001", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"), DateTime.Now.AddMinutes(movie.Mtime).ToString("yyyy/MM/dd hh:mm:ss"), 35);
            onMovies.Add(temp);
            temp = new OnMovie("O00002", movie.Mid, "T000002", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"), DateTime.Now.AddMinutes(movie.Mtime).ToString("yyyy/MM/dd hh:mm:ss"), 35);
            onMovies.Add(temp);
            temp = new OnMovie("O00003", movie.Mid, "T000003", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"), DateTime.Now.AddMinutes(movie.Mtime).ToString("yyyy/MM/dd hh:mm:ss"), 35);
            onMovies.Add(temp);
            temp = new OnMovie("O00004", movie.Mid, "T000001", DateTime.Now.AddDays(1).ToString("yyyy/MM/dd HH:mm:ss"), DateTime.Now.AddDays(1).AddMinutes(movie.Mtime).ToString("yyyy/MM/dd hh:mm:ss"), 35);
            onMovies.Add(temp);
            temp = new OnMovie("O00005", movie.Mid, "T000002", DateTime.Now.AddDays(1).ToString("yyyy/MM/dd HH:mm:ss"), DateTime.Now.AddDays(1).AddMinutes(movie.Mtime).ToString("yyyy/MM/dd hh:mm:ss"), 35);
            onMovies.Add(temp);
            temp = new OnMovie("O00006", movie.Mid, "T000001", DateTime.Now.AddDays(2).ToString("yyyy/MM/dd HH:mm:ss"), DateTime.Now.AddDays(2).AddMinutes(movie.Mtime).ToString("yyyy/MM/dd hh:mm:ss"), 35);
            onMovies.Add(temp);
            temp = new OnMovie("O00007", movie.Mid, "T000002", DateTime.Now.AddDays(2).ToString("yyyy/MM/dd HH:mm:ss"), DateTime.Now.AddDays(2).AddMinutes(movie.Mtime).ToString("yyyy/MM/dd hh:mm:ss"), 35);
            onMovies.Add(temp);
            ListView_OnMoviesToday.DataContext = selectOnMovies(0);
            ListView_OnMoviesTomo.DataContext = selectOnMovies(1);
            ListView_OnMoviesAfTomo.DataContext = selectOnMovies(2);
        }

        private List<OnMovie> selectOnMovies(int time)
        {
            List<OnMovie> myOnMovies =new List<OnMovie>();
            foreach(OnMovie temp in onMovies)
            {
                DateTime dateTime = DateTime.ParseExact(temp.Obegin, "yyyy/MM/dd HH:mm:ss", System.Globalization.CultureInfo.CurrentCulture);
                if (dateTime.Day - DateTime.Now.Day == time)
                {
                    myOnMovies.Add(temp);
                }
            }
            return myOnMovies;
        }

        private void Button_Buy_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Button_Cancel_Click(object sender, RoutedEventArgs e)
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

        private void Tab_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Close();
        }
    }
}
