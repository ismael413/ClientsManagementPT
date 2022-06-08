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
    public class CityRepository : IBaseRepository<City, int>,
        IValidations<City>
    {
        private readonly ApplicationDbContext context;

        public CityRepository(ApplicationDbContext _context)
        {
            context = _context;
        }

        public City Add(City entidad)
        {
            //Hacer validaciones
            if (!IsValidTo(entidad, Actions.Creating))
                return null;

            context.Add(entidad);
            return context.SaveChanges() > 0 ? entidad : null;
        }

        public bool HasRelatedEntityOnDatabase(int id)
        {
            //si esta ciudad pertenece a una direccion registrada...
            return context.Cities
                .Any(x => x.Id == id && x.Addresses.Count > 0);
        }

        public bool Delete(int id)
        {
            var city = context.Cities.SingleOrDefault(x => x.Id == id);

            //hacer validaciones
            if (!IsValidTo(city, Actions.Deleting))
                return false;

            context.Remove(city);
            return context.SaveChanges() > 0;
        }

        public bool ExistsWithNameOnCreating(string name)
        {
            //Si ya existe una ciudad con este nombre en la base de datos
            return context.Cities
                .Any(x => x.Name == name);
        }

        public bool ExistsWithNameOnUpdating(string name, int id)
        {
            //Si ya existe una ciudad diferente con el mismo nombre en la base de datos al actualizar
            return context.Cities
                .Any(x => x.Id != id && x.Name == name);
        }

        public IEnumerable<City> GetAll()
        {
            return context.Cities
                 .Include(x => x.Addresses)
                 .Include(x => x.Country);
        }

        public City GetOne(int id)
        {
            return context.Cities
                 .Include(x => x.Addresses)
                 .Include(x => x.Country)
                 .SingleOrDefault(x => x.Id == id);
        }

        public bool IsValidTo(City entidad, Actions action)
        {
            return action switch
            {
                Actions.Creating => !ExistsWithNameOnCreating(entidad.Name),
                Actions.Updating => !ExistsWithNameOnUpdating(entidad.Name,entidad.Id),
                Actions.Deleting => !HasRelatedEntityOnDatabase(entidad.Id),
                _ => false
            };
        }

        public bool Update(int id, City entidad)
        {
            var city = context.Cities.SingleOrDefault(x => x.Id == id);
            city.Name = entidad.Name;
            city.CountryId = entidad.CountryId;

            //Hacer validaciones
            if (!IsValidTo(city, Actions.Updating))
                return false;


            context.Update(city);
            return context.SaveChanges() > 0;
        }
    }
}
