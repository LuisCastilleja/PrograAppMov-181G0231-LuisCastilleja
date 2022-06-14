using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Firebase.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AppClientePartidoEnVivo.Repositories;
using AppClientePartidoEnVivo.Models;
using AndroidX.Core.App;
using AppClientePartidoEnVivo.Helpers;
using System.Globalization;

[assembly:Xamarin.Forms.Dependency(typeof(AppClientePartidoEnVivo.Droid.ServicioActualizaciones))]
namespace AppClientePartidoEnVivo.Droid
{
    [Service(Exported = false)]
    [IntentFilter(new[] { "com.google.firebase.MESSAGING_EVENT" })]
    public class ServicioActualizaciones : FirebaseMessagingService
    {
        public override void OnMessageReceived(RemoteMessage p0)
        {

            try
            {
                PartidosRepository repository = new PartidosRepository();

                Partido partido = null;
                if (p0.Data != null)
                {
                    var datos = p0.Data;
                    //Si la accion que me regresa es agregar
                    if (datos["Accion"] == "Nuevo")
                    {

                        //Instanciamos un nuevo objeto de la clase partido
                        //y le pasamos los datos que tiene la variable par
                        partido = new Partido()
                        {
                            DescripcionPartido = datos["Descripcion"],
                            Goles = datos["Goles"],
                            Equipos = datos["Equipos"],
                            FechaPartido = DateTime.ParseExact(datos["Fecha"],
                           "dd/MM/yyyy hh:mm:ss tt",
                           new CultureInfo("es-MX")),
                            EstadoPartido = datos["Equipos"],
                            Minuto = datos["Minuto"],
                        };
                        repository.Insert(partido);
                    }
                    //Si la accion es editar
                    else if (datos["Accion"] == "Editar")
                    {
                        var id = int.Parse(datos["Id"]);
                        var partidoEditar = repository.Get(id);
                        if (partidoEditar != null)
                        {
                            partidoEditar.DescripcionPartido = datos["Descripcion"];
                            partidoEditar.Goles = datos["Goles"];
                            partidoEditar.Equipos = datos["Equipos"];
                            partidoEditar.FechaPartido = DateTime.ParseExact(datos["Fecha"],
                       "dd/MM/yyyy hh:mm:ss tt",
                       new CultureInfo("es-MX"));
                            partidoEditar.EstadoPartido = datos["Equipos"];
                            partidoEditar.Minuto = datos["Minuto"];

                            repository.Update(partidoEditar);
                        }
                    }
                    //Si la acción es eliminar
                    else if (datos["Accion"] == "Eliminar")
                    {
                        var id = int.Parse(datos["Id"]);
                        var partidoEliminar = repository.Get(id);

                        if(partidoEliminar != null)
                        {
                            repository.Delete(partidoEliminar);
                        }
                    }
                    if (App.Current == null)
                    {
                        if (partido != null)
                        {
                            ShowNotification(partido.Id, partido.Equipos, partido.DescripcionPartido);
                        }
                    }
                    else
                    {
                        App.Actualizar();
                    }
                }
            }
            catch (Exception ex)
            {
                var m = ex.Message;
            }
            base.OnMessageReceived(p0);
        }


        //Mostrar la notificaion
        public void ShowNotification(int id, string title, string text)
        {
            NotificationCompat.Builder builder = new NotificationCompat.Builder(Application.Context,
                "CANALPARTIDO")
                //El titulo de la notificacion
                .SetContentTitle(title)
                //EL contenido
                .SetContentText(text)
                //La prioridad en Max para que se muestre aunque este bloqueado.
                .SetPriority(NotificationCompat.PriorityMax)
                .SetShowWhen(true)
                //El icono que mostrara
                .SetSmallIcon(Resource.Drawable.balon2);

            NotificationManager manager = Application.Context.GetSystemService(Application.NotificationService)
                as NotificationManager;

            manager.Notify(id, builder.Build());
        }
    }
}