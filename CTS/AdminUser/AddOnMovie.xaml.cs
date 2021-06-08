using AdminUser.Entity;
using AdminUser.AppService;
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
    /// AddOnMovie.xaml 的交互逻辑
    /// </summary>
    public partial class AddOnMovie : Window
    {
        private static AdminUserHandler handler;
        private static List<OnMovie> SubOnMovies;
        private static List<Movie> SubMovies;
        public AddOnMovie(List<OnMovie> onmovies, List<Movie> movies)
        {
            InitializeComponent();
            SubOnMovies = onmovies;
            SubMovies = movies;
            handler = AdminUserHandler.GetInstatnce();
            OidTip.Visibility = Visibility.Hidden;
            MidTip.Visibility = Visibility.Hidden;
            PidTip.Visibility = Visibility.Hidden;
            StimeTip.Text = "时间格式：2016-5-9 13:00:00";
            PriceTip.Visibility = Visibility.Hidden;
            
        }
        private DateTime CaculateEndTime(string Start,string Mid)
        {
            int Dtime = 0;
            for (int i=0; i< SubMovies.Count;i++)
            {
                if(SubMovies[i].Mid == Mid)
                {
                    Dtime = SubMovies[i].Mtime;
                }
            }
            DateTime EndTime = Convert.ToDateTime(Start).AddMinutes(Dtime);
            return EndTime;
        }

        private void Button_Add_Click(object sender, RoutedEventArgs e)
        {
            string Oid = TextBox_oid.Text;
            string Mid = TextBox_mid.Text;
            string Tid = TextBox_pid.Text;
            string Btime = TextBox_starttime.Text;
            DateTime BTime = Convert.ToDateTime(Btime);
            DateTime ETime = CaculateEndTime(Btime,Mid);
            float Price = float.Parse(TextBox_price.Text);
            

            if (TextBox_oid.Text == "")
            {
                MessageBox.Show("请输入场次号！");
            }
            else if (TextBox_mid.Text == "")
            {
                MessageBox.Show("请输入影片号！");
            }
            else if (TextBox_pid.Text == "")
            {
                MessageBox.Show("请输入影厅号！");
            }
            else if (TextBox_starttime.Text == "")
            {
                MessageBox.Show("请输入开始时间！");
            }
            else if (TextBox_price.Text == "")
            {
                MessageBox.Show("请输入票价！");
            }
            else
            {
                
                OnMovie n = new OnMovie(Oid, Mid, Tid, BTime, ETime, Price);
                //addOnMovieRequest();
                //addOnMovieReply();
                //int result = dgl.CreateStore(s);
                int result = 1;
                bool mexist = false;
                bool pexist = false;
                for (int i = 0; i < SubOnMovies.Count; i++)
                {
                    if (Oid == SubOnMovies[i].Oid)
                    {
                        MessageBox.Show("已有该场次");
                        result = 0;
                    }
                }

                for (int i = 0; i < SubOnMovies.Count; i++)
                {
                    if (Mid == SubOnMovies[i].Mid)
                    {
                        mexist = true;
                    }
                }
                if(mexist == false)
                {
                    MessageBox.Show("无该影片号");
                    result = 0;
                }


                for (int i = 0; i < SubOnMovies.Count; i++)
                {
                    if (Tid == SubOnMovies[i].Tid)
                    {
                        pexist = true;
                    }
                }
                if (pexist == false)
                {
                    MessageBox.Show("无该影厅号");
                    result = 0;
                }

                for (int i = 0; i < SubOnMovies.Count; i++)
                {
                    if (Tid == SubOnMovies[i].Tid)
                    {
                        if (DateTime.Compare(SubOnMovies[i].Oend, BTime) < 0 || DateTime.Compare(SubOnMovies[i].Obegin, ETime) > 0)
                        {
                            PriceTip.Visibility = Visibility.Hidden;
                        }
                        else
                        {
                            StimeTip.Text = "该影厅对应时间已有场次，请安排其他时间！";
                            MessageBox.Show("该影厅对应时间已有场次，请安排其他时间！", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
                            StimeTip.Visibility = Visibility.Visible;
                            result = 0;
                        }
                    }
                }

                if (result == 1)
                {
                    handler.addOnMovieRequest(n);
                    if (handler.addOnMovieReply() == "添加成功")
                    {
                        MessageBox.Show("添加成功！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    this.Tag = n;//写入窗体的Tag属性中,在主窗体对此进行接收

                    //DialogResult = true;//关闭窗体
                    this.Close();
                }
                else if (result == 0)
                {
                    MessageBox.Show("添加失败！", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    MessageBox.Show("未知错误！", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void Button_Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Button_MouseEnter(object sender, MouseEventArgs e)
        {
            Cursor = Cursors.Hand;
        }

        private void Button_MouseLeave(object sender, MouseEventArgs e)
        {
            Cursor = Cursors.Arrow;
        }

        private void TextBox_oid_MouseLeave(object sender, MouseEventArgs e)
        {
            if (TextBox_oid.IsFocused == true)
            {
                if (string.IsNullOrEmpty(TextBox_oid.Text))
                {
                    OidTip.Text = "场次号不能为空！";
                    OidTip.Visibility = Visibility.Visible;
                }
                else
                {
                    OidTip.Visibility = Visibility.Hidden;
                    string Id = TextBox_oid.Text;
                    for (int i = 0; i < SubOnMovies.Count; i++)
                    {
                        if (Id == SubOnMovies[i].Oid)
                        {
                            OidTip.Text = "场次号重复，请重新输入！";
                            OidTip.Visibility = Visibility.Visible;
                        }
                    }

                }
            }
        }

        private void TextBox_oid_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(TextBox_oid.Text))
            {
                OidTip.Text = "场次号不能为空！";
                OidTip.Visibility = Visibility.Visible;
            }
            else
            {
                OidTip.Visibility = Visibility.Hidden;
                string Id = TextBox_oid.Text;
                for (int i = 0; i < SubOnMovies.Count; i++)
                {
                    if (Id == SubOnMovies[i].Oid)
                    {
                        OidTip.Text = "场次号重复，请重新输入！";
                        OidTip.Visibility = Visibility.Visible;
                    }
                }

            }
        }

        private void TextBox_mid_MouseLeave(object sender, MouseEventArgs e)
        {
            if (TextBox_mid.IsFocused == true)
            {
                if (string.IsNullOrEmpty(TextBox_mid.Text))
                {
                    MidTip.Text = "影片号不能为空！";
                    MidTip.Visibility = Visibility.Visible;
                }
                else
                {
                    MidTip.Visibility = Visibility.Hidden;
                    string Id = TextBox_mid.Text;
                    bool exist = false;
                    for (int i = 0; i < SubOnMovies.Count; i++)
                    {
                        if (Id == SubOnMovies[i].Mid)
                        {
                            exist = true;
                        }
                    }
                    if (exist == false)
                    {
                        MidTip.Text = "无该影片号，请重新输入！";
                        MidTip.Visibility = Visibility.Visible;
                    }
                }
            }
        }

        private void TextBox_mid_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(TextBox_mid.Text))
            {
                MidTip.Text = "影片号不能为空！";
                MidTip.Visibility = Visibility.Visible;
            }
            else
            {
                MidTip.Visibility = Visibility.Hidden;
                string Id = TextBox_mid.Text;
                bool exist = false;
                for (int i = 0; i < SubOnMovies.Count; i++)
                {
                    if (Id == SubOnMovies[i].Mid)
                    {
                        exist = true;
                    }
                }
                if (exist == false)
                {
                    MidTip.Text = "无该影片号，请重新输入！";
                    MidTip.Visibility = Visibility.Visible;
                }
            }
        }

        private void TextBox_pid_MouseLeave(object sender, MouseEventArgs e)
        {
            if (TextBox_pid.IsFocused == true)
            {
                if (string.IsNullOrEmpty(TextBox_pid.Text))
                {
                    PidTip.Text = "影厅号不能为空！";
                    PidTip.Visibility = Visibility.Visible;
                }
                else
                {
                    PidTip.Visibility = Visibility.Hidden;
                    string Id = TextBox_pid.Text;
                    bool exist = false;
                    for (int i = 0; i < SubOnMovies.Count; i++)
                    {
                        if (Id == SubOnMovies[i].Tid)
                        {
                            exist = true;
                        }
                    }
                    if (exist == false)
                    {
                        PidTip.Text = "无该影厅号，请重新输入！";
                        PidTip.Visibility = Visibility.Visible;
                    }
                }
            }
        }

        private void TextBox_pid_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(TextBox_pid.Text))
            {
                PidTip.Text = "影厅号不能为空！";
                PidTip.Visibility = Visibility.Visible;
            }
            else
            {
                PidTip.Visibility = Visibility.Hidden;
                string Id = TextBox_pid.Text;
                bool exist = false;
                for (int i = 0; i < SubOnMovies.Count; i++)
                {
                    if (Id == SubOnMovies[i].Tid)
                    {
                        exist = true;
                    }
                }
                if (exist == false)
                {
                    PidTip.Text = "无该影厅号，请重新输入！";
                    PidTip.Visibility = Visibility.Visible;
                }
            }
        }

        private void TextBox_starttime_MouseLeave(object sender, MouseEventArgs e)
        {
            if (TextBox_starttime.IsFocused == true)
            {
                if (string.IsNullOrEmpty(TextBox_starttime.Text))
                {
                    StimeTip.Text = "开始时间不能为空！";
                    StimeTip.Visibility = Visibility.Visible;
                }
                else
                {
                    StimeTip.Visibility = Visibility.Hidden;
                }
            }
        }

        private void TextBox_starttime_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(TextBox_starttime.Text))
            {
                StimeTip.Text = "开始时间不能为空！";
                StimeTip.Visibility = Visibility.Visible;
            }
            else
            {
                StimeTip.Visibility = Visibility.Hidden;
                

            }
        }

        private void TextBox_price_MouseLeave(object sender, MouseEventArgs e)
        {
            if (TextBox_price.IsFocused == true)
            {
                if (string.IsNullOrEmpty(TextBox_price.Text))
                {
                    PriceTip.Text = "票价不能为空！";
                    PriceTip.Visibility = Visibility.Visible;
                }
                else
                {
                    PriceTip.Visibility = Visibility.Hidden;

                }
            }
        }

        private void TextBox_price_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(TextBox_price.Text))
            {
                PriceTip.Text = "票价不能为空！";
                PriceTip.Visibility = Visibility.Visible;
            }
            else
            {
                PriceTip.Visibility = Visibility.Hidden;

            }
        }

    }
}
