using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace AppMovilAnuncios.Models
{
   public class SimonPuntuacion
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int Puntuacion { get; set; }
        public DateTime Fecha { get; set; }
    }
}
