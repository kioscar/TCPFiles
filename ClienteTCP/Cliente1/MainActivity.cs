using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using ClienteTCP;
using System;

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

            var btnEnviar = FindViewById<Button>(Resource.Id.btntEnviar);
            btnEnviar.Click += BtnEnviar_Click;
        }

        private void BtnEnviar_Click(object sender, System.EventArgs e)
        {
            var txtServidor = FindViewById<TextView>(Resource.Id.edtServidor);
            var txtPort = FindViewById<TextView>(Resource.Id.edtPuerto);

            Toast.MakeText(this, "IP: " + txtServidor.Text + ", Puerto: " + 
                txtPort.Text, ToastLength.Long).Show();
            
            try
            {
                ClientFile clienteTcp = new ClientFile(aPort: int.Parse(txtPort.Text), aServer: "192.168.0.29");
                clienteTcp.Conectar();
                var edtMensaje = FindViewById<EditText>(Resource.Id.edtMensaje);
                edtMensaje.Text += clienteTcp.Enviar() + "\n";
            }
            catch(Exception ex)
            {
                var edtMensaje = FindViewById<EditText>(Resource.Id.edtMensaje);
                edtMensaje.Text += ex.Message + "\n";
            }
        }
    }
}