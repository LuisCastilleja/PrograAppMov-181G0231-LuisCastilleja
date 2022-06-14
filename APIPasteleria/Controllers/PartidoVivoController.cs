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
        PartidoEnVivoRepository repository;
        public PartidoVivoController(Itesrcne_181g0231Context context)
        {
            Context = context;
            repository = new PartidoEnVivoRepository(Context);

            if (FirebaseApp.DefaultInstance == null)
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
                .Where(x => x.Eliminado == 0).Select(
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
        [HttpPut]
        public async Task<IActionResult> Put(Partido par)
        {
            if (par != null)
            {
                var partido = repository.GetById(par.Id);
                if (partido != null)
                {
                    if (repository.IsValid(par, out List<string> errors))
                    {
                        partido.Eliminado = par.Eliminado;
                        partido.Equipos = par.Equipos;
                        partido.Minuto = par.Minuto;
                        partido.EstadoPartido = par.EstadoPartido;
                        partido.DescripcionPartido = par.DescripcionPartido;
                        partido.Goles = par.Goles;
                        partido.FechaPartido = par.FechaPartido;

                        repository.Update(partido);

                        //Mandar notificacion de edicion
                        Message m = new Message()
                        {
                            Topic = "partidos",
                            //Datos que quiero que lleve
                            Data = new Dictionary<string, string>()
                        {
                            {"Id", partido.Id.ToString() },
                            {"Goles", partido.Goles},
                            {"Equipos", partido.Equipos },
                            {"Minuto", partido.Minuto },
                            {"Descripcion", partido.DescripcionPartido},
                            {"Estado", partido.EstadoPartido },
                            {"Fecha", partido.FechaPartido.ToString()},
                            {"Accion","Editar"}
                        }
                        };
                        await FirebaseMessaging.DefaultInstance.SendAsync(m);
                        return Ok();
                    }
                    else
                    {
                        return BadRequest(errors);
                    }
                }
                else
                {
                    return NotFound();
                }
            }
            else
            {
                return BadRequest("Proporcione la actualización que sera editada");
            }
        }
        [HttpPost]
        public async Task<IActionResult> Post(Partido par)
        {
            if (repository.IsValid(par, out List<string> errors))
            {
                par.Eliminado = 0;
                repository.Insert(par);

                //Mensaje de Firebase
                //Mensajeria instantanea a un servicio
                //InApp Messagging
                //Mandar notificacion de agregar
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


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if(id == 0)
            {
                return BadRequest();
            }
            else
            {
                var partido = repository.GetById(id);
                if (partido != null)
                {
                    partido.Eliminado = 1;
                    repository.Update(partido);


                    //Mandar notificacion de eliminado.
                    Message m = new Message()
                    {
                        Topic = "partidos",
                        //Datos que quiero que lleve
                        Data = new Dictionary<string, string>()
                    {
                        {"Id", partido.Id.ToString() },                     
                        {"Accion","Eliminar"}
                    }
                    };
                    await FirebaseMessaging.DefaultInstance.SendAsync(m);

                    return Ok();
                }
                else
                {
                    return NotFound();
                }
            }
        }
    }
}
