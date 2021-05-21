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
    /// RegisterWindow.xaml 的交互逻辑
    /// </summary>
    public partial class RegisterWindow : Window
    {
        private User user;
        private string id = "";
        private string name = "";
        private string pwdInput = "";
        private string pwdCheck = "";

        public RegisterWindow()
        {
            InitializeComponent();
        }

        private void Button_MouseEnter(object sender, MouseEventArgs e)
        {
            Cursor = Cursors.Hand;
        }

        private void Button_MouseLeave(object sender, MouseEventArgs e)
        {
            Cursor = Cursors.Arrow;
        }

        private void Button_Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Button_Register_Click(object sender, RoutedEventArgs e)
        {

        }

        private void TextBox_pwdInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (TextBox_pwdInput.Text.Length <= pwdInput.Length)
            {
                pwdInput = pwdInput.Substring(0, TextBox_pwdInput.Text.Length);
                return;
            }
            pwdInput += TextBox_pwdInput.Text[TextBox_pwdInput.Text.Length - 1];
            string temp = "";
            for (int i = 0; i < TextBox_pwdInput.Text.Length; i++)
                temp += "*";
            TextBox_pwdInput.Text = temp;
            TextBox_pwdInput.Select(TextBox_pwdInput.Text.Length, 0);
        }

        private void TextBox_pwdCheck_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (TextBox_pwdCheck.Text.Length <= pwdCheck.Length)
            {
                pwdCheck = pwdCheck.Substring(0, TextBox_pwdCheck.Text.Length);
                return;
            }
            pwdCheck += TextBox_pwdCheck.Text[TextBox_pwdCheck.Text.Length - 1];
            string temp = "";
            for (int i = 0; i < TextBox_pwdCheck.Text.Length; i++)
                temp += "*";
            TextBox_pwdCheck.Text = temp;
            TextBox_pwdCheck.Select(TextBox_pwdCheck.Text.Length, 0);
        }

        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            TextBox_pwdInput.Text = pwdInput;
        }

        private void Image_MouseLeftButtonDown_2(object sender, MouseButtonEventArgs e)
        {
            TextBox_pwdCheck.Text = pwdCheck;
        }

        private void Image_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            string temp = "";
            for (int i = 0; i < pwdInput.Length; i++)
                temp += "*";
            TextBox_pwdInput.Text = temp;
            TextBox_pwdInput.Select(TextBox_pwdInput.Text.Length, 0);
        }

        private void Image_MouseLeftButtonUp_2(object sender, MouseButtonEventArgs e)
        {
            string temp = "";
            for (int i = 0; i < pwdCheck.Length; i++)
                temp += "*";
            TextBox_pwdCheck.Text = temp;
            TextBox_pwdCheck.Select(TextBox_pwdCheck.Text.Length, 0);
        }
    }
}
