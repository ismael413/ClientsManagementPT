using CM.DominioApi.Port.Models.Addreses;
using CM.DominioApi.Port.Ports;
using CM.Persistence.Adapter.Context;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;


namespace CM.Dominio.Repositories
{
    public class AddressRepository : IAddressRepository
    {
        private readonly ApplicationDbContext context;

        public AddressRepository(ApplicationDbContext _context)
        {
            context = _context;
        }

        public Address Add(Address address)
        {
            //Si existe esta direccion para un mismo cliente, no agregarla...
            if (ExistsSameAddressForSameClientOnCreating(address.FullAddress, address.ClientId))
                return null;

            context.Addresses.Add(address);
            context.SaveChanges();
            return context.Addresses.ToList().Last();
        }

        public bool Delete(int id)
        {
            var address = context.Addresses.FirstOrDefault(x => x.Id == id);
            context.Addresses.Remove(address);
            return context.SaveChanges() > 0;
        }

        public bool ExistsSameAddressForSameClientOnCreating(string fullAddress, int clientId)
        {
            //Si ya existe la misma direccion para el mismo cliente...
            return context.Addresses
                .Any(x => x.FullAddress.ToLower() == fullAddress.ToLower() && x.ClientId == clientId);
        }

        public bool ExistsSameAddressForSameClientOnUpdating(string fullAddress, int id, int clientId)
        {
            //Si ya existe la misma direccion para el mismo cliente...
            return context.Addresses
                .Any(x => x.Id != id && x.FullAddress.ToLower() == fullAddress.ToLower() && x.ClientId == clientId);
        }

        public IEnumerable<Address> GetAddressesByClientId(int clientId)
        {
            return context.Addresses
                .Where(x => x.ClientId == clientId);
        }

        public IEnumerable<Address> GetAll()
        {
            return context.Addresses
               .Include(x => x.Client)
               .Include(x => x.Country)
               .Include(x => x.City);
        }

        public Address GetOne(int id)
        {
            return context.Addresses
               .FirstOrDefault(x => x.Id == id);
        }

        public void RemoveClientAddresses(IEnumerable<Address> addresses)
        {
            context.Addresses.RemoveRange(addresses);
        }

        public string SetFullAddressString(Address address)
        {
            return address.Country.Name + " " + address.City.Name + " " + address.StreetName + " " + address.BuildingName;
        }

        public bool Update(int id, Address entidad)
        {
            var address = context.Addresses.FirstOrDefault(x => x.Id == id);

            address.ClientId = entidad.ClientId;
            address.CountryId = entidad.CountryId;
            address.CityId = entidad.CityId;
            address.StreetName = entidad.StreetName;
            address.BuildingName = entidad.BuildingName;
            address.FullAddress = entidad.FullAddress;

            //hacer validaciones
            if (ExistsSameAddressForSameClientOnUpdating(address.FullAddress, address.Id, address.ClientId))
            {
                context.Entry(address).State = EntityState.Unchanged;
                return false;
            }

            context.Addresses.Update(address);
            return context.SaveChanges() > 0;
        }
    }
}
