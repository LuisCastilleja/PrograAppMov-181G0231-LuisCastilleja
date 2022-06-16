using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using APIPasteleria.Repositories;
using APIPasteleria.Models;
namespace APIPasteleria.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ComprasController : ControllerBase
    {
        public Itesrcne_181g0231Context Context { get; set; }
        ComprasRepository repository;

        public ComprasController(Itesrcne_181g0231Context context)
        {
            Context = context;
            repository = new ComprasRepository(Context);
        }

        [HttpGet]
        public IActionResult Get()
        {
            var compras = repository.GetAll();
           return Ok(compras);
        }
    }
}
