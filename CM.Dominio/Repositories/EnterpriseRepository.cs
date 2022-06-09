﻿using CM.DominioApi.Port.Models;
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
    public class EnterpriseRepository : IBaseRepository<Enterprise, int>,
        IValidations<Enterprise>
    {
        private readonly ApplicationDbContext context;

        public EnterpriseRepository(ApplicationDbContext _context)
        {
            context = _context;
        }

        public Enterprise Add(Enterprise entidad)
        {
            if (ExistsWithNameOnCreating(entidad.Name))
                return null;

            context.Add(entidad);
            context.SaveChanges();

            return entidad;
        }

        public bool Delete(int id)
        {
            var enterprise = context.Enterprises.SingleOrDefault(x => x.Id == id);

            //hacer validaciones
            if (HasRelatedEntityOnDatabase(enterprise.Id))
                return false;

            context.Remove(enterprise);
            return context.SaveChanges() > 0;
        }

        public bool ExistsWithNameOnCreating(string name)
        {
            //Si ya existe una empresa con este nombre en la base de datos
            return context.Enterprises
                .Any(x => x.Name == name);
        }

        public bool ExistsWithNameOnUpdating(string name, int id)
        {
            //Si ya existe una empresa diferente con el mismo nombre en la base de datos al actualizar
            return context.Enterprises
                .Any(x => x.Id != id && x.Name == name);
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

        public bool HasRelatedEntityOnDatabase(int id)
        {
            //si esta empresa posee clientes registrados...
            return context.Enterprises
                .Include(x => x.Clients)
                .Any(x => x.Id == id && x.Clients.Count > 0);
        }

        public bool Update(int id, Enterprise entidad)
        {
            var enterprise = context.Enterprises.SingleOrDefault(x => x.Id == id);

            enterprise.Name = entidad.Name;
            enterprise.Description = entidad.Description;

            //Hacer validaciones
            if (ExistsWithNameOnUpdating(enterprise.Name, enterprise.Id))
            {
                context.Entry(enterprise).State = EntityState.Unchanged;
                return false;
            }

            context.Update(enterprise);
            return context.SaveChanges() > 0;
        }
    }
}
