using AppClientePartidoEnVivo.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace AppClientePartidoEnVivo.Repositories
{
    public class PartidosRepository
    {
       public SQLiteConnection Context { get; set; }

        public PartidosRepository()
        {
            var ruta = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "/partidos.db3";
            //Crear la bd y si existe la abre
            Context = new SQLiteConnection(ruta);

            Context.CreateTable<Partido>();
        }
        public Partido Get(int id)
        {
            return Context.Table<Partido>().FirstOrDefault(x => x.Id == id);
        }
        public IEnumerable<Partido> GetAll()
        {
            return Context.Table<Partido>();
        }
        public void Insert(Partido partido)
        {
            Context.Insert(partido);
        }
        public void Update(Partido partido)
        {
            Context.Update(partido);
        }
        public void Delete(Partido partido)
        {
            Context.Delete(partido);
        }
    }
}
