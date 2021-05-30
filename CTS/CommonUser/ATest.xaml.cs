using CommonUser.Kerberos;
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
            /*Socket socket = Connection.ConnectServer("127.0.0.1", 7000);
            Transceiver transceiver = new Transceiver(socket);
            TransMessage message = new TransMessage();
            message.fromAddress = new byte[4] { 127, 0, 0, 1 };
            message.toAddress = new byte[4] { 127, 0, 0, 2 };
            message.serviceType = 1;
            message.specificType = 1;
            message.errorCode = 0;
            message.cryptCode = 1;
            message.contents = "<Text><i>123456789</i></Text>";
            message.EnPackage("C:\\Users\\19705\\Desktop\\test.sk", "00000001");
            transceiver.SendMessage(message);*/
            /*byte[] vs = AddressPhaser.StringToBytes("127 .0 .0 .1 ");
            foreach (byte b in vs)
                Console.WriteLine(b.ToString());
            string str = AddressPhaser.BytesToString(vs);
            Console.WriteLine(str);*/
            /*ASHandler asHandler = ASHandler.GetInstatnce();
            asHandler.SendRequest();*/
            Close();
        }
    }
}
