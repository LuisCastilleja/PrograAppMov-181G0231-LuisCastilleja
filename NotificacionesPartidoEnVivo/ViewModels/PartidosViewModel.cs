using GalaSoft.MvvmLight.Command;
using NotificacionesPartidoEnVivo.Helpers;
using NotificacionesPartidoEnVivo.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using NotificacionesPartidoEnVivo.Repositories;
using System.Net.Http;

namespace NotificacionesPartidoEnVivo.ViewModels
{
    public enum Vistas { Agregar, Editar, Eliminar, VerPartidos }
    public class PartidosViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public ObservableCollection<Partido> ListaPartidos { get; set; } = new ObservableCollection<Partido>();
        public ObservableCollection<string> Estados { get; set; } = new ObservableCollection<string>();
        public Vistas Vistas { get; set; } = Vistas.VerPartidos;
        public HttpClientHelper<Partido> Client;
        public PartidoEnVivoRepository repository = new PartidoEnVivoRepository();

        private Partido partido = new Partido();
        private string error = "";

        public Partido Partido
        {
            get { return partido; }
            set
            {
                partido = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Partido"));
            }
        }

        public string Error
        {
            get { return error; }
            set
            {
                error = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Error"));
            }
        }
        public ICommand VerAgregarCommand { get; set; }
        public ICommand VerEditarCommand { get; set; }
        public ICommand VerEliminarCommand { get; set; }

        //Para aceptar la edicion o aceptar al agregar un partido
        public ICommand GuardarCommand { get; set; }
        public ICommand CancelarCommand { get; set; }
        public ICommand EliminarCommand { get; set; }
        public PartidosViewModel()
        {
            Estados.Add("En vivo");
            Estados.Add("Finalizado");
            Uri uri = new("https://181g0231.82g.itesrc.net/api/PartidoVivo");
            Client = new HttpClientHelper<Partido>(uri!);
            _ = DescargarPartidos();
            VerAgregarCommand = new RelayCommand(VerAgregar);
            VerEditarCommand = new RelayCommand(VerEditar);
            VerEliminarCommand = new RelayCommand(VerEliminar);
            GuardarCommand = new RelayCommand(Guardar);
            CancelarCommand = new RelayCommand(Cancelar);
            EliminarCommand = new RelayCommand(Eliminar);
        }

        private void VerEliminar()
        {
            if (Partido != null)
            {
                Error = "";
                Vistas = Vistas.Eliminar;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null));
            }
        }

        private void VerEditar()
        {
            if (Partido != null)
            {
                Error = "";
                Vistas = Vistas.Editar;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null));
            }      
        }

        private async void Eliminar()
        {
            if (Partido != null)
            {
                try
                {
                    await Client.Delete(Partido.Id);
                    Vistas = Vistas.VerPartidos;
                    Error = "";
                    _ = DescargarPartidos();
                }
                catch(Exception ex)
                {
                    Error = ex.Message;
                }
               
            }
            else
            {
                Error = "Seleccione una actualizacion para eliminarla.";
            }
        }

        public async Task DescargarPartidos()
        {
            ListaPartidos = new();
            var lista = await Client.Get();
            foreach (var partido in lista)
            {
                ListaPartidos.Add(partido);
            }
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null));
        }
        private void Cancelar()
        {
            Partido = null!;
            Error = "";
            Vistas = Vistas.VerPartidos;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null));
        }

        private async void Guardar()
        {
            if(Partido != null)
            {
                //Verificar si se va a agregar o editar
                //Si no tiene Id es porque se agregara
                if(Partido.Id == 0)
                {
                    try
                    {
                        if(repository.IsValid(Partido,out List<String> errors))
                        {
                            //Si no hay excepciones hacemos el post.
                           await Client.Post(Partido);
                            Vistas = Vistas.VerPartidos;
                            Error = "";
                            _ = DescargarPartidos();
                        }
                        else
                        {
                            Error = errors.FirstOrDefault()!;
                        }
                    }
                    catch(HttpRequestException ex)
                    {
                        Error = ex.Message;
                    }
                }
                else
                {
                    try
                    {
                        //Para editarr
                        if (repository.IsValid(Partido, out List<String> errors))
                        {
                            //Si no hay excepciones hacemos el post.
                            await Client.Put(Partido);
                            Vistas = Vistas.VerPartidos;
                            Error = "";
                            _ = DescargarPartidos();
                        }
                        else
                        {
                            Error = errors.FirstOrDefault()!;
                        }
                    }
                    catch (HttpRequestException ex)
                    {
                        Error = ex.Message;
                    }
                }
            }
        }

        private void VerAgregar()
        {
            Partido = new Partido();
            Error = "";
            Vistas = Vistas.Agregar;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null));
        }
    }
}
