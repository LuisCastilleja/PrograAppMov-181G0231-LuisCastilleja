using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificacionesPartidoEnVivo.Models
{
    public class Partido
    {
        public int Id { get; set; }
        public string Equipos { get; set; } = "";
        public string DescripcionPartido { get; set; } = "";
        public string Goles { get; set; } = "";
        public string Minuto { get; set; } = "";
        public string EstadoPartido { get; set; } = "";
        public DateTime FechaPartido { get; set; } = DateTime.Now;
        public ulong Eliminado { get; set; }
    }
}
