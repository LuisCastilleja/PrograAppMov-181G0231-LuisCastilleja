using AppClientePartidoEnVivo.Models;
using AppClientePartidoEnVivo.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;

namespace AppClientePartidoEnVivo.ViewModels
{
    public class PartidoViewModel:INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string property = null)
        {
            PropertyChanged?.Invoke(this,
                new PropertyChangedEventArgs(property));
        }

        PartidosRepository repository = new PartidosRepository();


        public ObservableCollection<Partido> ListaPartidos { get; set; } = new ObservableCollection<Partido>();

        public PartidoViewModel()
        {
            App.PartidosActualizados += Actualizar;
            Actualizar();

        }

        public void Actualizar()
        {
            var datos = repository.GetAll();
            ListaPartidos.Clear();
            foreach (var partido in ListaPartidos)
            {
                ListaPartidos.Add(partido);
            }
        }
    }
}
