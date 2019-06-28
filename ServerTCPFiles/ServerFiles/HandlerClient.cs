using System;
using System.Collections.Generic;
using System.Text;

// using utilizados para Sockets
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;

namespace ServerFiles
{
    class HandlerClient
    {


        #region Variables
        TcpClient clientSocket;
        string fileName;

        #endregion

        #region Métodos

        public void StartClient(TcpClient inClientSocket, string aNombreArchivo)
        {
            clientSocket = inClientSocket;
            fileName = aNombreArchivo;
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
            byte[] bytesFrom = new byte[4096];
            string dataFromClient = null;
            byte[] sendBytes = null;
            string serverResponse = null;
            try
            {
                NetworkStream networkStream = clientSocket.GetStream();
                networkStream.Read(bytesFrom, 0, bytesFrom.Length);
                dataFromClient = Encoding.ASCII.GetString(bytesFrom);
                dataFromClient = dataFromClient.Substring(0, dataFromClient.IndexOf("$"));

                Console.WriteLine(" >> " + dataFromClient);

                //serverResponse = "Server to clinet('Hi this is my response')";

                // Cargamos el archivo para enviarlo 
                StreamReader streamReader = new StreamReader(fileName);
                var memoryStream = new MemoryStream();
                streamReader.BaseStream.CopyTo(memoryStream);
                sendBytes = memoryStream.ToArray();
                memoryStream.Close();
                streamReader.Close();

                //sendBytes =  Encoding.ASCII.GetBytes(serverResponse);
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
            }
            finally
            {
                clientSocket.Close();
            }// try - finally
        } // DoAction
        #endregion
    }
}
