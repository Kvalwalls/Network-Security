﻿using System;
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
    /// AddUser.xaml 的交互逻辑
    /// </summary>
    public partial class AddUser : Window
    {
        private static List<User> SubUsers;
        public AddUser(List<User> users)
        {
            InitializeComponent();
            SubUsers = users;
            IdTip.Visibility = Visibility.Hidden;
            NameTip.Visibility = Visibility.Hidden;
            PasTip.Visibility = Visibility.Hidden;
            AceTip.Visibility = Visibility.Hidden;
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
            String Id = TextBox_id.Text;
            String Name = TextBox_name.Text;
            String Password = TextBox_pwdInput.Text;
            String Access = ComboBox_Access.Text;
            float Money = 0;

            if (Id == "")
            {
                MessageBox.Show("请输入用户号！");
            }
            else if (Name == "")
            {
                MessageBox.Show("请输入用户名称！");
            }
            else if (Password == "")
            {
                MessageBox.Show("请输入用户密码！");
            }
            else if (Access == "")
            {
                MessageBox.Show("请选择用户权限！");
            }
            else
            {
                User n = new User(Id, Name, Password, Access, Money);
                //int result = dgl.CreateStore(s);
                int result = 1;
                for (int i=0;i<SubUsers.Count;i++)
                {
                    if(Id == SubUsers[i].id)
                    {
                        MessageBox.Show("已有该用户");
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
                    IdTip.Text = "用户号不能为空！";
                    IdTip.Visibility = Visibility.Visible;
                }
                else
                {
                    IdTip.Visibility = Visibility.Hidden;
                    string Id = TextBox_id.Text;
                    for (int i = 0; i < SubUsers.Count; i++)
                    {
                        if (Id == SubUsers[i].id)
                        {
                            IdTip.Text = "用户号重复，请重新输入！";
                            IdTip.Visibility = Visibility.Visible;
                        }
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
                    NameTip.Text = "用户名称不能为空！";
                    NameTip.Visibility = Visibility.Visible;
                }
                else
                {
                    NameTip.Visibility = Visibility.Hidden;
                }
            }
        }

        private void TextBox_pwdInput_MouseLeave(object sender, MouseEventArgs e)
        {
            if (TextBox_pwdInput.IsFocused == true)
            {
                if (string.IsNullOrEmpty(TextBox_pwdInput.Text))
                {
                    PasTip.Text = "用户密码不能为空！";
                    PasTip.Visibility = Visibility.Visible;
                }
                else
                {
                    PasTip.Visibility = Visibility.Hidden;
                }
            }
        }

        private void ComboBox_Access_MouseLeave(object sender, MouseEventArgs e)
        {
            if (ComboBox_Access.IsFocused == true)
            {
                if (string.IsNullOrEmpty(ComboBox_Access.Text) || ComboBox_Access.SelectedItem == null)
                {
                    AceTip.Text = "用户权限不能为空！";
                    AceTip.Visibility = Visibility.Visible;
                }
                else
                {
                    AceTip.Visibility = Visibility.Hidden;
                }
            }
        }

        private void TextBox_id_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(TextBox_id.Text))
            {
                IdTip.Text = "用户号不能为空！";
                IdTip.Visibility = Visibility.Visible;
            }
            else
            {
                IdTip.Visibility = Visibility.Hidden;
                string Id = TextBox_id.Text;
                for (int i = 0; i < SubUsers.Count; i++)
                {
                    if (Id == SubUsers[i].id)
                    {
                        IdTip.Text = "用户号重复，请重新输入！";
                        IdTip.Visibility = Visibility.Visible;
                    }
                }

            }
        }

        private void TextBox_name_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(TextBox_name.Text))
            {
                NameTip.Text = "用户名称不能为空！";
                NameTip.Visibility = Visibility.Visible;
            }
            else
            {
                NameTip.Visibility = Visibility.Hidden;
            }
        }

        private void TextBox_pwdInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(TextBox_pwdInput.Text))
            {
                PasTip.Text = "用户密码不能为空！";
                PasTip.Visibility = Visibility.Visible;
            }
            else
            {
                PasTip.Visibility = Visibility.Hidden;
            }
        }
    }
}