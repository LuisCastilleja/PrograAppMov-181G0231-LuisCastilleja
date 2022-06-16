using AppMovilComprasJwt.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Http;
using System.Text;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace AppMovilComprasJwt.ViewModels
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        public Usuarios Usuarios { get; set; } = new Usuarios();
        public bool Indicator { get; set; }

        private string error;

        public string Error
        {
            get { return error; }
            set { error = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Error")); }
        }
        
        public ICommand IniciarSesionCommand { get; set; }

        public LoginViewModel()
        {
            IniciarSesionCommand = new Command(IniciarSesion);
        }

        private async void IniciarSesion()
        {
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                Error = "";
                if (string.IsNullOrWhiteSpace(Usuarios.Usuario))
                {
                    Error = "Proporcione el usuario para iniciar sesión";
                }
                if (string.IsNullOrWhiteSpace(Usuarios.Password))
                {
                    Error = "Proporcione la contraseña para inciar sesión";
                }
                else
                {
                    Error = "";
                    Indicator = true;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null));
                    HttpClient client = new HttpClient();
                    client.BaseAddress = new Uri("https://181g0231.82g.itesrc.net/");
                    var json = JsonConvert.SerializeObject(Usuarios);
                    HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");
                    var result = await client.PostAsync("api/login", content);
                    if (result.IsSuccessStatusCode)
                    {
                        var token = await result.Content.ReadAsStringAsync();
                        App.User.IniciarSesion(token);
                        App.User.Redirigir();
                    }
                    else
                    {
                        Error = await result.Content.ReadAsStringAsync();
                    }
                    Indicator = false;
                }
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null));
            }
            else
            {
                IToast toast = DependencyService.Get<IToast>();
                toast.MostrarToast("No tiene conexión a internet. Verifique su conexión para poder hacer login");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

    }
}
