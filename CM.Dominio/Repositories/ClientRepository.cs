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
    public class ClientRepository : IBaseRepository<Client, int>
    {
        private readonly ApplicationDbContext context;

        public ClientRepository(ApplicationDbContext _context)
        {
            context = _context;
        }

        public Client Add(Client entidad)
        {
            context.Add(entidad);
            return context.SaveChanges() > 0 ? entidad : null;
        }

        public void Delete(int id)
        {
            var client = context.Clients.SingleOrDefault(x => x.Id == id);
            context.Remove(client);
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
                 .SingleOrDefault(x => x.Id == id);
        }

        public void Update(int id, Client entidad)
        {
            var client = context.Clients.SingleOrDefault(x => x.Id == id);

            client.Name = entidad.Name;
            client.LastName = entidad.LastName;
            client.Age = entidad.Age;
            client.Genre = entidad.Genre;

            context.Update(client);
            context.SaveChanges();
        }
    }
}
