using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using AppMovilPasteleria.Models;
using Xamarin.Essentials;
using System.Net;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace AppMovilPasteleria.Services
{
    public class SincronizadorService
    {
        HttpClient cliente = new HttpClient()
        {
            BaseAddress = new Uri("https://181G0231.82g.itesrc.net/")
        };
        CatalogoVentasPasteles bdLocal;
        public List<PastelesEstado> buffer { get; set; } = new List<PastelesEstado>();

        public event Action Sincronizado;
        public SincronizadorService(CatalogoVentasPasteles catalogo)
        {
            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
            bdLocal = catalogo;
            bool key = Preferences.ContainsKey("FechaActualizada");
            if (!key)
            {
                DescargarPrimeraVez();
            }
            var hilo = new Thread(new ThreadStart(Sincronizar));
            hilo.Start();
        }

        private async void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            await SincronizarApi();
        }

        public  IEnumerable<Pasteles> Filtar(string filtro)
        {
            CatalogoVentasPasteles cv = new CatalogoVentasPasteles();
            return cv.GetAllByFiltro(filtro);
        }

        public async Task<List<string>> Agregar(Pasteles p)
        {
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                return await EnviarDatosApi(p, HttpMethod.Post);
            }
            else
            {
                PastelesEstado pastelesEstado = new PastelesEstado();
                pastelesEstado.Pastel = p;
                pastelesEstado.Estado = Estado.Agregado;
                buffer.Add(pastelesEstado);
                Sincronizado?.Invoke();
            }
            return null;

        }
        public async Task<List<string>> Editar(Pasteles p)
        {
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                return await EnviarDatosApi(p, HttpMethod.Put);
            }
            else
            {
                PastelesEstado pastelEstado = new PastelesEstado();
                pastelEstado.Pastel = p;
                pastelEstado.Estado = Estado.Modificado;
                buffer.Add(pastelEstado);
                Sincronizado?.Invoke();
            }
            return null;
        }
        public async Task<List<string>> Eliminar(Pasteles p)
        {
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                return await EnviarDatosApi(p, HttpMethod.Delete);
            }
            else
            {
                PastelesEstado pastelesEstado = new PastelesEstado();
                pastelesEstado.Pastel = p;
                pastelesEstado.Estado = Estado.Eliminado;
                buffer.Add(pastelesEstado);
                Sincronizado?.Invoke();
            }
            return null;
        }
        private async Task<List<string>> EnviarDatosApi(Pasteles p, HttpMethod method)
        {
            string json = JsonConvert.SerializeObject(p);
            var hrm = new HttpRequestMessage(method, cliente.BaseAddress + "api/ventasPasteles");
            hrm.Content = new StringContent(json, Encoding.UTF8, "application/json");
            var result = await cliente.SendAsync(hrm);
            if (result.StatusCode == HttpStatusCode.OK)
            {
                //Para que sincronice despues de realizar la accion
                await SincronizarApi();
                return null;
            }
            else if (result.StatusCode == HttpStatusCode.BadRequest)
            {
                json = await result.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<string>>(json);
            }
            else if (result.StatusCode == HttpStatusCode.NotFound)
            {
                return new List<string>() { "La venta de pastel especificada no ha sido encontrada" };
            }
            else
            {
                //Devolver el error que nos da el statusCode ya que no lo capturamos nosotros
                return new List<string>() { result.StatusCode.ToString() };
            }
        }
        private async void Sincronizar()
        {
            //Para no iniciar otro hilo extraemos el metodo
            while (true)
            {
                var contenido = bdLocal.GetAll();
                await SincronizarApi();
                Thread.Sleep(TimeSpan.FromMinutes(1));
            }
        }
        private async Task SincronizarApi()
        {

            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                if (buffer.Count > 0)
                {
                    foreach (var item in buffer.ToArray())
                    {
                        buffer.Remove(item);
                        switch (item.Estado)
                        {
                            case Estado.Agregado:
                                await Agregar(item.Pastel);
                                break;
                            case Estado.Modificado:
                                await Editar(item.Pastel);
                                break;
                            case Estado.Eliminado:
                                await Eliminar(item.Pastel);
                                break;
                        }
                    }
                }
                var fecha = Preferences.Get("FechaActualizada", DateTime.MinValue);
                string json = JsonConvert.SerializeObject(fecha);
                fecha = DateTime.Now;
                var result = await cliente.PostAsync("api/ventasPasteles/sincronizar", new StringContent(json, Encoding.UTF8, "application/json"));
                if (result.StatusCode == HttpStatusCode.OK)
                {
                    json = await result.Content.ReadAsStringAsync();
                    var ventas = JsonConvert.DeserializeObject<Pasteles[]>(json);
                    foreach (var venta in ventas)
                    {
                        bdLocal.InsertOrReplace(venta);
                    }
                    Preferences.Set("FechaActualizada", fecha);
                    if (ventas.Length > 0)
                    {
                        Sincronizado?.Invoke();
                    }
                }
            }
        }

        private async void DescargarPrimeraVez()
        {
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                var fecha = DateTime.Now;
                var result = await cliente.GetAsync("api/ventasPasteles");
                if (result.StatusCode == HttpStatusCode.OK)
                {
                    var json = await result.Content.ReadAsStringAsync();
                    List<Pasteles> ventas = JsonConvert.DeserializeObject<List<Pasteles>>(json);
                    foreach (var venta in ventas)
                    {
                        bdLocal.InsertOrReplace(venta);
                    }
                    Preferences.Set("FechaActualizada", fecha);
                    if (ventas.Count > 0)
                    {
                        Sincronizado?.Invoke();
                    }
                }
            }
        }
    }
}
