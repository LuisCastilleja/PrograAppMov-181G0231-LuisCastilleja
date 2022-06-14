using AppClientePartidoEnVivo.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace AppClientePartidoEnVivo.Repositories
{
    public class PartidosRepository
    {
        SQLiteConnection conexion;

        public PartidosRepository()
        {
            var ruta = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "/partidos.db3";
            //Crear la bd y si existe la abre
            conexion = new SQLiteConnection(ruta);

            conexion.CreateTable<Partido>();
        }
        public Partido Get(int id)
        {
            return conexion.Table<Partido>().FirstOrDefault(x => x.Id == id);
        }
        public IEnumerable<Partido> GetAll()
        {
            return conexion.Table<Partido>();
        }
        public void Insert(Partido partido)
        {
            conexion.Insert(partido);
        }
        public void Update(Partido partido)
        {
            conexion.Update(partido);
        }
        public void Delete(Partido partido)
        {
            conexion.Delete(partido);
        }
    }
}
