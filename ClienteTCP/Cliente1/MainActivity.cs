using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;

namespace Cliente1
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            //var btnEnviar = FindViewById<Button>(Resource.Id.btntEnviar);
            //btnEnviar.Click += BtnEnviar_Click;
        }

        private void BtnEnviar_Click(object sender, System.EventArgs e)
        {
            var txtServidor = FindViewById<TextView>(Resource.Id.txtIpServidor);
            var txtPort = FindViewById<TextView>(Resource.Id.txtPort);

            Toast.MakeText(this, "IP: " + txtServidor.Text + ", Puerto: " + 
                txtPort.Text, ToastLength.Long).Show();
        }
    }
}