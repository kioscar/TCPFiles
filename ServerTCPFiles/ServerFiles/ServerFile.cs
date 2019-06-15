using System;
using System.Collections.Generic;
using System.Text;

// using utilizados para Sockets
using System.Net;
using System.Net.Sockets;

//Multi-Thread
using System.Threading;
namespace ServerFiles
{
    public class ServerFile
    {
        #region Variables
        TcpListener serverSocket;
        TcpClient clientSocket;
        #endregion

        #region Propiedades
        public int Port { set; get; }
        public string Server { set; get; }
        #endregion

        #region Constructores
        /// <summary>
        /// Constructor vacío.
        /// </summary>
        public ServerFile(){}

        /// <summary>
        /// Constructor con puerto indicado.
        /// </summary>
        /// <param name="aPort"> Puerto sobre el que se levantará el servidor.</param>
        public ServerFile(int aPort)
        {
            this.Port = aPort;
        }

        /// <summary>
        /// Constructor con server y puerto.
        /// </summary>
        /// <param name="aServer">IP del servidor</param>
        /// <param name="aPort">Puerto </param>
        public ServerFile(string aServer, int aPort)
        {
            Port = aPort;
            Server = aServer;
        }

        #endregion

        #region Destructor
        ~ServerFile()
        {
            serverSocket.Stop();         
        }
        #endregion


        #region Métodos
        public void Start()
        {
            try
            {
                serverSocket = new TcpListener(IPAddress.Parse(Server), Port);
                clientSocket = default(TcpClient);

                serverSocket.Start();

                while (true)
                {
                    // Aceptamos la conexión del cliente y la manejamos en otro Thread.
                    clientSocket =  serverSocket.AcceptTcpClient();
                    HandlerClient hndlCliente = new HandlerClient();
                    hndlCliente.StartClient(clientSocket);
                }

            }
            catch (SocketException Arg)
            {
                Console.WriteLine(Arg.Message);
            }
            catch (ArgumentNullException Arg)
            {
                Console.WriteLine(Arg.Message);
            } // try - finally
        }
        #endregion
    }
}
