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
    public class CityRepository : IBaseRepository<City, int>
    {
        private readonly ApplicationDbContext context;

        public CityRepository(ApplicationDbContext _context)
        {
            context = _context;
        }

        public City Add(City entidad)
        {
            context.Add(entidad);
            return context.SaveChanges() > 0 ? entidad : null;
        }

        public void Delete(int id)
        {
            var city = context.Cities.SingleOrDefault(x => x.Id == id);
            context.Remove(city);
        }

        public IEnumerable<City> GetAll()
        {
            return context.Cities
                .Include(x => x.Addresses);
        }

        public City GetOne(int id)
        {
            return context.Cities
                 .Include(x => x.Addresses)
                 .SingleOrDefault(x => x.Id == id);
        }

        public void Update(int id, City entidad)
        {
            var city = context.Cities.SingleOrDefault(x => x.Id == id);

            city.Name = entidad.Name;

            context.Update(city);
            context.SaveChanges();
        }
    }
}
