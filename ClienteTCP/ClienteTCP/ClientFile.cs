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
using System.IO;
using Java.IO;
using Android.Telephony;

namespace ClienteTCP
{
   public class ClientFile
    {
        #region Propiedades
        public int Port { set; get; }
        public string Server { set; get; }
        /// <summary>
        /// Inlcuye el path del archivo.
        /// </summary>
        public string FileName { set; get; }

        public Context Context { set; get; }
        #endregion

        #region Variables

        TcpClient clientSocket = new TcpClient();
        #endregion

        #region Métodos

        public ClientFile(int aPort) {
            Port = aPort;
        }
        public ClientFile(int aPort, string aServer) {
            Port = aPort;
            Server = aServer;
        }

        public ClientFile(int aPort, string aServer, string aFileName)
        {
            Port = aPort;
            Server = aServer;
            FileName = aFileName;
        }

        public ClientFile() { }


        public void Conectar()
        {
            try
            {
                clientSocket.Connect(Server, Port);
            }
            catch (SocketException)
            {
                System.Console.WriteLine("Ocurrió un problema con el socket");
                throw new Exception("Problema en el socket.");
            }
            catch (ObjectDisposedException)
            {
                System.Console.WriteLine("El socket cliente se encuentra null");
                throw new Exception("El socket cliente no esta creado.");
            }
        }

        public string EnviarMensajeRecibirArchivo()
        {
            int bytesRead = 0;
            try
            {
                NetworkStream serverStream = clientSocket.GetStream();
                byte[] outStream = Encoding.ASCII.GetBytes("Enviar archivo a dispositivo$");
                serverStream.Write(outStream, 0, outStream.Length);
                serverStream.Flush();

                // Recibimos el archivo y lo guardamos.
                MemoryStream memoryStream = new MemoryStream();
                byte[] inStream = new byte[4096*8];
                do
                {
                    bytesRead = serverStream.Read(inStream, 0, inStream.Length);                    
                    memoryStream.Write(inStream, 0, bytesRead);
                } while (bytesRead > 0);

                
                if (!Directory.Exists(Path.GetDirectoryName(FileName)))
                    Directory.CreateDirectory(Path.GetDirectoryName(FileName));

                Java.IO.File file = new Java.IO.File(FileName);
                if (file.Exists())
                    file.Delete();

                try
                {
                    FileOutputStream outs = new FileOutputStream(file);
                    outs.Write(memoryStream.ToArray());
                    outs.Flush();
                    outs.Close();
                    return "Archivo recibido.";
                }
                catch (Exception ex)
                {
                    return "No se pudo recibir el archivo:\n" + ex.Message;
                }
            }
            catch (ArgumentOutOfRangeException)
            {
                throw new Exception("Algo anda mal con el tamaño del arreglo: " + bytesRead);
            }     
            catch(Exception ex)
            {
                throw new Exception("Error desconocido:\n" + ex.Message);
            }
        } // EnviarMensajeRecibirArchivo()


        public string EnviarMensajeEnviarArchivo()
        {
            // Se implementa el envio del archivo 
            int bytesRead = 0;
            try
            {
                TelephonyManager mTelephonyMgr;
                mTelephonyMgr = (TelephonyManager)Context.GetSystemService(Context.TelephonyService);
                string imeiTel = mTelephonyMgr.DeviceId;//"AAAA";

                NetworkStream serverStream = clientSocket.GetStream();
                byte[] outStream = Encoding.ASCII.GetBytes(imeiTel + "!$");
                serverStream.Write(outStream, 0, outStream.Length);
                serverStream.Flush();

                // Recibimos la respuesta y enviamos el archivo.

                byte[] inStream = new byte[4096];
                bytesRead = serverStream.Read(inStream, 0, inStream.Length);
                string returndata = Encoding.ASCII.GetString(inStream, 0, bytesRead);

                if (!(returndata == "Continuar"))
                    return "No es posible enviar el archivo";

                try
                {
                    StreamReader streamReader = new StreamReader(FileName);
                    var memoryStream = new MemoryStream();
                    streamReader.BaseStream.CopyTo(memoryStream);
                    memoryStream.Close();
                    streamReader.Close();

                    outStream = memoryStream.ToArray();
                    serverStream.Write(outStream, 0, outStream.Length);
                    serverStream.Flush();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                } // try-catch 

                return "Archivo enviado.";
            }
            catch (ArgumentOutOfRangeException)
            {
                throw new Exception("Algo anda mal con el tamaño del arreglo: " + bytesRead);
            }
            catch (Exception ex)
            {
                throw new Exception("Error desconocido:\n" + ex.Message);
            }
        }

        #endregion
    }
}