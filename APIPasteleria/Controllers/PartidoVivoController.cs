using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using APIPasteleria.Models;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using APIPasteleria.Repositories;
using System.Linq;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using FirebaseAdmin.Messaging;
using System.Threading.Tasks;

namespace APIPasteleria.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PartidoVivoController : ControllerBase
    {
        public Itesrcne_181g0231Context Context { get; set; }
        PartidoEnVivoRepository  repository;
        public PartidoVivoController(Itesrcne_181g0231Context context)
        {
            Context = context;
            repository = new PartidoEnVivoRepository(Context);

            if(FirebaseApp.DefaultInstance == null)
            {
                //Abrir la configuración
                FirebaseApp.Create(new AppOptions
                {
                    //Obtener las credenciales del json
                    //Que descargamos de Firebase
                    Credential = GoogleCredential.FromFile("partidos.json")
                });                                   
            }
        }
        [HttpGet]
        public IActionResult Get()
        {
            var partidos = repository.GetAll().OrderByDescending(x => x.FechaPartido)
                .Where(x=>x.Eliminado==0).Select(
                x => new
                {
                    x.Id,
                    x.Equipos,
                    x.DescripcionPartido,
                    x.Goles,
                    x.Minuto,
                    x.EstadoPartido,
                    x.FechaPartido
                });
            return Ok(partidos);
        }
        [HttpPost]
        public async Task<IActionResult> Post(Partido par)
        {
            if(repository.IsValid(par, out List<string> errors))
            {
                par.Eliminado = 0;
                repository.Insert(par);

                //Mensaje de Firebase
                //Mensajeria instantanea a un servicio
                //InApp Messagging
                Message m = new Message()
                {
                    Topic = "partidos",
                    //Datos que quiero que lleve
                    Data = new Dictionary<string, string>()
                    {
                    //Con solo el Id podemos descargar los datos
                        {"Id", par.Id.ToString() },
                        {"Accion","Nuevo"}
                    }
                };
                //Si no lo hacemos awit no llega el mensaje
                await FirebaseMessaging.DefaultInstance.SendAsync(m);
                return Ok();
            }
            else
            {
                return BadRequest(errors);
            }
        }
    }
}
