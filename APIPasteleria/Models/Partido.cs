using System;
using System.Collections.Generic;

#nullable disable

namespace APIPasteleria.Models
{
    public partial class Partido
    {
        public int Id { get; set; }
        public string Equipos { get; set; }
        public string DescripcionPartido { get; set; }
        public string Goles { get; set; }
        public string Minuto { get; set; }
        public string EstadoPartido { get; set; }
        public DateTime FechaPartido { get; set; }
        public ulong Eliminado { get; set; }
    }
}
