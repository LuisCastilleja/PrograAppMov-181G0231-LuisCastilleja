using AppMovilComprasJwt.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace AppMovilComprasJwt.ViewModels
{
    public class CompraViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<Compra> ListaCompras { get; set; } = new ObservableCollection<Compra>();

        public CompraViewModel()
        {

                DescargarDatos();
        }

        private async void DescargarDatos()
        {           
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                var fecha = DateTime.Now;
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri("https://181g0231.82g.itesrc.net/");
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {await App.User.GetToken()}");
                var result = await client.GetAsync("api/compras");
                if (result.IsSuccessStatusCode)
                {
                    var json = await result.Content.ReadAsStringAsync();
                    var lista = JsonConvert.DeserializeObject<List<Compra>>(json);
                    foreach (var compra in lista)
                    {
                        ListaCompras.Add(compra);
                    }
                }
                else
                {
                    App.User.CerrarSesion();
                }
            }
            else
            {
                IToast toast = DependencyService.Get<IToast>();
                toast.MostrarToast("No tiene conexión a internet. Verifique su conexión para poder hacer login");
            }
        }
    }
}
