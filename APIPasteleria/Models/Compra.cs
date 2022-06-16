using System;
using System.Collections.Generic;

#nullable disable

namespace APIPasteleria.Models
{
    public partial class Compra
    {
        public int Id { get; set; }
        public string DescripcionCompra { get; set; }
        public int Costo { get; set; }
        public DateTime Fecha { get; set; }
        public string Tienda { get; set; }
    }
}
