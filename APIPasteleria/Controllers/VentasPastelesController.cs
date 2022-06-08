using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APIPasteleria.Repositories;
using APIPasteleria.Models;
namespace APIPasteleria.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VentasPastelesController : ControllerBase
    {
        VentasPastelesRepository repository;
        public Itesrcne_181g0231Context Context { get; set; }
        public VentasPastelesController(Itesrcne_181g0231Context context)
        {
            Context = context;
            repository = new(Context);
        }
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(repository.GetAll().OrderBy(x => x.FechaVenta)
                .Select(x => new
                {
                    x.Id,
                    x.NombrePastel,
                    x.Costo,
                    x.FechaVenta,
                    x.Sucursal
                })
            );
        }
       
        [HttpPost]
        public IActionResult Post([FromBody] Pasteles pastel)
        {
            if(repository.IsValid(pastel, out List<string> errores))
            {
                pastel.Id = 0;
                repository.Insert(pastel);
                return Ok();
            }
            else
            {
                return BadRequest(errores);
            }
        }
        [HttpPost("sincronizar")]
        public IActionResult Post([FromBody]DateTime fecha)
        {
            var datos = repository.GetSinceDate(fecha);
            var noEliminados = datos.Where(x => x.Eliminado == 0)
                .Select(x => new
                {
                    x.Id,
                    x.NombrePastel,
                    x.Costo,
                    x.FechaVenta,
                    x.Sucursal
                });
            var eliminados = datos.Where(x => x.Eliminado == 1)
                .Select(x => new
                {
                    x.Id
                });
            List<object> resultado = new List<object>();
            resultado.AddRange(noEliminados);
            resultado.AddRange(eliminados);
            return Ok(resultado);
        }
        [HttpPut]
        public IActionResult Put([FromBody] Pasteles p)
        {
            if (repository.IsValid(p, out List<string> errores))
            {
                var pastel = repository.GetById(p.Id);
                if (pastel == null || pastel.Eliminado == 1)
                {
                    return NotFound();
                }
                else
                {
                    pastel.NombrePastel = p.NombrePastel;
                    pastel.Costo = p.Costo;
                    pastel.FechaVenta = p.FechaVenta;
                    pastel.Sucursal = p.Sucursal;
                    repository.Update(pastel);
                    return Ok();
                }
            }
            else
            {
                return BadRequest(errores);
            }
        }
        [HttpDelete]
        public IActionResult Delete([FromBody]Pasteles p)
        {
            var pastel = repository.GetById(p.Id);
            if(pastel==null || pastel.Id == 1)
            {
                return NotFound();
            }
            else
            {
                repository.Delete(pastel);
                return Ok();
            }
        }
    }
}
