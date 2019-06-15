using System;
using System.Collections.Generic;
using System.Text;

// using utilizados para Sockets
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace ServerFiles
{
    class HandlerClient
    {

        #region Variables
        TcpClient clientSocket;

        #endregion

        #region Métodos

        public void StartClient(TcpClient inClientSocket)
        {
            clientSocket = inClientSocket;;
            
            try
            {
                Thread ctThread = new Thread(DoAction);
                ctThread.Start();
            }
            catch (ThreadStartException)
            {
                throw new Exception("Error al iniciar servicio del thread.");
            }
            catch (Exception)
            {
                throw new Exception("Error al iniciar servicio en general.");
            }

        }

        private void DoAction()
        {
            byte[] bytesFrom = new byte[10025];
            string dataFromClient = null;
            Byte[] sendBytes = null;
            string serverResponse = null;

            while ((true))
            {
                try
                {
                    NetworkStream networkStream = clientSocket.GetStream();
                    networkStream.Read(bytesFrom, 0, (int)clientSocket.ReceiveBufferSize);
                    dataFromClient = Encoding.ASCII.GetString(bytesFrom);
                    dataFromClient = dataFromClient.Substring(0, dataFromClient.IndexOf("$"));

                    Console.WriteLine(" >> " + dataFromClient);

                    serverResponse = "Server to clinet('Hi this is my response')";
                    sendBytes = Encoding.ASCII.GetBytes(serverResponse);
                    networkStream.Write(sendBytes, 0, sendBytes.Length);
                    networkStream.Flush();
                    Console.WriteLine(" >> " + serverResponse);
                }
                catch(ObjectDisposedException)
                {
                    Console.WriteLine("El NetworkStream del client esta vacío.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(" >> " + ex.ToString());
                } // try - finally
            } // while
        } // DoAction
        #endregion
    }
}
