using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace NotificacionesPartidoEnVivo.Helpers
{
    public class HttpClientHelper<T> where T : class
    {
        public HttpClient Client { get; private set;}
        public Uri Uri { get; set; }
        public HttpClientHelper(Uri uri)
        {
            Client = new HttpClient();
            Uri = uri;
        }

        //Peticion Get por id
        public async Task<T> Get(int id)
        {
            var response = await Client.GetAsync(Uri + $"/{id}");
            //Captura excepciones y detiene el metodo
            //En caso de que no sea un 200
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            T datos = JsonConvert.DeserializeObject<T>(json)!;
            return datos!;
        }       
        //Peticion Get
        //Siempre que regresaremos algo se usa el Task
        public async Task<IEnumerable<T>> Get()
        {
            var response = await Client.GetAsync(Uri);

            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            List<T> datos = JsonConvert.DeserializeObject<List<T>>(json)!;
            return datos;
        }

        //Peticion Post que enviamos un objeto de una clase
        public async Task<Object> Post(T model)
        {
            var json = JsonConvert.SerializeObject(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await Client.PostAsync(Uri, content);

            response.EnsureSuccessStatusCode();

            if(response.Content.Headers.ContentLength == 0)
            {
                return null!;
            }
            else
            {
                var result = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject(result)!;
            }
        }
        //Metodo Post que recibe una Uri y un objeto
        public async Task<Object> Post(string uri, Object model)
        {
            var json = JsonConvert.SerializeObject(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await Client.PostAsync(Uri + uri, content);

            response.EnsureSuccessStatusCode();

            if (response.Content.Headers.ContentLength == 0)
                return null!;
            else
            {
                var result = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject(result)!;
            }
        }
        //Metodo Put 
        public async Task Put(T model)
        {
            var json = JsonConvert.SerializeObject(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await Client.PutAsync(Uri, content);

            response.EnsureSuccessStatusCode();
        }

        //Metodo Delete por Id
        public async Task Delete(int id)
        {
            var reponse = await Client.DeleteAsync(Uri + $"/{id}");
            reponse.EnsureSuccessStatusCode();
        }

        //Metodo Delete por objeto
        public async Task Delete(T model)
        {
            var json = JsonConvert.SerializeObject(model);
            HttpRequestMessage rm = new HttpRequestMessage()
            {
                Method = HttpMethod.Delete,
                RequestUri = Uri,
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };

            var reponse = await Client.SendAsync(rm);

            reponse.EnsureSuccessStatusCode();
        }
    }
}
