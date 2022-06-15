using AppClientePartidoEnVivo.Models;
using AppClientePartidoEnVivo.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using AppClientePartidoEnVivo.Helpers;
using Microsoft.Extensions.DependencyInjection;
using Xamarin.Essentials;
using System.Windows.Input;
using Xamarin.Forms;
using AppClientePartidoEnVivo.Views;
using System.Globalization;

namespace AppClientePartidoEnVivo.ViewModels
{
    public class PartidoViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Partido> ListaPartidos { get; set; } = new ObservableCollection<Partido>();
        public ObservableCollection<Partido> ListaPartidosFinalizados { get; set; } = new ObservableCollection<Partido>();
        PartidosRepository repository = new PartidosRepository();
        HttpClientHelper<Partido> Client;
        private PartidoCompletoView partidoCompletoView;
        private PartidoCompletoFinalizadoView partidoCompletoFinalizadoView;
        private Partido partido;

        public Partido Partido
        {
            get { return partido; }
            set { partido = value; OnPropertyChanged(); }
        }

        public ICommand VerPartidoCompleto { get; set; }
        public PartidoViewModel()
        {
            Uri uri = new Uri("https://181g0231.82g.itesrc.net/api/PartidoVivo");
            Client = new HttpClientHelper<Partido>(uri);
            bool key = Preferences.ContainsKey("FechaActualizada");
            if (!key)
            {
                DescargarPrimeraVez();
            }
            else
            {
                Actualizar();
            }
            App.PartidosActualizados += App_PartidosActualizados;
            VerPartidoCompleto = new Command(VerPartido);
        }

        private async void VerPartido(object obj)
        {
            var partidoCompleto = obj as Partido;
            if (partidoCompleto.EstadoPartido == "Finalizado")
            {
                if (partidoCompletoFinalizadoView == null)
                {
                    partidoCompletoFinalizadoView = new PartidoCompletoFinalizadoView() { BindingContext = this };
                }
                Partido = partidoCompleto;
                await App.Current.MainPage.Navigation.PushAsync(partidoCompletoFinalizadoView);
            }
            else
            {
                if (partidoCompletoView == null)
                {
                    partidoCompletoView = new PartidoCompletoView() { BindingContext = this };
                }
                Partido = partidoCompleto;
                await App.Current.MainPage.Navigation.PushAsync(partidoCompletoView);
            }
        }

        private async void DescargarPrimeraVez()
        {
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                var fecha = DateTime.Now;
                var listaPartidos = await Client.Get();
                foreach (var partido in listaPartidos)
                {
                    repository.Insert(partido);
                }
                Preferences.Set("FechaActualizada", fecha);
                Actualizar();
            }
            else
            {
                IToast toast = DependencyService.Get<IToast>();
                toast.MostrarToast("No tiene conexión a internet. Verifique su conexión a internet para ver los partidos actualizados");
            }
        }

        private void App_PartidosActualizados()
        {
            OnPropertyChanged();
            Actualizar();
        }

        public void Actualizar()
        {
            var lista = repository.GetAll();
            ListaPartidos.Clear();
            ListaPartidosFinalizados.Clear();
            foreach (var partido in lista)
            {
                if (partido.EstadoPartido == "En vivo")
                {
                    ListaPartidos.Add(partido);
                }
                else
                {
                    ListaPartidosFinalizados.Add(partido);
                }
            }

        }


        ~PartidoViewModel()
        {
            App.PartidosActualizados -= App_PartidosActualizados;
        }


        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string property = null)
        {
            PropertyChanged?.Invoke(this,
                new PropertyChangedEventArgs(property));
        }

    }
}
