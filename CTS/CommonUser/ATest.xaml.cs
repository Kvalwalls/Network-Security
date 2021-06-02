using CommonUser.Kerberos;
using CommonUser.Transmission;
using System;
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
            try
            {
                ASHandler asHandler = ASHandler.GetInstatnce();
                String[] keyAndTicket = asHandler.ASCertification();
                Console.WriteLine(keyAndTicket[0]);
                Console.WriteLine(keyAndTicket[1]);
                asHandler.CloseASConnection();
                /*Socket socket = Connection.ConnectServer("127.0.0.1", 7000);
                Transceiver transceiver = new Transceiver(socket);
                TransMessage message = transceiver.ReceiveMessage();
                message.DePackage("..\\..\\KeyFiles\\AS.pk", "00000000");
                Console.WriteLine(message.fromAddress.Length);*/
                Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }
        }
    }
}
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

/* new TransMessage();
        message.fromAddress = new byte[4] { 127, 0, 0, 1 };
        message.toAddress = new byte[4] { 127, 0, 0, 2 };
        message.serviceType = 1;
        message.specificType = 1;
        message.errorCode = 0;
        message.cryptCode = 1;
        message.contents = (PicturePhaser.PictureToBase64("..\\..\\MoviePictures\\M00001.jpg"));
        message.EnPackage("..\\..\\KeyFiles\\Client.sk","00000000");
        int length = message.contents.Length;
        Console.WriteLine(message.contents);
        transceiver.SendMessage(message);*/
/*Socket socket = Connection.ConnectServer("127.0.0.1", 7000);
    Transceiver transceiver = new Transceiver(socket);
    TransMessage message = transceiver.ReceiveMessage();
    message.DePackage("..\\..\\KeyFiles\\Client.pk", "00000000");
    PicturePhaser.Base64ToPicture(message.contents,"rnm");*/

/*Socket socket = Connection.ConnectServer("127.0.0.1", 7000);
        Transceiver transceiver = new Transceiver(socket);
        TransMessage message = transceiver.ReceiveMessage();
        message.DePackage("..\\..\\KeyFiles\\AS.pk", "00000000");
        Console.WriteLine(message.errorCode);*/
