using APIPasteleria.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace APIPasteleria.Repositories
{
    public class PartidoEnVivoRepository : Repository<Partido>
    {
        public PartidoEnVivoRepository(DbContext context) : base(context)
        {

        }
        public override bool IsValid(Partido entity, out List<string> errors)
        {
            errors = new List<string>();
            if (string.IsNullOrWhiteSpace(entity.Equipos))
            {
                errors.Add("Proporcione los nombres de los equipos");
            }
            if (string.IsNullOrWhiteSpace(entity.DescripcionPartido))
            {
                errors.Add("Proporcione la descripción del partido");
            }
            if (string.IsNullOrWhiteSpace(entity.Goles))
            {
                errors.Add("Proporcione los goles de los equipos");
            }
            if (string.IsNullOrWhiteSpace(entity.Minuto))
            {
                errors.Add("Proporcione el minuto del partido");
            }
            if (string.IsNullOrWhiteSpace(entity.EstadoPartido))
            {
                errors.Add("Proporcione el estado del partido");
            }
            if (entity.FechaPartido.Date > DateTime.Now.Date)
            {
                errors.Add("La fecha del partido en vivo no puede ser mayor a la del dia de hoy");
            }
            return errors.Count == 0;
        }
    }
}
