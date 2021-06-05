using CommonUser.AppServices;
using CommonUser.Entity;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CommonUser
{
    /// <summary>
    /// RegisterWindow.xaml 的交互逻辑
    /// </summary>
    public partial class RegisterWindow : Window
	{
		private string UpwdInput = string.Empty;
		private string UpwdCheck = string.Empty;
        private CUVHandler handler;
		public RegisterWindow()
		{
			InitializeComponent();
            handler = CUVHandler.GetInstance();
        }

        //取消按钮函数
		private void Button_Cancel_Click(object sender, RoutedEventArgs e)
		{
			Close();
		}

        //注册按钮函数
        private void Button_Register_Click(object sender, RoutedEventArgs e)
        {
            if (TextBox_id.Text.Length > 20)
            {
                MessageBox.Show("账号长度不得超过20位！", "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (TextBox_name.Text.Length > 20)
            {
                MessageBox.Show("用户名长度不得超过20位！", "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (TextBox_pwdInput.Text.Length > 20 || TextBox_pwdCheck.Text.Length > 20)
            {
                MessageBox.Show("密码长度不得超过20位！", "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (!string.Empty.Equals(TextBox_id.Text) && !string.Empty.Equals(TextBox_name.Text) && !string.Empty.Equals(TextBox_pwdInput.Text) && !string.Empty.Equals(TextBox_pwdCheck.Text))
                if (!TextBox_pwdInput.Text.Equals(TextBox_pwdCheck.Text))
                {
                    MessageBox.Show("输入的密码不一致！", "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                    TextBox_pwdInput.Text = string.Empty;
                    TextBox_pwdCheck.Text = string.Empty;
                }
                else
                {
                    User user = new User();
                    user.Uid = TextBox_id.Text;
                    user.Uname = TextBox_name.Text;
                    user.Upassword = UpwdInput;
                    if (handler.Register(user))
                        MessageBox.Show("注册成功！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    else
                        MessageBox.Show("注册失败！账号已重复！", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
                    Close();
                }
            else
                MessageBox.Show("存在空白项！", "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        private void TextBox_pwdInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (TextBox_pwdInput.Text.Length <= UpwdInput.Length)
            {
                UpwdInput = UpwdInput.Substring(0, TextBox_pwdInput.Text.Length);
                return;
            }
            UpwdInput += TextBox_pwdInput.Text[TextBox_pwdInput.Text.Length - 1];
            string temp = "";
            for (int i = 0; i < TextBox_pwdInput.Text.Length; i++)
                temp += "*";
            TextBox_pwdInput.Text = temp;
            TextBox_pwdInput.Select(TextBox_pwdInput.Text.Length, 0);
        }

        private void TextBox_pwdCheck_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (TextBox_pwdCheck.Text.Length <= UpwdCheck.Length)
            {
                UpwdCheck = UpwdCheck.Substring(0, TextBox_pwdCheck.Text.Length);
                return;
            }
            UpwdCheck += TextBox_pwdCheck.Text[TextBox_pwdCheck.Text.Length - 1];
            string temp = "";
            for (int i = 0; i < TextBox_pwdCheck.Text.Length; i++)
                temp += "*";
            TextBox_pwdCheck.Text = temp;
            TextBox_pwdCheck.Select(TextBox_pwdCheck.Text.Length, 0);
        }

        private void Button_MouseEnter(object sender, MouseEventArgs e)
        {
            Cursor = Cursors.Hand;
        }
        private void Button_MouseLeave(object sender, MouseEventArgs e)
        {
            Cursor = Cursors.Arrow;
        }

        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            TextBox_pwdInput.Text = UpwdInput;
        }

        private void Image_MouseLeftButtonDown_2(object sender, MouseButtonEventArgs e)
        {
            TextBox_pwdCheck.Text = UpwdCheck;
        }

        private void Image_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            string temp = string.Empty;
            for (int i = 0; i < UpwdInput.Length; i++)
                temp += "*";
            TextBox_pwdInput.Text = temp;
            TextBox_pwdInput.Select(TextBox_pwdInput.Text.Length, 0);
        }

        private void Image_MouseLeftButtonUp_2(object sender, MouseButtonEventArgs e)
        {
            string temp = string.Empty;
            for (int i = 0; i < UpwdCheck.Length; i++)
                temp += "*";
            TextBox_pwdCheck.Text = temp;
            TextBox_pwdCheck.Select(TextBox_pwdCheck.Text.Length, 0);
        }
    }
}
