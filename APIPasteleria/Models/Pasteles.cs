using System;
using System.Collections.Generic;

#nullable disable

namespace APIPasteleria.Models
{
    public partial class Pasteles
    {
        public int Id { get; set; }
        public string NombrePastel { get; set; }
        public decimal Costo { get; set; }
        public DateTime FechaVenta { get; set; }
        public string Sucursal { get; set; }
        public DateTime TimeStamp { get; set; }
        public ulong Eliminado { get; set; }
    }
}
