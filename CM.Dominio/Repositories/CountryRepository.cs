using CM.DominioApi.Port.Models.Addreses;
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
    public class CountryRepository : IBaseRepository<Country, int>
    {
        private readonly ApplicationDbContext context;

        public CountryRepository(ApplicationDbContext _context)
        {
            context = _context;
        }

        public Country Add(Country entidad)
        {
            context.Add(entidad);
            return context.SaveChanges() > 0 ? entidad : null;
        }

        public void Delete(int id)
        {
            var country = context.Countries.SingleOrDefault(x => x.Id == id);
            context.Remove(country);
        }

        public IEnumerable<Country> GetAll()
        {
            return context.Countries
                .Include(x => x.Addresses);
        }

        public Country GetOne(int id)
        {
            return context.Countries
                 .Include(x => x.Addresses)
                 .SingleOrDefault(x => x.Id == id);
        }

        public void Update(int id, Country entidad)
        {
            var country = context.Countries.SingleOrDefault(x => x.Id == id);

            country.Name = entidad.Name;

            context.Update(country);
            context.SaveChanges();
        }
    }
}
