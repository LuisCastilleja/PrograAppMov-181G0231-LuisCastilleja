using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace AppMovilPasteleria.Models
{
    public class Pasteles
    {
        [PrimaryKey]
        public int Id { get; set; }
        public string NombrePastel { get; set; }
        public decimal Costo { get; set; }
        public DateTime FechaVenta { get; set; }
        public string Sucursal { get; set; }
    }
}

