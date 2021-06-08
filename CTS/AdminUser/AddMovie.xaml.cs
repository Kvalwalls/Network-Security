using AdminUser.Entity;
using AdminUser.AppService;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace AdminUser
{
    /// <summary>
    /// AddMovie.xaml 的交互逻辑
    /// </summary>
    public partial class AddMovie : Window
    {
        private static AdminUserHandler handler;
        private static List<Movie> SubMovies;
        private static string picture;
        public AddMovie(List<Movie> movies)
        {
            InitializeComponent();
            handler = AdminUserHandler.GetInstatnce();
            SubMovies = movies;
            MidTip.Visibility = Visibility.Hidden;
            NameTip.Visibility = Visibility.Hidden;
            Type1Tip.Visibility = Visibility.Hidden;
            Type2Tip.Visibility = Visibility.Hidden;
            Type3Tip.Visibility = Visibility.Hidden;
            TimeTip.Visibility = Visibility.Hidden;
            ScoreTip.Visibility = Visibility.Hidden;
            DesTip.Visibility = Visibility.Hidden;
            PicTip.Visibility = Visibility.Hidden;
        }

        private void Button_Select_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openfiledialog = new OpenFileDialog
            {
                Filter = "图像文件|*.jpg;*.png;|所有文件|*.*"
            };

            if ((bool)openfiledialog.ShowDialog())
            {
                BitmapImage image = new BitmapImage(new Uri(openfiledialog.FileName));
                if(image == null)
                {
                    PicTip.Text = "影片图片不能为空！";
                    PicTip.Visibility = Visibility.Visible;
                }
                MovieImage.Source = image;

                BitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(image));

                using (var fileStream = new System.IO.FileStream(@"C:\Users\dell\Desktop\网安课设\MoviePictures\" + TextBox_mid.Text + ".png", System.IO.FileMode.Create))
                {
                    encoder.Save(fileStream);
                }
                picture = @"C:\Users\dell\Desktop\网安课设\MoviePictures\" + TextBox_mid.Text + ".png";
            }


        }

        private void Button_Add_Click(object sender, RoutedEventArgs e)
        {
            string Id = TextBox_mid.Text;

            string Name = TextBox_name.Text;
            string Type = TextBox_type1.Text + '/' + TextBox_type2.Text + '/' + TextBox_type3.Text;
            int Time = int.Parse(TextBox_time.Text);
            float Score = float.Parse(TextBox_score.Text);
            string Description = TextBox_description.Text;

            if (TextBox_mid.Text == "")
            {
                MessageBox.Show("请输入影片号！");
            }
            else if (TextBox_name.Text == "")
            {
                MessageBox.Show("请输入影片名称！");
            }
            else if (TextBox_type1.Text == "")
            {
                MessageBox.Show("请输入影片类型！");
            }
            else if (TextBox_time.Text == "")
            {
                MessageBox.Show("请输入影片时长！");
            }
            else if (TextBox_score.Text == "")
            {
                MessageBox.Show("请输入影片评分！");
            }
            else if (picture == "")
            {
                MessageBox.Show("请选择影片图片！");
            }
            else if (TextBox_description.Text == "")
            {
                MessageBox.Show("请输入影片简介！");
            }
            else
            {
                Movie n = new Movie(Id, Name, Type, Time, Score, Description);
                int result = 1;
                for (int i = 0; i < SubMovies.Count; i++)
                {
                    if (Id == SubMovies[i].Mid)
                    {
                        MessageBox.Show("已有该影片");
                        result = 0;
                    }
                }

                if (result == 1)
                {
                    handler.addMovieRequest(n);
                    if (handler.addMovieReply() == "添加成功")
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
                    
                    for (int i = 0; i < SubMovies.Count; i++)
                    {
                        if (Id == SubMovies[i].Mid)
                        {
                            MidTip.Text = "影片号重复，请重新输入！";
                            MidTip.Visibility = Visibility.Visible;
                        }
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
                
                for (int i = 0; i < SubMovies.Count; i++)
                {
                    if (Id == SubMovies[i].Mid)
                    {
                        MidTip.Text = "影片号重复，请重新输入！";
                        MidTip.Visibility = Visibility.Visible;
                    }
                }
                
            }
        }

        private void TextBox_name_MouseLeave(object sender, MouseEventArgs e)
        {
            if (TextBox_name.IsFocused == true)
            {
                if (string.IsNullOrEmpty(TextBox_name.Text))
                {
                    NameTip.Text = "影片名称不能为空！";
                    NameTip.Visibility = Visibility.Visible;
                }
                else
                {
                    NameTip.Visibility = Visibility.Hidden;
                    
                }
            }
        }

        private void TextBox_name_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(TextBox_name.Text))
            {
                NameTip.Text = "影片名称不能为空！";
                NameTip.Visibility = Visibility.Visible;
            }
            else
            {
                NameTip.Visibility = Visibility.Hidden;

            }
        }

        private void TextBox_type1_MouseLeave(object sender, MouseEventArgs e)
        {
            if (TextBox_type1.IsFocused == true)
            {
                if (string.IsNullOrEmpty(TextBox_type1.Text))
                {
                    Type1Tip.Text = "影片类型1不能为空！";
                    Type1Tip.Visibility = Visibility.Visible;
                }
                else
                {
                    Type1Tip.Visibility = Visibility.Hidden;

                }
            }
        }

        private void TextBox_type1_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(TextBox_type1.Text))
            {
                Type1Tip.Text = "影片类型1不能为空";
                Type1Tip.Visibility = Visibility.Visible;
            }
            else
            {
                NameTip.Visibility = Visibility.Hidden;

            }
        }

        private void TextBox_type2_MouseLeave(object sender, MouseEventArgs e)
        {
            if (TextBox_type2.IsFocused == true)
            {
                if (string.IsNullOrEmpty(TextBox_type2.Text))
                {
                    Type2Tip.Text = "影片类型2不能为空！";
                    Type2Tip.Visibility = Visibility.Visible;
                }
                else
                {
                    Type2Tip.Visibility = Visibility.Hidden;

                }
            }
        }

        private void TextBox_type2_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(TextBox_type2.Text))
            {
                Type2Tip.Text = "影片类型2不能为空！";
                Type2Tip.Visibility = Visibility.Visible;
            }
            else
            {
                Type2Tip.Visibility = Visibility.Hidden;

            }
        }

        private void TextBox_type3_MouseLeave(object sender, MouseEventArgs e)
        {
            if (TextBox_name.IsFocused == true)
            {
                if (string.IsNullOrEmpty(TextBox_type3.Text))
                {
                    Type3Tip.Text = "影片类型3不能为空！";
                    Type3Tip.Visibility = Visibility.Visible;
                }
                else
                {
                    Type3Tip.Visibility = Visibility.Hidden;

                }
            }
        }

        private void TextBox_type3_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(TextBox_type3.Text))
            {
                Type3Tip.Text = "影片类型3不能为空！";
                Type3Tip.Visibility = Visibility.Visible;
            }
            else
            {
                Type3Tip.Visibility = Visibility.Hidden;
                if(TextBox_type3.Text == TextBox_type2.Text)
                {
                    Type3Tip.Text = "类型重复，请重新输入！";
                    Type3Tip.Visibility = Visibility.Visible;
                }
            }
        }

        private void TextBox_time_MouseLeave(object sender, MouseEventArgs e)
        {
            if (TextBox_time.IsFocused == true)
            {
                if (string.IsNullOrEmpty(TextBox_time.Text))
                {
                    TimeTip.Text = "影片时长不能为空！";
                    TimeTip.Visibility = Visibility.Visible;
                }
                else
                {
                    TimeTip.Visibility = Visibility.Hidden;
                }
            }
        }

        private void TextBox_time_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (TextBox_time.IsFocused == true)
            {
                if (string.IsNullOrEmpty(TextBox_time.Text))
                {
                    TimeTip.Text = "影片时长不能为空！";
                    TimeTip.Visibility = Visibility.Visible;
                }
                else
                {
                    TimeTip.Visibility = Visibility.Hidden;
                }
            }
        }

        private void TextBox_score_MouseLeave(object sender, MouseEventArgs e)
        {
            if (TextBox_score.IsFocused == true)
            {
                if (string.IsNullOrEmpty(TextBox_score.Text))
                {
                    ScoreTip.Text = "票价不能为空！";
                    ScoreTip.Visibility = Visibility.Visible;
                }
                else
                {
                    ScoreTip.Visibility = Visibility.Hidden;

                }
            }
        }

        private void TextBox_score_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(TextBox_score.Text))
            {
                ScoreTip.Text = "票价不能为空！";
                ScoreTip.Visibility = Visibility.Visible;
            }
            else
            {
                ScoreTip.Visibility = Visibility.Hidden;

            }
        }

        private void TextBox_description_MouseLeave(object sender, MouseEventArgs e)
        {
            if (TextBox_description.IsFocused == true)
            {
                if (string.IsNullOrEmpty(TextBox_description.Text))
                {
                    DesTip.Text = "影片简介不能为空！";
                    DesTip.Visibility = Visibility.Visible;
                }
                else
                {
                    DesTip.Visibility = Visibility.Hidden;

                }
            }
        }

        private void TextBox_description_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(TextBox_description.Text))
            {
                DesTip.Text = "影片简介不能为空！";
                DesTip.Visibility = Visibility.Visible;
            }
            else
            {
                DesTip.Visibility = Visibility.Hidden;

            }
        }



    }
}
