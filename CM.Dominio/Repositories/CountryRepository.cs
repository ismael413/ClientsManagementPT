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
    public class CountryRepository : IBaseRepository<Country, int>,
        IValidations<Country>
    {
        private readonly ApplicationDbContext context;

        public CountryRepository(ApplicationDbContext _context)
        {
            context = _context;
        }

        public Country Add(Country entidad)
        {
            //Hacer validaciones
            if (!IsValidTo(entidad, Actions.Creating))
                return null;

            context.Add(entidad);
            return context.SaveChanges() > 0 ? entidad : null;
        }

        public bool Delete(int id)
        {
            var country = context.Countries.SingleOrDefault(x => x.Id == id);

            //Hacer validaciones
            if (!IsValidTo(country, Actions.Deleting))
                return false;

            context.Remove(country);

            return context.SaveChanges() > 0;
        }

        public bool ExistsWithNameOnCreating(string name)
        {
            //Si ya existe un pais con este nombre en la base de datos
            return context.Countries
                .Any(x => x.Name == name);
        }

        public bool ExistsWithNameOnUpdating(string name, int id)
        {
            //Si ya existe un pais diferente con el mismo nombre en la base de datos al actualizar
            return context.Countries
                .Any(x => x.Id != id && x.Name == name);
        }

        public IEnumerable<Country> GetAll()
        {
            return context.Countries
                 .Include(x => x.Cities)
                 .Include(x => x.Addresses);
        }

        public Country GetOne(int id)
        {
            return context.Countries
                 .Include(x => x.Cities)
                 .Include(x => x.Addresses)
                 .SingleOrDefault(x => x.Id == id);
        }

        public bool HasRelatedEntityOnDatabase(int id)
        {
            //si este pais pertenece a una direccion registrada o tiene una o varias ciudades relacionadas...
            return context.Countries
                .Any(x => x.Id == id && (x.Addresses.Count > 0 || x.Cities.Count > 0));
        }

        public bool IsValidTo(Country entidad, Actions action)
        {
            return action switch
            {
                Actions.Creating => !ExistsWithNameOnCreating(entidad.Name),
                Actions.Updating => !ExistsWithNameOnUpdating(entidad.Name, entidad.Id),
                Actions.Deleting => !HasRelatedEntityOnDatabase(entidad.Id),
                _ => false
            };
        }

        public bool Update(int id, Country entidad)
        {
            var country = context.Countries.SingleOrDefault(x => x.Id == id);

            country.Name = entidad.Name;

            //Hacer validaciones
            if (!IsValidTo(country, Actions.Updating))
                return false;

            context.Update(country);
            return context.SaveChanges() > 0;
        }
    }
}
