using CommonUser.Transmission;
using System;
using System.Configuration;
using System.Net.Sockets;

namespace CommonUser.Kerberos
{
    class VHandler
    {
        private readonly Transceiver transceiver;
        private static VHandler instance = new VHandler();
        private VHandler()
        {
            Socket socket = Connection.ConnectServer(
               ConfigurationManager.AppSettings["V_IPAddress"],
               int.Parse(ConfigurationManager.AppSettings["V_Port"]));
            transceiver = new Transceiver(socket);
            if (transceiver == null)
                throw new Exception("Transceiver错误！");
        }
    }
}
