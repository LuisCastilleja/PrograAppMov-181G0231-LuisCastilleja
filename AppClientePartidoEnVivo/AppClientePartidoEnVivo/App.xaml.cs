using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using AppClientePartidoEnVivo.Views;
using Microsoft.Extensions.DependencyInjection;
using AppClientePartidoEnVivo.Repositories;

namespace AppClientePartidoEnVivo
{
    public partial class App : Application
    {
        public static IServiceProvider ServiceProvider { get; set; }
        public static event Action PartidosActualizados;

        public static void Actualizar()
        {
            PartidosActualizados?.Invoke();
        }
        public App()
        {
            InitializeComponent();
            SetupServices();
            MainPage = new NavigationPage(new PartidosTabbedView());
        }
        void SetupServices()
        {
            var services = new ServiceCollection();
            services.AddSingleton<PartidosRepository>();
            ServiceProvider = services.BuildServiceProvider();
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
