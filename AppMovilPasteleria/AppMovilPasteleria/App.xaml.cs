using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using AppMovilPasteleria.Views;
using AppMovilPasteleria.Models;
using AppMovilPasteleria.Services;
namespace AppMovilPasteleria
{
    public partial class App : Application
    {

        public static CatalogoVentasPasteles Catalogo { get; set; }
        public static SincronizadorService Sincronizador { get; set; }
        public App()
        {
            InitializeComponent();
            Catalogo = new CatalogoVentasPasteles();
            Sincronizador = new SincronizadorService(Catalogo);
            MainPage = new NavigationPage(new MainPage());
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
