using AppMovilAnuncios.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppMovilAnuncios
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new SimonDiceView());
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
