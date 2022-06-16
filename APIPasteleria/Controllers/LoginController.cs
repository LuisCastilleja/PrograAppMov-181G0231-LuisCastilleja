using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using APIPasteleria.Models;
using APIPasteleria.Repositories;
using System.Linq;
using System.Security.Claims;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System;
using System.Text;

namespace APIPasteleria.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        public Itesrcne_181g0231Context Context { get; set; }
        public IConfiguration Configuration { get; }
        UsuariosRepository repository;
        public LoginController(IConfiguration configuration, Itesrcne_181g0231Context context)
        {
            Configuration = configuration;
            Context = context;
            repository = new UsuariosRepository(Context);
        }
        [HttpPost]
        public IActionResult Post(Usuarios usuario)
        {
            if (repository.IsValid(usuario, out List<string> errors))
            {
                var usuarioExistente = repository.GetAll().FirstOrDefault(x => x.Usuario == usuario.Usuario && x.Password == usuario.Password);
                if (usuarioExistente != null)
                {
                    //Crear la identidad
                    List<Claim> claims = new List<Claim>();
                    claims.Add(new Claim(ClaimTypes.Name, usuarioExistente.Usuario));
                    claims.Add(new Claim(ClaimTypes.NameIdentifier, $"{usuarioExistente.Id}"));
                    //Crear un token.
                    var handler = new JwtSecurityTokenHandler();
                    var descriptor = new SecurityTokenDescriptor();

                    descriptor.Issuer = Configuration["Jwt:Issuer"];
                    descriptor.Audience = Configuration["Jwt:Audience"];
                    //La identidad a quien se emite el token.
                    descriptor.Subject = new ClaimsIdentity(claims, JwtBearerDefaults.AuthenticationScheme);
                    //En cuanto tiempo se expira el token que mandamos
                    descriptor.Expires = DateTime.UtcNow.AddMinutes(60);
                    descriptor.IssuedAt = DateTime.UtcNow;
                    descriptor.SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Secret"])), SecurityAlgorithms.HmacSha256);

                    //Creamos el token
                    var token = handler.CreateToken(descriptor);
                    //Para escribir el token que sera enviado.
                    var tokenSerializado = handler.WriteToken(token);

                    return Ok(tokenSerializado);
                }
                else
                {
                    return Unauthorized("El usuario o la contraseña son incorrectos.");
                }
            }
            else
            {
                return BadRequest(errors);
            }
           
        }
    }
}
