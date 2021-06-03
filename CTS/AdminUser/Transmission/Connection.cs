using System.Net;
using System.Net.Sockets;
using System.Text;

namespace AdminUser.Transmission
{
    class Connection
    {
        /// <summary>
        /// 连接服务器函数
        /// </summary>
        /// <param name="IPBytes">byte格式的IP地址</param>
        /// <param name="port">端口号</param>
        /// <returns>连接Socket</returns>
        public static Socket ConnectServer(byte[] IPBytes, int port)
        {
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPAddress IPAddr = IPAddress.Parse(Encoding.UTF8.GetString(IPBytes));
            IPEndPoint server = new IPEndPoint(IPAddr, port);
            socket.Connect(server);
            return socket;
        }

        /// <summary>
        /// 连接服务器函数
        /// </summary>
        /// <param name="IPStr">string格式的IP地址</param>
        /// <param name="port">端口号</param>
        /// <returns>连接Socket</returns>
        public static Socket ConnectServer(string IPStr, int port)
        {
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPAddress IPAddr = IPAddress.Parse(IPStr);
            IPEndPoint server = new IPEndPoint(IPAddr, port);
            socket.Connect(server);
            return socket;
        }
    }
}
