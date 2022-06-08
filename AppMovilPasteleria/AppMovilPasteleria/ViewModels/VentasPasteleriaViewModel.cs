using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;
using AppMovilPasteleria.Views;
using Xamarin.Forms;
using AppMovilPasteleria.Models;
using System.Collections.ObjectModel;
using System.Linq;

namespace AppMovilPasteleria.ViewModels
{
    public class VentasPasteleriaViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Pasteles> ListaVentas { get; set; } = new ObservableCollection<Pasteles>();
        public event PropertyChangedEventHandler PropertyChanged;
        private AgregarVentaPastelView agregarVentaPastelView;
        private EditarVentaPastelView editarVentaPastelView;
        private MainPage mainPageView;
        private Pasteles pastel;
        private string filtro;

        public string Filtro
        {
            get { return filtro; }
            set { filtro = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Filtro")); }
        }




        public Pasteles Pastel
        {
            get { return pastel; }
            set { pastel = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Pastel")); }
        }
        private string errors;

        public string Errores
        {
            get { return errors; }
            set { errors = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Errores")); }
        }

        public DateTime FechaActual { get; set; }

        public ICommand VerAgregarCommand { get; set; }
        public ICommand VerEditarCommand { get; set; }
        public ICommand AgregarCommand { get; set; }
        public ICommand EditarCommand { get; set; }
        public ICommand EliminarCommand { get; set; }
        public ICommand CancelarCommand { get; set; }
        public ICommand FiltarCommand { get; set; }
        public ICommand Drag { get; set; }

        public VentasPasteleriaViewModel()
        {
            VerAgregarCommand = new Command(VerAgregar);
            VerEditarCommand = new Command(VerEditar);
            AgregarCommand = new Command(AgregarVenta);
            EditarCommand = new Command(EditarVenta);
            EliminarCommand = new Command(EliminarVenta);
            CancelarCommand = new Command(Cancelar);
            FiltarCommand = new Command(Filtrar);
            Drag = new Command(DragStarts);
            FechaActual = DateTime.Now;
            Actualizar();
            App.Sincronizador.Sincronizado += Sincronizador_Sincronizado;
        }

        private async void Filtrar(object obj)
        {
            Errores = "";
            Filtro = obj as string;
            if (Filtro == null)
            {
                await App.Current.MainPage.DisplayAlert("Mensaje", "Proporcione el nombre del pastel para filtrar", "Aceptar");
            }
            else
            {
                ListaVentas.Clear();
                var result = App.Sincronizador.Filtar(Filtro.ToLower());              
                foreach (var item in result.ToList())
                {
                    ListaVentas.Add(item);
                }             
            }
        }

        private void DragStarts(object obj)
        {
            Pastel = obj as Pasteles;
        }

        private void Sincronizador_Sincronizado()
        {
            Actualizar();
        }

        private async void Cancelar()
        {
            await App.Current.MainPage.Navigation.PopAsync();
        }

        private async void EliminarVenta(object obj)
        {
            var pastel = obj as Pasteles;
            if (pastel == null)
            {
                await App.Current.MainPage.DisplayAlert("Mensaje", "Seleccione una venta para eliminarla", "Aceptar");
            }
            else
            {
                var respuesta = await App.Current.MainPage.DisplayAlert("Confirmar", $"¿Desea eliminar la venta del pastel {pastel.NombrePastel}?", "Si", "No");
                if (respuesta == true)
                {
                    var result = await App.Sincronizador.Eliminar(pastel);
                    if (result != null)
                    {
                        Errores = string.Join("\n", result);
                    }
                }
            }
        }

        private async void EditarVenta()
        {
            var result = await App.Sincronizador.Editar(Pastel);
            if (result == null)
            {
                await App.Current.MainPage.Navigation.PopAsync();
            }
            else
            {
                Errores = string.Join("\n", result);
            }
        }

        private async void AgregarVenta()
        {
            var result = await App.Sincronizador.Agregar(Pastel);
            if (result == null)
            {
                await App.Current.MainPage.Navigation.PopAsync();
            }
            else
            {
                Errores = string.Join("\n", result);
            }
        }

        private async void VerEditar(object obj)
        {
            Errores = "";
            var pastel = obj as Pasteles;
            if (pastel == null)
            {
                await App.Current.MainPage.DisplayAlert("Mensaje", "Seleccione una venta para editarla", "Aceptar");
            }
            else
            {
                if (editarVentaPastelView == null)
                {
                    editarVentaPastelView = new EditarVentaPastelView() { BindingContext = this };
                }
                Pastel = pastel;
                await App.Current.MainPage.Navigation.PushAsync(editarVentaPastelView);
            }
        }

        private async void VerAgregar()
        {
            Errores = "";
            if (agregarVentaPastelView == null)
            {

                agregarVentaPastelView = new AgregarVentaPastelView() { BindingContext = this };
            }
            Pastel = new Pasteles();
            await App.Current.MainPage.Navigation.PushAsync(agregarVentaPastelView);
        }
        public void Actualizar()
        {
            ListaVentas.Clear();
            var pasteles = App.Catalogo.GetAll().ToList();
            foreach (var item in App.Sincronizador.buffer)
            {
                switch (item.Estado)
                {
                    case Estado.Agregado:
                        pasteles.Add(item.Pastel);
                        break;
                    case Estado.Modificado:
                        var p = pasteles.FirstOrDefault(x => x.Id == item.Pastel.Id);
                        if (p != null)
                        {
                            p.NombrePastel = item.Pastel.NombrePastel;
                            p.Costo = item.Pastel.Costo;
                            p.FechaVenta = item.Pastel.FechaVenta;
                            p.Sucursal = item.Pastel.Sucursal;
                        }
                        break;
                    case Estado.Eliminado:
                        p = pasteles.FirstOrDefault(x => x.Id == item.Pastel.Id);
                        if (p != null)
                        {
                            pasteles.Remove(p);
                        }
                        break;
                }
            }
            foreach (var pastel in pasteles)
            {
                ListaVentas.Add(pastel);
            }
        }
    }
}
