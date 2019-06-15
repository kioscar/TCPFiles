﻿using System;
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
   public class ClientFile
    {
        #region Propiedades
        int Port { set; get; }
        string Server { set; get; }
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
        public ClientFile() { }


        public void Conectar()
        {
            try
            {
                clientSocket.Connect(Server, Port);
            }
            catch (SocketException)
            {
                Console.WriteLine("Ocurrió un problema con el socket");
                throw new Exception("Problema en el socket.");
            }
            catch (ObjectDisposedException)
            {
                Console.WriteLine("El socket cliente se encuentra null");
                throw new Exception("El socket cliente no esta creado.");
            }
        }

        public string Enviar()
        {
            int bytesRead = 0;
            try
            {
                NetworkStream serverStream = clientSocket.GetStream();
                byte[] outStream = Encoding.ASCII.GetBytes("Message from Client$");
                serverStream.Write(outStream, 0, outStream.Length);
                serverStream.Flush();

                byte[] inStream = new byte[4096];
                bytesRead = serverStream.Read(inStream, 0, inStream.Length/*(int)clientSocket.ReceiveBufferSize*/);
                string returndata = Encoding.ASCII.GetString(inStream, 0, bytesRead);
                return returndata;
            }
            catch (ArgumentOutOfRangeException)
            {
                throw new Exception("Algo anda mal con el tamaño del arreglo: " + bytesRead);
            }            
        }

        #endregion
    }
}