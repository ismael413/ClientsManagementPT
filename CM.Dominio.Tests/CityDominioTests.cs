using CM.Dominio.Repositories;
using CM.DominioApi.Port.Models.Addreses;
using CM.Persistence.Adapter.Context;
using NUnit.Framework;
using System.Linq;

namespace CM.Dominio.Tests
{
    public class CityDominioTests
    {
        ApplicationDbContext context;
        CityRepository cities; 

        [SetUp]
        public void Setup()
        {
            context = new ApplicationDbContext();
            cities = new CityRepository(context);
        }

        [Test]
        // Prueba para asegurar que no se agregue mas de una ciudad con el mismo nombre para el mismo pais.
        public void AddMethod_ReturnsNullWhenCityNameAlreadyExistsForTheSameCountry()
        {
            //Arrange
            context.Countries.Add(new Country()
            {
                Id = 1,
                Name = "Republica Dominicana"
            });

            context.Cities.Add(new City()
            {
                Id = 1,
                Name = "Santo Domingo",
                CountryId = 1
            });
            context.SaveChanges();

            //Act
            City addResult = cities.Add(new City()
            {
                Id = 2,
                Name = "Santo Domingo",
                CountryId = 1
            });

            //Assert
            Assert.IsNull(addResult); 
            Assert.AreEqual(1, context.Cities.Count(x => x.CountryId == 1 && x.Name == "Santo Domingo")); 
           
        }

        [Test]
        // Prueba para asegurar que no se modifique una ciudad con el nombre de otra ciudad ya existente para el mismo pais en la base de datos.
        public void UpdateMethod_ReturnsNullWhenCityNameAlreadyExists()
        {
            //Arrange
            context.Cities.Add(new City()
            {
                Id = 3,
                Name = "Puerto Plata",
                CountryId = 1
            });

            context.Cities.Add(new City()
            {
                Id = 4,
                Name = "Nagua",
                CountryId = 1
            });
            context.SaveChanges();

            //Act
            City cityAdded = context.Cities.Find(4);
            cityAdded.Name = "Puerto Plata";
            bool updateResult = cities.Update(4, cityAdded);

            //Assert
            Assert.IsFalse(updateResult);
            Assert.AreEqual(1, context.Cities.Count(x => x.CountryId == 1 && x.Name == "Puerto Plata"));
        }

        [Test]
        //Prueba para asegurar que no se elimine ninguna ciudad que ya pertenezca a alguna direccion de algun cliente.
        public void DeleteMethod_ReturnsFalseWhenCityBelongsToAnAddress()
        {
            //Arrange
            context.Cities.Add(new City()
            {
                Id = 5,
                Name = "Montecristi",
                CountryId = 1
            });

            context.Addresses.Add(new Address()
            {
                Id = 5,
                BuildingName = "Building1",
                StreetName = "Street1",
                CityId = 5,
                CountryId = 1,
                ClientId = 1,
                FullAddress = "fullAddress1"
            });
            context.SaveChanges();

            //Act
            var deleteResult = cities.Delete(5);

            //Assert
            Assert.IsFalse(deleteResult);
            Assert.IsTrue(context.Cities.Count(x => x.Id == 5) > 0);
        }
    }
}