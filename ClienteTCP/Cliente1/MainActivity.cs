using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using ClienteTCP;
using System;
using System.IO;

namespace Cliente1
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            var btnRecibir = FindViewById<Button>(Resource.Id.btnRecibir);
            btnRecibir.Click += BtnRecibir_Click;

            // Limpiar textbox
            var btnLimpiar = FindViewById<Button>(Resource.Id.btnLimpiar);
            btnLimpiar.Click += BtnLimpiar_Click;

            // Enviar el archivo al servidor 
            var btnEnviar = FindViewById<Button>(Resource.Id.btnEnviar);
            btnEnviar.Click += BtnEnviar_Click;
        }

        private void BtnEnviar_Click(object sender, EventArgs e)
        {
            var txtServidor = FindViewById<TextView>(Resource.Id.edtServidor);
            var txtPort = FindViewById<TextView>(Resource.Id.edtPuerto);

            Toast.MakeText(this, "IP: " + txtServidor.Text + ", Puerto: " +
                txtPort.Text, ToastLength.Long).Show();

            try
            {
                var rutaArchivo = Path.Combine(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath, "XMLS");
                rutaArchivo = Path.Combine(rutaArchivo, "count.xml");

                ClientFile clienteTcp = new ClientFile(aPort: int.Parse(txtPort.Text), aServer: txtServidor.Text, aFileName: rutaArchivo);
                clienteTcp.Context = this;
                clienteTcp.Conectar();
                var edtMensaje = FindViewById<EditText>(Resource.Id.edtMensaje);
                edtMensaje.Text += clienteTcp.EnviarMensajeEnviarArchivo() + "\n";
            }
            catch (Exception ex)
            {
                var edtMensaje = FindViewById<EditText>(Resource.Id.edtMensaje);
                edtMensaje.Text += ex.Message + "\n";
            }

        }

        private void BtnLimpiar_Click(object sender, EventArgs e)
        {
            var edtMensaje = FindViewById<EditText>(Resource.Id.edtMensaje);
            edtMensaje.Text = "";
        }

        private void BtnRecibir_Click(object sender, System.EventArgs e)
        {
            var txtServidor = FindViewById<TextView>(Resource.Id.edtServidor);
            var txtPort = FindViewById<TextView>(Resource.Id.edtPuerto);

            Toast.MakeText(this, "IP: " + txtServidor.Text + ", Puerto: " + 
                txtPort.Text, ToastLength.Long).Show();
            
            try
            {
                var rutaArchivo = Path.Combine(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath, "XMLS");
                rutaArchivo = Path.Combine(rutaArchivo, "archivo.xml");

                ClientFile clienteTcp = new ClientFile(aPort: int.Parse(txtPort.Text), aServer: "192.168.0.29", aFileName: rutaArchivo);
                clienteTcp.Conectar();
                var edtMensaje = FindViewById<EditText>(Resource.Id.edtMensaje);
                edtMensaje.Text += clienteTcp.EnviarMensajeRecibirArchivo() + "\n";
            }
            catch(Exception ex)
            {
                var edtMensaje = FindViewById<EditText>(Resource.Id.edtMensaje);
                edtMensaje.Text += ex.Message + "\n";
            }
        }
    }
}