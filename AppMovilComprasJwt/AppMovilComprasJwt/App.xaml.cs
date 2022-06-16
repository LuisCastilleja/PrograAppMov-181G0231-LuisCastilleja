using AppMovilComprasJwt.Services;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppMovilComprasJwt
{
    public partial class App : Application
    {
        public static UserService User { get; set; } = new UserService();

        public App()
        {
            InitializeComponent();
            User.Redirigir();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
