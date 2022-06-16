using System;
using System.Collections.Generic;

#nullable disable

namespace APIPasteleria.Models
{
    public partial class Usuarios
    {
        public int Id { get; set; }
        public string Usuario { get; set; }
        public string Password { get; set; }
    }
}
