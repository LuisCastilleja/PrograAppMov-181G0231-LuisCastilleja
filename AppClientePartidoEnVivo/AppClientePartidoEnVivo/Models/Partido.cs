using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace AppClientePartidoEnVivo.Models
{
    public class Partido
    {
        [PrimaryKey]
        public int Id { get; set; }
        public string Equipos { get; set; }
        public string DescripcionPartido { get; set; }
        public string Goles { get; set; }
        public string Minuto { get; set; }
        public string EstadoPartido { get; set; }
        public DateTime FechaPartido { get; set; }
        [Ignore]
        public ulong Eliminado { get; set; }
    }
}
