using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using System.Net;
using System.Net.Sockets;
namespace ClienteTCP
{
    class ClientFile
    {
        #region Propiedades
        int Port { set; get; }
        string Server { set; get; }
        #endregion

        #region Variables

        TcpClient socketCliente;

        #endregion

        #region Métodos
        public ClientFile(int aPort) {
            Port = aPort;
        }
        public ClientFile(int aPort, string aServer) {
            Port = aPort;
            Server = aServer;
        }
        public ClientFile() { }

        #endregion
    }
}