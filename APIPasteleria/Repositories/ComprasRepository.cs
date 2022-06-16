using APIPasteleria.Models;
using APIPasteleria.Repositories;
using Microsoft.EntityFrameworkCore;

namespace APIPasteleria.Repositories
{
    public class ComprasRepository : Repository<Compra>
    {
        public ComprasRepository(DbContext context) : base(context)
        {
        }
    }
}
