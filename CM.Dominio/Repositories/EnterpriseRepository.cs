using CM.DominioApi.Port.Models;
using CM.DominioApi.Port.Ports;
using CM.Persistence.Adapter.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CM.Dominio.Repositories
{
    public class EnterpriseRepository : IBaseRepository<Enterprise, int>
    {
        private readonly ApplicationDbContext context;

        public EnterpriseRepository(ApplicationDbContext _context)
        {
            context = _context;
        }

        public Enterprise Add(Enterprise entidad)
        {
            context.Add(entidad);
            return context.SaveChanges() > 0 ? entidad : null;
        }

        public void Delete(int id)
        {
            var enterprise = context.Enterprises.SingleOrDefault(x => x.Id == id);
            context.Remove(enterprise);
        }

        public IEnumerable<Enterprise> GetAll()
        {
            return context.Enterprises
                .Include(x => x.Clients);
        }

        public Enterprise GetOne(int id)
        {
            return context.Enterprises
                 .Include(x => x.Clients)
                 .SingleOrDefault(x => x.Id == id);
        }

        public void Update(int id, Enterprise entidad)
        {
            var enterprise = context.Enterprises.SingleOrDefault(x => x.Id == id);

            enterprise.Name = entidad.Name;

            context.Update(enterprise);
            context.SaveChanges();
        }
    }
}
