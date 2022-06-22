using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using AppMovilAnuncios.ViewModels;
using System.Reflection;
using System.IO;
using Plugin.SimpleAudioPlayer;
using AppMovilAnuncios.Models;
using System.Threading.Tasks;

namespace AppMovilAnuncios.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SimonDiceView : ContentPage
    {
        SimonViewModel simon = new SimonViewModel();
        ISimpleAudioPlayer player;
        ISimpleAudioPlayer player2;
        ISimpleAudioPlayer player3;
        ISimpleAudioPlayer player4;
        ISimpleAudioPlayer player5;
        ISimpleAudioPlayer player6;

        public SimonDiceView()
        {
            InitializeComponent();
            simon.BotonEncendido += Simon_BotonEncendido;
            simon.Perdio += Simon_Perdio;
            simon.SumoPuntos += Simon_SumoPuntos;
            simon.GanoPuntos += Simon_GanoPuntos;
            var assembly = typeof(App).GetTypeInfo().Assembly;
            var stream = assembly.GetManifestResourceStream("AppMovilAnuncios.Assets.inicio.mp3");
            var stream2 = assembly.GetManifestResourceStream("AppMovilAnuncios.Assets.botonAma.mp3");
            var stream3 = assembly.GetManifestResourceStream("AppMovilAnuncios.Assets.BotoAzul.mp3");
            var stream4 = assembly.GetManifestResourceStream("AppMovilAnuncios.Assets.botonRojo.mp3");
            var stream5 = assembly.GetManifestResourceStream("AppMovilAnuncios.Assets.botonVerde.mp3");
            var stream6 = assembly.GetManifestResourceStream("AppMovilAnuncios.Assets.perdedor.mp3");
            player = CrossSimpleAudioPlayer.CreateSimpleAudioPlayer();
            player2 = CrossSimpleAudioPlayer.CreateSimpleAudioPlayer();
            player3 = CrossSimpleAudioPlayer.CreateSimpleAudioPlayer();
            player4 = CrossSimpleAudioPlayer.CreateSimpleAudioPlayer();
            player5 = CrossSimpleAudioPlayer.CreateSimpleAudioPlayer();
            player6 = CrossSimpleAudioPlayer.CreateSimpleAudioPlayer();
            player.Load(stream);
            player2.Load(stream2);
            player3.Load(stream3);
            player4.Load(stream4);
            player5.Load(stream5);
            player6.Load(stream6);
            InicioSonidos();
        }

        private void Simon_GanoPuntos()
        {
            simon.Puntuacion += 30;
            lblPuntuacion.Text = simon.Puntuacion.ToString("Puntuación del jugador: 0");
            btnGanarPuntos.IsEnabled = false;
            btnGanarPuntos.BorderColor = Color.Black;
        }

        private void Simon_SumoPuntos()
        {
            simon.Puntuacion++;
            lblPuntuacion.Text = simon.Puntuacion.ToString("Puntuación del jugador: 0");
        }

        private void InicioSonidos()
        {
            btnIniciar.Clicked += BtnIniciar_Clicked;
            Verde.Clicked += Verde_Clicked;
            Rojo.Clicked += Rojo_Clicked;
            Azul.Clicked += Azul_Clicked;
            Amarillo.Clicked += Amarillo_Clicked1;
        }

        private void Amarillo_Clicked1(object sender, EventArgs e)
        {
            player2.Play();
        }

        private void Azul_Clicked(object sender, EventArgs e)
        {
            player3.Play();
        }

        private void Rojo_Clicked(object sender, EventArgs e)
        {
            player4.Play();
        }

        private void Verde_Clicked(object sender, EventArgs e)
        {
            player5.Play();
        }

        private  void BtnIniciar_Clicked(object sender, EventArgs e)
        {
           
        }


        private void Simon_Perdio()
        {
            DisplayAlert("Simon dice: haz perdido.", "Suerte para la proxima. Eres un gran jugador.", "Okey");
            btnIniciar.IsVisible = true;
            Verde.IsEnabled = false;
            Rojo.IsEnabled = false;
            Amarillo.IsEnabled = false;
            Azul.IsEnabled = false;
            player6.Play();
            simon.Puntuacion = 0;
            lblPuntuacion.Text = simon.Puntuacion.ToString("Puntuación del jugador: 0");
            btnPuntos.IsEnabled = true;
            btnPuntos.BorderColor = Color.White;
            btnGanarPuntos.IsEnabled = true;
            btnGanarPuntos.BorderColor = Color.White;
        }

        private async void Simon_BotonEncendido(SimonDice cuadrante)
        {
            Verde.IsEnabled = false;
            Rojo.IsEnabled = false;
            Amarillo.IsEnabled = false;
            Azul.IsEnabled = false;
            switch (cuadrante.Cuadrantes)
            {

                case Cuadrantes.Verde:
                    Verde.BackgroundColor = Color.LightGreen;
                    player5.Play();
                    await Task.Delay(800);
                    Verde.BackgroundColor = Color.DarkGreen;
                    await Task.Delay(2500);
                    break;
                case Cuadrantes.Rojo:
                    Rojo.BackgroundColor = Color.Red;
                    player4.Play();
                    await Task.Delay(800);
                    Rojo.BackgroundColor = Color.DarkRed;
                    await Task.Delay(2000);
                    break;
                case Cuadrantes.Azul:
                    player3.Play();
                    Azul.BackgroundColor = Color.LightBlue;
                    await Task.Delay(300);
                    Azul.BackgroundColor = Color.DarkBlue;
                    await Task.Delay(2000);
                    break;
                case Cuadrantes.Amarillo:
                    Amarillo.BackgroundColor = Color.LightYellow;
                    player2.Play();
                    await Task.Delay(300);
                    Amarillo.BackgroundColor = Color.Yellow;
                    await Task.Delay(2000);
                    break;
            }

            Verde.IsEnabled = true;
            Rojo.IsEnabled = true;
            Amarillo.IsEnabled = true;
            Azul.IsEnabled = true;
        }

        private void Amarillo_Clicked(object sender, EventArgs e)
        {

        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            if (btnIniciar.IsVisible == false)
            {
                Button btn = (Button)sender;
                SimonDice n = new SimonDice();
                n.Cuadrantes= (Cuadrantes)btn.BindingContext;
                await Task.Delay(500);
                simon.Verificar(n);
            }
        }

        private async void btnIniciar_Clicked_1(object sender, EventArgs e)
        {
            btnIniciar.IsVisible = false;
            btnPuntos.IsEnabled = false;
            btnPuntos.BorderColor = Color.Black;
            btnGanarPuntos.IsEnabled = false;
            btnGanarPuntos.BorderColor = Color.Black;
            player.Play();
            await Task.Delay(6400);
            simon.IniciarJuego();
            Verde.IsEnabled = true;
            Rojo.IsEnabled = true;
            Amarillo.IsEnabled = true;
            Azul.IsEnabled = true;
        }

        private  void btnVerPuntuacion_Clicked(object sender, EventArgs e)
        {
         
        }
    }
}