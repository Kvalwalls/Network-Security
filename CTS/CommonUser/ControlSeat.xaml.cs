using CommonUser.Entity;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace CommonUser
{
    /// <summary>
    /// ControlSeat.xaml 的交互逻辑
    /// </summary>
    public partial class ControlSeat : UserControl
    {
        public byte status { get; set; }
        public ControlSeat(byte status)
        {
            this.status = status;
            InitializeComponent();
            InitBackImage();
        }

        private void ChangeImageSource(Image image, string path, bool isRelative)
        {
            BitmapImage bmp = new BitmapImage();
            bmp.BeginInit();
            if (isRelative)
                bmp.UriSource = new Uri(path, UriKind.Relative);
            else
                bmp.UriSource = new Uri(path, UriKind.Absolute);
            bmp.EndInit();
            image.Source = bmp;
        }

        private void InitBackImage()
        {
            if (status != EnumSeatStatus.Unselected)
                ChangeImageSource(BackImage, "ImageResources\\图标(黑)_已选座位.png", true);
            else
            {
                ChangeImageSource(BackImage, "ImageResources\\图标(黑)_可选座位.png", true);
                BackImage.MouseEnter += BackImage_MouseEnter;
                BackImage.MouseLeave += BackImage_MouseLeave;
                BackImage.MouseDown += BackImage_MouseDown;
            }
        }

        private void BackImage_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Cursor = Cursors.Hand;
        }

        private void BackImage_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Cursor = Cursors.Arrow;
        }

        private void BackImage_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (status == EnumSeatStatus.Unselected)
            {
                ChangeImageSource(BackImage, "ImageResources\\图标(黑)_在选座位.png", true);
                status = EnumSeatStatus.Selecting;
            }
            else if(status == EnumSeatStatus.Selecting)
            {
                ChangeImageSource(BackImage, "ImageResources\\图标(黑)_可选座位.png", true);
                status = EnumSeatStatus.Unselected;
            }
            else
            {
                throw new Exception("座位状态异常！");
            }
        }
    }
}
