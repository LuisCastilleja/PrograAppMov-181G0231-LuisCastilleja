using System;
using System.Collections.Generic;
using System.Text;

namespace AppMovilComprasJwt.Models
{
    public class Compra
    {
        public int Id { get; set; }
        public string DescripcionCompra { get; set; }
        public int Costo { get; set; }
        public DateTime Fecha { get; set; }
        public string Tienda { get; set; }
    }
}
