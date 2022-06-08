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

        public bool AddClientAddress(Address address, int clientId)
        {
            address.ClientId = clientId;
            context.Addresses.Add(address);
            return context.SaveChanges() > 0;
        }

        public IEnumerable<Address> GetClientAddresses(int clientId)
        {
           return context.Addresses
                .Include(x => x.Client)
                .Include(x => x.Country)
                .Include(x => x.City)
                .Where(x => x.ClientId == clientId);
        }

        public bool RemoveClientAddress(int addressId, int clientId)
        {
            var address = context.Addresses.SingleOrDefault(x => x.Id == addressId && x.ClientId == clientId);
            context.Addresses.Add(address);
            return context.SaveChanges() > 0;
        }

        public void RemoveClientAddresses(int clientId)
        {
            var addresses = context.Addresses.Where(x => x.ClientId == clientId);
            
            if(addresses.Any())
                context.RemoveRange(addresses);
        }
    }
}
