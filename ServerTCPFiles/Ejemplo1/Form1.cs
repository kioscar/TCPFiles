using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

using ServerFiles;
namespace Ejemplo1
{
    public partial class MainFrm : Form
    {
        ServerFile server;
        const string rutaArchivo = @"C:\Archivos\archivo.xml";

        public MainFrm()
        {
            InitializeComponent();
        }

        private void btnIniciar_Click(object sender, EventArgs e)
        {

            try
            {
                server = new ServerFile(txtServer.Text, Convert.ToInt32(txtPort.Text), rutaArchivo);
                server.Start();
                /*Thread ServidorThread = new Thread(DoSomething);
                ServidorThread.Start();*/
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrió un problema: \n" + ex.Message);
            }

        }

        private void DoSomething()
        {
           
        }

        private void MainFrm_Leave(object sender, EventArgs e)
        {
        }
    }
}
