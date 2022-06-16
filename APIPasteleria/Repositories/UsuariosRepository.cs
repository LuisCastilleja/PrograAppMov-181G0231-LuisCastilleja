using APIPasteleria.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
namespace APIPasteleria.Repositories
{
    public class UsuariosRepository : Repository<Usuarios>
    {
        public UsuariosRepository(DbContext context) : base(context)
        {

        }
        public override bool IsValid(Usuarios entity, out List<string> errors)
        {
            errors = new List<string>();
            if (string.IsNullOrWhiteSpace(entity.Usuario))
            {
                errors.Add("Proporcione su usuario para iniciar sesión.");
            }
            if (string.IsNullOrWhiteSpace(entity.Password))
            {
                errors.Add("Proporcione su contraseña para iniciar sesión");
            }    
            return errors.Count == 0;
        }
    }
}
