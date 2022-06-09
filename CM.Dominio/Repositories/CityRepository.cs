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
        ICitiesValidation
    {
        private readonly ApplicationDbContext context;

        public CityRepository(ApplicationDbContext _context)
        {
            context = _context;
        }

        public City Add(City entidad)
        {
            //Hacer validaciones
            if (ExistsWithNameForSameCountryOnCreating(entidad.Name, entidad.CountryId))
                return null;

            context.Cities.Add(entidad);
            context.SaveChanges();

            return context.Cities.ToList().Last();
        }

        public bool HasRelatedEntityOnDatabase(int id)
        {
            //si esta ciudad pertenece a una direccion registrada...
            return context.Addresses.Any(x => x.CityId == id);
        }

        public bool Delete(int id)
        {
            var city = context.Cities.FirstOrDefault(x => x.Id == id);

            //hacer validaciones
            if (HasRelatedEntityOnDatabase(city.Id))
                return false;

            context.Remove(city);
            return context.SaveChanges() > 0;
        }

        public bool ExistsWithNameForSameCountryOnCreating(string name, int countryId)
        {
            //Si ya existe una ciudad con este nombre para el mismo pais en la base de datos
            return context.Cities
                .Any(x => x.Name == name && x.CountryId == countryId);
        }

        public bool ExistsWithNameForSameCountryOnUpdating(string name, int id, int countryId)
        {
            //Si ya existe una ciudad diferente con el mismo nombre para el mismo pais en la base de datos al actualizar
            return context.Cities
                .Any(x => x.Id != id && x.Name == name && x.CountryId == countryId);
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
                 .FirstOrDefault(x => x.Id == id);
        }

        public bool Update(int id, City entidad)
        {
            var city = context.Cities.FirstOrDefault(x => x.Id == id);
            city.Name = entidad.Name;
            city.CountryId = entidad.CountryId;

            //Hacer validaciones
            if (ExistsWithNameForSameCountryOnUpdating(city.Name, city.Id, city.CountryId))
            {
                context.Entry(city).State = EntityState.Unchanged;
                return false;
            }


            context.Update(city);
            return context.SaveChanges() > 0;
        }
    }
}
