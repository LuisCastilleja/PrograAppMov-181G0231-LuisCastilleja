using AppMovilAnuncios.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace AppMovilAnuncios.Repositories
{
    public class SimonRepository
    {
        SQLiteConnection conexion;

        public SimonRepository()
        {
            var ruta = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "/simon.db3";
            //Crear la bd y si existe la abre
            conexion = new SQLiteConnection(ruta);

            conexion.CreateTable<SimonPuntuacion>();
        }
        public IEnumerable<SimonPuntuacion> GetAll()
        {
            return conexion.Table<SimonPuntuacion>().OrderByDescending(x => x.Puntuacion);
        }

        public void Insert(SimonPuntuacion simon)
        {
            conexion.Insert(simon);
        }
    }
}
