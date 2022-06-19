using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using AppMovilAnuncios.Models;
using AppMovilAnuncios.Repositories;
using AppMovilAnuncios.Views;
using Xamarin.Forms;

namespace AppMovilAnuncios.ViewModels
{
    public class SimonViewModel:INotifyPropertyChanged
    {
        List<SimonDice> secuencia = new List<SimonDice>();
        public List<SimonPuntuacion> ListaPuntuaciones { get; set; } = new List<SimonPuntuacion>();
        SimonDice simon;
        SimonRepository repository;
        VerPuntuacionView verPuntuacionView;
        Random random = new Random();
        int posicion = 0;
        private int puntuacion = 0;

        public int Longitud { get { return secuencia.Count; } }
        public int Puntuacion
        {
            get { return puntuacion; }
            set { puntuacion = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Puntuacion")); }
        }

        public event Action<SimonDice> BotonEncendido;
        public event Action Perdio;
        public event Action SumoPuntos;
        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand VerPuntuacionCommand { get; set; }
        public SimonViewModel()
        {
            VerPuntuacionCommand = new Command(VerPuntuaciones);
            ListaPuntuaciones.Clear();
            repository = new SimonRepository();
            var puntuaciones = repository.GetAll();
            if (puntuaciones != null)
            {
                foreach (var puntuacion in puntuaciones)
                {
                    ListaPuntuaciones.Add(puntuacion);
                }
            }
        }
        public void IniciarJuego()
        {
            secuencia.Clear();
            Agregar();
        }

        private async void VerPuntuaciones()
        {
            ListaPuntuaciones.Clear();
            repository = new SimonRepository();
            var puntuaciones = repository.GetAll();
            if (puntuaciones != null)
            {
                foreach (var puntuacion in puntuaciones)
                {
                    ListaPuntuaciones.Add(puntuacion);
                }
            }
            if (verPuntuacionView == null)
            {
                verPuntuacionView = new VerPuntuacionView() { BindingContext = this };
                await App.Current.MainPage.Navigation.PushAsync(verPuntuacionView);

            }
            else
            {        
                await App.Current.MainPage.Navigation.PushAsync(verPuntuacionView);
            }

        }

        public async void Agregar()
        {
            foreach (var anterior in secuencia)
            {
                BotonEncendido?.Invoke(anterior);
                await Task.Delay(1000);
            }
            simon = new SimonDice();
            simon.Cuadrantes = (Cuadrantes)random.Next(0, 4);
            secuencia.Add(simon);
            BotonEncendido?.Invoke(simon);
            await Task.Delay(1000);
            posicion = 0;
        }
        public async void Verificar(SimonDice cuadrante)
        {
            if (Longitud > 0)
            {
                if (secuencia[posicion].Cuadrantes == cuadrante.Cuadrantes)
                {
                    posicion++;
                    if (posicion >= secuencia.Count)
                    {
                        SumoPuntos?.Invoke();
                        await Task.Delay(1000);
                        Agregar();
                    }
                }
                else
                {
                    SimonPuntuacion simonPuntos = new SimonPuntuacion();
                    simonPuntos.Puntuacion = Puntuacion;
                    simonPuntos.Fecha = DateTime.Now;
                    repository = new SimonRepository();
                    repository.Insert(simonPuntos);
                    ListaPuntuaciones.Clear();
                    Perdio?.Invoke();
                }
            }
        }
    }
}
