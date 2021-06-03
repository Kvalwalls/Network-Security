using CommonUser.Transmission;
using System;
using System.Configuration;
using System.Net.Sockets;

namespace CommonUser.AppServices
{
    class CUVHandler : VHandler
    {
        private string sessionKey;
        private static CUVHandler instance = new CUVHandler();
        private CUVHandler()
        {
            Socket socket = Connection.ConnectServer(
               IPStr: ConfigurationManager.AppSettings["V_IPAddress"],
               int.Parse(ConfigurationManager.AppSettings["V_Port"]));
            transceiver = new Transceiver(socket);
            if (transceiver == null)
                throw new Exception("Transceiver错误！");
        }
        public static CUVHandler GetInstance()
        {
            return instance;
        }
    }
}
