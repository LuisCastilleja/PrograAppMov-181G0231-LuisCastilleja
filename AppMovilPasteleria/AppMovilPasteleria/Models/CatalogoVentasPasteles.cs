using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
namespace AppMovilPasteleria.Models
{
    public class CatalogoVentasPasteles
    {
        SQLiteConnection conexion;
        public CatalogoVentasPasteles()
        {
            var ruta = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "/ventaspastelesLECT.db3";
            conexion = new SQLiteConnection(ruta);
            conexion.CreateTable<Pasteles>();
        }
        public IEnumerable<Pasteles> GetAll()
        {
            return conexion.Table<Pasteles>();
        }
        public IEnumerable<Pasteles> GetAllByFiltro(string filtro) 
        {
            if (filtro.ToLower() == "todos")
            {
                return conexion.Table<Pasteles>().ToList();
            }
            else
            {
                var filtrados = conexion.Table<Pasteles>().Where(x => x.NombrePastel.ToLower().Contains(filtro.ToLower())).ToList();
                return filtrados;
            }
        }

        public void InsertOrReplace(Pasteles pastel)
        {
            var registro = conexion.Find<Pasteles>(pastel.Id);
            //Eliminar venta
            if (pastel.NombrePastel == null)
            {
                if (registro != null)
                {
                    conexion.Delete(registro);
                }
            }
            else
            {
                //Agregar venta
                if (registro == null)
                {
                    conexion.Insert(pastel);
                }
                //Editar Venta
                else
                {
                    registro.NombrePastel = pastel.NombrePastel;
                    registro.Sucursal = pastel.Sucursal;
                    registro.FechaVenta = pastel.FechaVenta;
                    registro.Costo = pastel.Costo;
                    conexion.Update(registro);
                }
            }
        }
    }
}
