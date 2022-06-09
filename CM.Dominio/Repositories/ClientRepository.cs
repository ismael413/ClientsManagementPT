using CM.DominioApi.Port.Models;
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
    public class ClientRepository : IClientRepository<Client, int>
    {
        private readonly ApplicationDbContext context;
        private readonly IAddressRepository addressRepository;

        public ClientRepository(
            ApplicationDbContext _context,
            IAddressRepository _addressRepository)
        {
            context = _context;
            addressRepository = _addressRepository;
        }

        public Client Add(Client entidad)
        {
            if (ExistsWithNameOnCreating(entidad.Name + " " + entidad.LastName))
                return null;

            context.Clients.Add(entidad);
            context.SaveChanges();
            return context.Clients.Last();
        }

        public bool ConfirmStatesEntriesAreDeleted(Client entidad)
        {
            var addresses = addressRepository.GetAddressesByClientId(entidad.Id);

            return context.Entry(addresses.First()).State == EntityState.Deleted && 
                context.Entry(entidad).State == EntityState.Deleted;
        }

        public void Delete(Client client)
        {
            //obtener direcciones del cliente a eliminar
            var addresses = addressRepository.GetAddressesByClientId(client.Id);

            //eliminar direcciones del cliente, si las hay...
            addressRepository.RemoveClientAddresses(addresses);

            context.Remove(client);
        }

        public bool ExistsWithNameOnCreating(string name)
        {
            //si ya existe un cliente con los mismos nombres y apellidos...
            return context.Clients
                .Any(x => (x.Name + " " + x.LastName).ToLower() == name.ToLower());
        }

        public bool ExistsWithNameOnUpdating(string name, int id)
        {
            //si ya existe un cliente diferente con los mismos nombres y apellidos...
            return context.Clients
                .Any(x => x.Id != id && (x.Name + " " + x.LastName).ToLower() == name.ToLower());
        }

        public IEnumerable<Address> GetAddresses(int id)
        {
            return addressRepository.GetAddressesByClientId(id);
        }

        public IEnumerable<Client> GetAll()
        {
            return context.Clients
                .Include(x => x.Addresses);
        }

        public Client GetOne(int id)
        {
            return context.Clients
                 .Include(x => x.Addresses)
                 .FirstOrDefault(x => x.Id == id);
        }

        public bool SaveChanges()
        {
            return context.SaveChanges() > 0;
        }

        public bool Update(int id, Client entidad)
        {
            var client = context.Clients.FirstOrDefault(x => x.Id == id);

            client.Name = entidad.Name;
            client.LastName = entidad.LastName;
            client.Age = entidad.Age;
            client.Genre = entidad.Genre;

            //Hacer validaciones
            if (ExistsWithNameOnUpdating(client.Name + " " + client.LastName, client.Id))
            {
                context.Entry(client).State = EntityState.Unchanged;
                return false;
            }

            context.Update(client);
            return context.SaveChanges() > 0;
        }

        
    }
}
