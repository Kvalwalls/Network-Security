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

namespace 服务器UI
{
    /// <summary>
    /// AddTheater.xaml 的交互逻辑
    /// </summary>
    public partial class AddTheater : Window
    {
        private static List<Theater> SubTheaters;
        public AddTheater(List<Theater> theaters)
        {
            InitializeComponent();
            SubTheaters = theaters;
            IdTip.Visibility = Visibility.Hidden;
            TypeTip.Visibility = Visibility.Hidden;
            SizeTip.Visibility = Visibility.Hidden;
        }

        private void Button_MouseEnter(object sender, MouseEventArgs e)
        {
            Cursor = Cursors.Hand;
        }

        private void Button_MouseLeave(object sender, MouseEventArgs e)
        {
            Cursor = Cursors.Arrow;
        }

        private void Button_Add_Click(object sender, RoutedEventArgs e)
        {
            string Id = TextBox_id.Text;
            string Type = ComboBox_TheaterType.Text;
            int Size = int.Parse(ComboBox_TheaterSize.Text);

            if (TextBox_id.Text == "")
            {
                MessageBox.Show("请输入影厅号！");
            }
            else if (ComboBox_TheaterType.SelectedItem == null)
            {
                MessageBox.Show("请选择影厅类型！");
            }
            else if (ComboBox_TheaterSize.SelectedItem == null)
            {
                MessageBox.Show("请选择影厅大小！");
            }
            else
            {
                byte type = 0;
                if(Type == "普通影厅")
                {
                    type = 0;
                }
                else if(Type == "VIP影厅")
                {
                    type = 1;
                }
                else if (Type == "SVIP影厅")
                {
                    type = 2;
                }
                Theater n = new Theater(Id, type, Size);
                //addTheaterRequest();
                //addTheaterReply();
                //int result = dgl.CreateStore(s);
                int result = 1;
                for (int i = 0; i < SubTheaters.Count; i++)
                {
                    if (Id == SubTheaters[i].Tid)
                    {
                        MessageBox.Show("已有该影厅");
                        result = 0;
                    }
                }

                if (result == 1)
                {
                    MessageBox.Show("添加成功");
                    this.Tag = n;//写入窗体的Tag属性中,在主窗体对此进行接收

                    //DialogResult = true;//关闭窗体
                    this.Close();
                }
                else if (result == 0)
                {
                    MessageBox.Show("添加失败");

                }
                else
                {
                    MessageBox.Show("未知错误");
                }
            }
        }

        private void Button_Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void TextBox_id_MouseLeave(object sender, MouseEventArgs e)
        {
            if (TextBox_id.IsFocused == true)
            {
                if (string.IsNullOrEmpty(TextBox_id.Text))
                {
                    IdTip.Text = "影厅号不能为空！";
                    IdTip.Visibility = Visibility.Visible;
                }
                else
                {
                    IdTip.Visibility = Visibility.Hidden;
                    string Id = TextBox_id.Text;
                    for (int i = 0; i < SubTheaters.Count; i++)
                    {
                        if (Id == SubTheaters[i].Tid)
                        {
                            IdTip.Text = "影厅号重复，请重新输入！";
                            IdTip.Visibility = Visibility.Visible;
                        }
                    }

                }
            }
        }

        private void ComboBox_TheaterType_MouseLeave(object sender, MouseEventArgs e)
        {
            if (ComboBox_TheaterType.IsFocused == true)
            {
                if (string.IsNullOrEmpty(ComboBox_TheaterType.Text) || ComboBox_TheaterType.SelectedItem == null)
                {
                    TypeTip.Text = "影厅类型不能为空！";
                    TypeTip.Visibility = Visibility.Visible;
                }
                else
                {
                    TypeTip.Visibility = Visibility.Hidden;
                }
            }
        }

        private void ComboBox_TheaterSize_MouseLeave(object sender, MouseEventArgs e)
        {
            if (ComboBox_TheaterSize.IsFocused == true)
            {
                if (string.IsNullOrEmpty(ComboBox_TheaterSize.Text) || ComboBox_TheaterSize.SelectedItem == null)
                {
                    SizeTip.Text = "影厅大小不能为空！";
                    SizeTip.Visibility = Visibility.Visible;
                }
                else
                {
                    TypeTip.Visibility = Visibility.Hidden;
                }
            }
        }

        private void TextBox_id_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(TextBox_id.Text))
            {
                IdTip.Text = "影厅号不能为空！";
                IdTip.Visibility = Visibility.Visible;
            }
            else
            {
                IdTip.Visibility = Visibility.Hidden;
                string Id = TextBox_id.Text;
                for (int i = 0; i < SubTheaters.Count; i++)
                {
                    if (Id == SubTheaters[i].Tid)
                    {
                        IdTip.Text = "影厅号重复，请重新输入！";
                        IdTip.Visibility = Visibility.Visible;
                    }
                }

            }
        }

    }
}
