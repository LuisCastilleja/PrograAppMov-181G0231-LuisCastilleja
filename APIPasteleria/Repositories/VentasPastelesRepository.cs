using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using APIPasteleria.Repositories;
using APIPasteleria.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using APIPasteleria.Helpers;
namespace APIPasteleria.Repositories
{
    public class VentasPastelesRepository:Repository<Pasteles>
    {
        public VentasPastelesRepository(DbContext context):base(context)
        {

        }
        public override void Insert(Pasteles entity)
        {
            entity.Eliminado = 0;
            entity.FechaVenta = DateTime.Now;
            entity.TimeStamp = DateTime.Now.ToMexicoDateTime();
            base.Insert(entity);
        }
        public override void Update(Pasteles entity)
        {
            entity.FechaVenta = DateTime.Now;
            entity.TimeStamp = DateTime.Now.ToMexicoDateTime();
            base.Update(entity);
        }
        public override void Delete(Pasteles entity)
        {
            entity.Eliminado = 1;
            entity.TimeStamp = DateTime.Now.ToMexicoDateTime();
            base.Update(entity);
            int tiempoVida = 3;
            var fechaLimite = DateTime.Now.ToMexicoDateTime().Subtract(TimeSpan.FromDays(tiempoVida));
            var porEliminar = Context.Set<Pasteles>().Where(x => x.Eliminado == 1 && x.TimeStamp < fechaLimite);
            foreach (var venta in porEliminar)
            {
                Context.Remove(venta);
            }
            Context.SaveChanges();
        }
        public override IEnumerable<Pasteles> GetAll()
        {
            return base.GetAll().Where(x=>x.Eliminado==0);
        }
        public IEnumerable<Pasteles> GetSinceDate(DateTime f)
        {
            return base.GetAll().Where(x => x.TimeStamp >= f);
        }     
        public override bool IsValid(Pasteles entity, out List<string> errors)
        {
            errors = new List<string>();
            if (string.IsNullOrWhiteSpace(entity.NombrePastel))
            {
                errors.Add("Escriba el nombre del pastel");
            }       
            if (entity.Costo <= 0)
            {
                errors.Add("El precio del pastel debe ser mayor a 0 pesos mexicanos");
            }
            if (entity.FechaVenta.Date > DateTime.Now.Date)
            {
                errors.Add("La fecha de venta no puede ser mayor a la del día de hoy");
            }
            if (string.IsNullOrWhiteSpace(entity.Sucursal))
            {
                errors.Add("Escriba el nombre de la sucursal");
            }
            return errors.Count == 0;
        }       
    }
}
