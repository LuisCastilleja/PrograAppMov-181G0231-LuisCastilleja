﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using AppClientePartidoEnVivo.Views;
namespace AppClientePartidoEnVivo.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PartidosEnVivoView : ContentPage
    {
        public PartidosEnVivoView()
        {
            InitializeComponent();
        }
        
        private  void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {

        }
    }
}