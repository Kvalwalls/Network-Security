using CommonUser.Transmission;
using System;
using System.Configuration;
using System.Net.Sockets;

namespace CommonUser.AppServices
{
    class AUVHandler : VHandler
    {
        private string sessionKey;
        private static AUVHandler instance = new AUVHandler();
        private AUVHandler()
        {
            Socket socket = Connection.ConnectServer(
               IPStr: ConfigurationManager.AppSettings["V_IPAddress"],
               int.Parse(ConfigurationManager.AppSettings["V_Port"]));
            transceiver = new Transceiver(socket);
            if (transceiver == null)
                throw new Exception("Transceiver错误！");
        }
        public static AUVHandler GetInstance()
        {
            return instance;
        }
    }
}
