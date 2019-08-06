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
        private TcpListener serverSocket;
        private TcpClient clientSocket;

        public IServerFile subject;
        #endregion

        #region Propiedades
        public int Port { set; get; }
        public string Server { set; get; }
        public string RutaArchivo { set; get; }
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

        public ServerFile(string aServer, int aPort, string aRutaArchivo)
        {
            Port = aPort;
            Server = aServer;
            RutaArchivo = aRutaArchivo;
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

                serverSocket.Start();
               
                serverSocket.BeginAcceptTcpClient(new AsyncCallback(AcceptTcp), serverSocket);
            }
            catch (SocketException ex)
            {
                LevantaExcepcion(ex);
            }
            catch (ArgumentNullException ex)
            {
                LevantaExcepcion(ex);
            } // try - finally
        }

        private void AcceptTcp(IAsyncResult ar)
        {
            try
            {
                clientSocket = default;
                clientSocket = serverSocket.EndAcceptTcpClient(ar);
                HandlerClient hndlCliente = new HandlerClient();
                hndlCliente.subject = this.subject;
                hndlCliente.StartClient(clientSocket, RutaArchivo);

                // Lo dejamos escuchando para nuevas conexiones.
                serverSocket.BeginAcceptTcpClient(new AsyncCallback(AcceptTcp), serverSocket);
            }catch(Exception ex)
            {
                LevantaExcepcion(ex);
            }
        }

        private void LevantaExcepcion(Exception ex)
        {
            throw new Exception(ex.Message);
        }
        #endregion
    }
}
