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
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }
            else
            {
                var partido = repository.GetAll().Where(x => x.Eliminado == 0 && x.Id == id).Select(
                    x=> new
                    {
                        x.Id,
                        x.Equipos,
                        x.DescripcionPartido,
                        x.Goles,
                        x.Minuto,
                        x.EstadoPartido,
                        x.FechaPartido
                    }      
                    );
                if (partido != null)
                {
                    return Ok(partido);
                }
                else
                {
                    return BadRequest();
                }
               
            }
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
                        {"Id", par.Id.ToString() },
                        {"Goles", par.Goles},
                        {"Equipos", par.Equipos },
                        {"Minuto", par.Minuto },
                        {"Descripcion", par.DescripcionPartido},
                        {"Estado", par.EstadoPartido },
                        {"Fecha", par.FechaPartido.ToString()},
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
