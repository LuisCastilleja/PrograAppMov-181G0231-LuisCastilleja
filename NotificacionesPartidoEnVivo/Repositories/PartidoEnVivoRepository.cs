using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NotificacionesPartidoEnVivo.Models;
namespace NotificacionesPartidoEnVivo.Repositories
{
    public class PartidoEnVivoRepository
    {
        public bool IsValid(Partido entity, out List<string> errors)
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
