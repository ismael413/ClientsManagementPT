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
    public class ClientRepository : IBaseRepository<Client, int>,
        IValidations<Client>,
        IClientRepository
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
            context.Add(entidad);
            return context.SaveChanges() > 0 ? entidad : null;
        }

        public bool Delete(int id)
        {
            var client = context.Clients.SingleOrDefault(x => x.Id == id);

            //eliminar direcciones del cliente, si las hay...
            addressRepository.RemoveClientAddresses(client.Id);

            context.Remove(client);

            return context.SaveChanges() > 0;
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

        public IEnumerable<Client> GetAll()
        {
            return context.Clients
                .Include(x => x.Addresses);
        }

        public IEnumerable<Address> GetAddresses(int clientId)
        {
            return addressRepository.GetClientAddresses(clientId);
        }

        public Client GetOne(int id)
        {
            return context.Clients
                 .Include(x => x.Addresses)
                 .SingleOrDefault(x => x.Id == id);
        }

        public bool HasRelatedEntityOnDatabase(int id)
        {
            //Normalmente un cliente debe de poder eliminarse en conjunto con sus direcciones...
            //Esta condicion puede cambiar si se agrega una entidad que pertenezca al cliente y que no pueda ser sensible a eliminaciones.
            return false;
        }

        public bool IsValidTo(Client entidad, Actions action)
        {
            return action switch
            {
                Actions.Creating => !ExistsWithNameOnCreating(entidad.Name),
                Actions.Updating => !ExistsWithNameOnUpdating(entidad.Name, entidad.Id),
                Actions.Deleting => !HasRelatedEntityOnDatabase(entidad.Id),
                _ => false
            };
        }

        public bool RemoveAddresses(int clientId)
        {
            addressRepository.RemoveClientAddresses(clientId);
            return context.SaveChanges() > 0;
        }

        public bool Update(int id, Client entidad)
        {
            var client = context.Clients.SingleOrDefault(x => x.Id == id);

            client.Name = entidad.Name;
            client.LastName = entidad.LastName;
            client.Age = entidad.Age;
            client.Genre = entidad.Genre;

            //Hacer validaciones
            if (!IsValidTo(client, Actions.Updating))
                return false;

            context.Update(client);
            return context.SaveChanges() > 0;
        }
    }
}
