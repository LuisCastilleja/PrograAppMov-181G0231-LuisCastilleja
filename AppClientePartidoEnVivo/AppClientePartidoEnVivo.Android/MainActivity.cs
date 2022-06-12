using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using Firebase.Messaging;

namespace AppClientePartidoEnVivo.Droid
{
    [Activity(Label = "AppClientePartidoEnVivo", Icon = "@mipmap/ic_launcher", Theme = "@style/MainTheme", MainLauncher = false, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize )]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {

        public static Activity CurrentActivity { get; set; }
        protected override void OnCreate(Bundle savedInstanceState)
        {
            CurrentActivity = this;
            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App());

            //Suscribirse a nuestro topic
            FirebaseMessaging.Instance.SubscribeToTopic("partidos");

            NotificationManager manager = Application.Context
                .GetSystemService(Application.NotificationService) as NotificationManager;

            manager.CreateNotificationChannel(new NotificationChannel("CANALPARTIDO", "Canal de los partidos", NotificationImportance.Max));

        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}