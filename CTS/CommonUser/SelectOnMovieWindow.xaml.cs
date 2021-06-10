using CommonUser.AppServices;
using CommonUser.Entity;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CommonUser
{
    /// <summary>
    /// SelectOnMovieWindow.xaml 的交互逻辑
    /// </summary>
    public partial class SelectOnMovieWindow : Window
    {
        private User user;
        private Movie movie;
        private List<OnMovie> onMovies = new List<OnMovie>();
        private CUVHandler handler;
        public SelectOnMovieWindow(User user,Movie movie)
        {
            this.user = user;
            this.movie = movie;
            handler = CUVHandler.GetInstance();
            InitializeComponent();
            Grid_SelectOnMovie.DataContext = movie;
            SetTabItem();
            SetOnMovies();
        }

        private void SetTabItem()
        {
            Tab_Today.Header += "\n(" + DateTime.Now.ToString("MM/dd") + ")";
            Tab_Tomorrow.Header += "\n(" + DateTime.Now.AddDays(1).ToString("MM/dd") + ")";
            Tab_AfterTom.Header += "\n(" + DateTime.Now.AddDays(2).ToString("MM/dd") + ")";
        }

        private void SetOnMovies()
        {
            onMovies = handler.GetOnMovies(movie.Mid);
            ListView_OnMoviesToday.DataContext = selectOnMovies(0);
            ListView_OnMoviesToday.Items.Refresh();
            ListView_OnMoviesTomo.DataContext = selectOnMovies(1);
            ListView_OnMoviesTomo.Items.Refresh();
            ListView_OnMoviesAfTomo.DataContext = selectOnMovies(2);
            ListView_OnMoviesAfTomo.Items.Refresh();
        }

        private List<OnMovie> selectOnMovies(int time)
        {
            List<OnMovie> myOnMovies =new List<OnMovie>();
            foreach(OnMovie temp in onMovies)
            {
                DateTime dateTime = DateTime.ParseExact(temp.Obegin, "yyyy/MM/dd HH:mm:ss", System.Globalization.CultureInfo.CurrentCulture);
                if (dateTime.Day - DateTime.Now.Day == time && dateTime.CompareTo(DateTime.Now) > 0)
                {
                    myOnMovies.Add(temp);
                }
            }
            return myOnMovies;
        }

        private void X_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Cursor = Cursors.Hand;
        }

        private void X_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Cursor = Cursors.Arrow;
        }

        private void Tab_MouseDoubleClick_Today(object sender, MouseButtonEventArgs e)
        {
            OnMovie temp = ListView_OnMoviesToday.SelectedItem as OnMovie;
            new SelectSeatWindow(user, movie, temp).Show();
            Close();
        }

        private void Tab_MouseDoubleClick_Tomo(object sender, MouseButtonEventArgs e)
        {
            OnMovie temp = ListView_OnMoviesTomo.SelectedItem as OnMovie;
            new SelectSeatWindow(user, movie, temp).Show();
            Close();
        }

        private void Tab_MouseDoubleClick_AfTomo(object sender, MouseButtonEventArgs e)
        {
            OnMovie temp = ListView_OnMoviesAfTomo.SelectedItem as OnMovie;
            new SelectSeatWindow(user, movie, temp).Show();
            Close();
        }

        private void Image_Back_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Close();
        }
    }
}