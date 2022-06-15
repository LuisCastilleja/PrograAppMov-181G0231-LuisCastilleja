using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

[assembly: Xamarin.Forms.Dependency(typeof(AppClientePartidoEnVivo.Droid.ClaseToast))]

namespace AppClientePartidoEnVivo.Droid
{
    public class ClaseToast : IToast
    {
        public void MostrarToast(string mensaje)
        {
            var toast = Toast.MakeText(Application.Context, mensaje, ToastLength.Long);
            toast.Show();
        }
    }
}