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
                LevantaExcepcion( new Exception("Error al iniciar servicio del thread."));
            }
            catch (Exception)
            {
                LevantaExcepcion( new Exception("Error al iniciar servicio en general."));
            }

        }

        private void DoAction()
        {
            byte[] bytesFrom = new byte[4096];
            try
            {
                NetworkStream networkStream = clientSocket.GetStream();
                networkStream.Read(bytesFrom, 0, bytesFrom.Length);
                string dataFromClient = Encoding.ASCII.GetString(bytesFrom);
                dataFromClient = dataFromClient.Substring(0, dataFromClient.IndexOf("$"));

                // Si encontramos en la cadena este sibolo ! es para recibir un archivo desde el cliente
                if (dataFromClient.Contains("!"))
                {
                    var ArrNombreArchivo = dataFromClient.Split('!');
                    string IdDispositivo = ArrNombreArchivo[0];
                    fileName = Path.Combine(Path.GetDirectoryName(fileName), IdDispositivo + "_count.xml");

                    var Response = Encoding.ASCII.GetBytes("Continuar");
                    networkStream.Write(Response, 0, Response.Length);
                    networkStream.Flush();

                    // Recibimos ahora el archivo y lo guardamos.
                    int bytesRead = 0;
                    try
                    {
                        MemoryStream memoryStream = new MemoryStream();
                        byte[] inStream = new byte[4096 * 8];
                        bytesRead = networkStream.Read(inStream, 0, inStream.Length);
                        memoryStream.Write(inStream, 0, bytesRead);

                        if (!Directory.Exists(Path.GetDirectoryName(fileName)))
                            Directory.CreateDirectory(Path.GetDirectoryName(fileName));


                        // Guardar el archivo 
                        var ArchivoRecibido = File.Create(fileName);
                        memoryStream.Seek(0, SeekOrigin.Begin);
                        memoryStream.CopyTo(ArchivoRecibido);
                        ArchivoRecibido.Close();
                        Console.WriteLine(" >> Recibido Correctamente");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                else
                {

                    Console.WriteLine(" >> " + dataFromClient);

                    // Cargamos el archivo para enviarlo 
                    StreamReader streamReader = new StreamReader(fileName);
                    var memoryStream = new MemoryStream();
                    streamReader.BaseStream.CopyTo(memoryStream);
                    byte[] sendBytes = memoryStream.ToArray();
                    memoryStream.Close();
                    streamReader.Close();

                    //sendBytes =  Encoding.ASCII.GetBytes(serverResponse);
                    networkStream.Write(sendBytes, 0, sendBytes.Length);
                    networkStream.Flush();
                }
            }
            catch (ObjectDisposedException)
            {
                LevantaExcepcion(new Exception("Stream null."));
            }
            catch (Exception ex)
            {
                LevantaExcepcion(ex);
            }
            finally
            {
                clientSocket.Close();
            }// try - finally
        } // DoAction


        private void LevantaExcepcion(Exception ex)
        {
            throw new Exception(ex.Message);
        }
        #endregion
    }
}
