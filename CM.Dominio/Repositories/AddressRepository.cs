using CM.DominioApi.Port.Models.Addreses;
using CM.DominioApi.Port.Ports;
using CM.Persistence.Adapter.Context;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;


namespace CM.Dominio.Repositories
{
    public class AddressRepository : IBaseRepository<Address, int>
    {
        private readonly ApplicationDbContext context;

        public AddressRepository(ApplicationDbContext _context)
        {
            context = _context;
        }

        public Address Add(Address entidad)
        {
            context.Add(entidad);
            return context.SaveChanges() > 0 ? entidad : null;
        }

        public void Delete(int id)
        {
            var address = context.Addresses.SingleOrDefault(x => x.Id == id);
            context.Remove(address);
        }

        public IEnumerable<Address> GetAll()
        {
            return context.Addresses
                .Include(x => x.Country)
                .Include(x => x.City)
                .Include(x => x.Client);
        }

        public Address GetOne(int id)
        {
            return context.Addresses
                 .Include(x => x.Country)
                 .Include(x => x.City)
                 .Include(x => x.Client)
                 .SingleOrDefault(x => x.Id == id);
        }

        public void Update(int id, Address entidad)
        {
            var address = context.Addresses.SingleOrDefault(x => x.Id == id);

            address.CountryId = entidad.CountryId;
            address.CityId = entidad.CityId;
            address.StreetName = entidad.StreetName;
            address.BuildingName = entidad.BuildingName;
            address.FullAddress = entidad.FullAddress;

            context.Update(address);
            context.SaveChanges();
        }
    }
}
