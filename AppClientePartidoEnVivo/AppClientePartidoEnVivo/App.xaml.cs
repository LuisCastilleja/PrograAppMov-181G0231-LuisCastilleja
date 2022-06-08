using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using AppClientePartidoEnVivo.Views;
namespace AppClientePartidoEnVivo
{
    public partial class App : Application
    {
        public static Action PartidosActualizados;
        public static Action LanzarEvento;

        public static void Actualizar()
        {
            PartidosActualizados?.Invoke();
        }
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new PartidosView());
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
