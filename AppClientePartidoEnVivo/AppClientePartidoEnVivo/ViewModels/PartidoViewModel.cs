using AppClientePartidoEnVivo.Models;
using AppClientePartidoEnVivo.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using AppClientePartidoEnVivo.Helpers;
using Microsoft.Extensions.DependencyInjection;

namespace AppClientePartidoEnVivo.ViewModels
{
    public class PartidoViewModel:INotifyPropertyChanged
    {
        public ObservableCollection<Partido> ListaPartidos { get; set; } = new ObservableCollection<Partido>();
        PartidosRepository repository;

        public PartidoViewModel()
        {
            App.PartidosActualizados += App_PartidosActualizados;
            Actualizar();

        }

        private void App_PartidosActualizados()
        {
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
