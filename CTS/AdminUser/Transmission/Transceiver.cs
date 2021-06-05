using System.IO;
using System.Net.Sockets;
using System.Text;
using 服务器UI;

namespace AdminUser.Transmission
{
    class Transceiver
    {
        private readonly Socket socket;

        /// <summary>
        /// 带参数的构造函数
        /// </summary>
        public Transceiver(Socket socket)
        {
            this.socket = socket;
        }

        /// <summary>
        /// 报文发送函数
        /// </summary>
        /// <param name="message">发送的报文对象</param>
        public void SendMessage(TransMessage message)
        {
            byte[] buffer = message.MessageToBytes();
            socket.Send(buffer);
            CatchPackage(message);
        }

        /// <summary>
        /// 报文接收函数
        /// </summary>
        /// <returns>接收的报文对象</returns>
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
            int signLen = IntBytesPhaser.BytesToInt(buffer);
            //报文内容长度字段
            buffer = new byte[4];
            socket.Receive(buffer);
            int contentLen = IntBytesPhaser.BytesToInt(buffer);
            //数字签名字段
            buffer = new byte[signLen];
            socket.Receive(buffer);
            message.signature = Encoding.UTF8.GetString(buffer);
            //报文内容字段
            buffer = new byte[contentLen];
            socket.Receive(buffer);
            message.contents = Encoding.UTF8.GetString(buffer);
            CatchPackage(message);
            return message;
        }

        /// <summary>
        /// 关闭数据收发器函数
        /// </summary>
        public void CloseTransceiver()
        {
            socket.Shutdown(SocketShutdown.Receive);
            socket.Shutdown(SocketShutdown.Send);
            socket.Close();
        }

        public void CatchPackage(TransMessage transMessage)
        {
            string filename = "..\\..\\Package\\package.txt";
            FileStream fs;
            try
            {
                fs = File.Create(filename);
            }
            catch (IOException ex)
            {
                return;
            }
            
            try
            {
                fs.Write(transMessage.fromAddress, 0, transMessage.fromAddress.Length);
                fs.Write(transMessage.toAddress, 0, transMessage.fromAddress.Length);
                fs.Flush();
            }
            catch (IOException ex)
            {
                //Console.WriteLine(ex.Message);
            }
            finally
            {
                fs.Close();
            }
        }
    }
}