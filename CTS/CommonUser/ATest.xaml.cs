using CommonUser.Transmission;
using System;
using System.Configuration;
using System.Net.Sockets;
using System.Windows;

namespace CommonUser
{
    /// <summary>
    /// ATest.xaml 的交互逻辑
    /// </summary>
    public partial class ATest : Window
    {
        public ATest()
        {
            InitializeComponent();
            Hide();
            Socket socket = Connection.ConnectServer("127.0.0.1", 7000);
            Transceiver transceiver = new Transceiver(socket);
            TransMessage message = transceiver.ReceiveMessage();
            message.DePackage("C:\\Users\\19705\\Desktop\\test.pk","00000000");
            Console.WriteLine(message.errorCode);
            Console.WriteLine(message.contents);
            Close();
        }
    }
}
