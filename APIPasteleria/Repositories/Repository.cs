using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace APIPasteleria.Repositories
{
    public abstract class Repository<T> where T : class
    {
        public  DbContext Context { get; set; }
        protected Repository(DbContext context)
        {
            Context = context;
        }
        public virtual void Insert(T entity)
        {
            Context.Add(entity);
            Context.SaveChanges();
        }
        public virtual void Update(T entity)
        {
            Context.Update(entity);
            Context.SaveChanges();
        }
        public virtual void Delete(T entity)
        {
            Context.Remove(entity);
            Context.SaveChanges();
        }
        public virtual IEnumerable<T> GetAll()
        {
           return Context.Set<T>();
        }
        public virtual T GetById(object id)
        {
           return Context.Find<T>(id);
        }
        public virtual bool IsValid(T entity, out List<string> errors)
        {
           errors = new List<string>();            
          return false;
        }
    }
}
