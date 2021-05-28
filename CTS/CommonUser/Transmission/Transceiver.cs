using System.Net.Sockets;
using System.Text;

namespace CommonUser.Transmission
{
    class Transceiver
    {
        private readonly Socket socket;

        public Transceiver(Socket socket)
        {
            this.socket = socket;
        }

        public void SendMessage(TransMessage message)
        {
            byte[] buffer = message.MessageToBytes();
            socket.Send(buffer);
        }

        public TransMessage ReceiveMessage()
        {
            byte[] buffer = null;
            TransMessage message = new TransMessage();
            //源、目的IP地址字段
            buffer = new byte[4];
            socket.Receive(buffer);
            message.toAddress = buffer;
            socket.Receive(buffer);
            message.fromAddress = buffer;
            //其他控制字段
            buffer = new byte[1];
            socket.Receive(buffer);
            message.serviceType = buffer[0];
            socket.Receive(buffer);
            message.specificType = buffer[0];
            socket.Receive(buffer);
            message.errorCode = buffer[0];
            socket.Receive(buffer);
            message.cryptCode = buffer[0];
            //数字签名长度字段
            buffer = new byte[4];
            socket.Receive(buffer);
            int signLen = int.Parse(Encoding.UTF8.GetString(buffer).Trim());
            //报文内容长度字段
            buffer = new byte[4];
            socket.Receive(buffer);
            int contentLen = int.Parse(Encoding.UTF8.GetString(buffer).Trim());
            //数字签名字段
            buffer = new byte[signLen];
            socket.Receive(buffer);
            message.signature = Encoding.UTF8.GetString(buffer);
            //报文内容字段
            buffer = new byte[contentLen];
            socket.Receive(buffer);
            message.contents = Encoding.UTF8.GetString(buffer);
            return message;
        }

    }
}