using System;
using System.Collections.Generic;
using System.Text;

namespace AppMovilPasteleria.Models
{
    public enum Estado { Agregado,Modificado,Eliminado}
   public class PastelesEstado
    {
        public Pasteles Pastel { get; set; }
        public Estado Estado { get; set; }
    }
}
