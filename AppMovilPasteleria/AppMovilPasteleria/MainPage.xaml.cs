using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AppMovilPasteleria
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            
        }

        private void DragGestureRecognizer_DragStarting(object sender, DragStartingEventArgs e)
        {
                btn1.IsVisible = true;
                btn2.IsVisible = true;
                btn3.IsVisible = false;
        }

        private void DragGestureRecognizer_DropCompleted(object sender, DropCompletedEventArgs e)
        {
            btn1.IsVisible = false;
            btn2.IsVisible = false;
            btn3.IsVisible = true;
        }
    }
}
