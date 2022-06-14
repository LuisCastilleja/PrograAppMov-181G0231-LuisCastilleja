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

namespace AppClientePartidoEnVivo.ViewModels
{
    public class PartidoViewModel:INotifyPropertyChanged
    {
        public ObservableCollection<Partido> ListaPartidos { get; set; } = new ObservableCollection<Partido>();
        PartidosRepository repository = new PartidosRepository();
        HttpClientHelper<Partido> Client;
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
            
        }

        private async void DescargarPrimeraVez()
        {
            if(Connectivity.NetworkAccess == NetworkAccess.Internet)
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
            foreach (var partido in lista)
            {
                ListaPartidos.Add(partido);
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
