using CM.Dominio.Repositories;
using CM.DominioApi.Port.Models.Addreses;
using CM.Persistence.Adapter.Context;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CM.Dominio.Tests
{
    public class CountryDominioTests
    {
        ApplicationDbContext context;
        CountryRepository countries;

        [SetUp]
        public void Setup()
        {
            context = new ApplicationDbContext();
            countries = new CountryRepository(context);
        }

        [Test]
        public void AddMethod_ReturnsNullWhenCountryNameAlreadyExists()
        {
            //Arrange
            context.Countries.Add(new Country()
            {
                Id = 10,
                Name = "Puerto Rico"
            });
            context.SaveChanges();

            //Act
            var addresult = countries.Add(new Country()
            {
                Id = 11,
                Name = "Puerto Rico"
            });

            //Assert
            Assert.IsNull(addresult);
            Assert.AreEqual(1, context.Countries.Count(x => x.Name == "Puerto Rico"));
        }

        [Test]
        public void UpdateMethod_ReturnsFalseWhenCountryNameAlreadyExists()
        {
            //Arrange
            context.Countries.Add(new Country()
            {
                Id = 11,
                Name = "Venezuela"
            });

            context.Countries.Add(new Country()
            {
                Id = 12,
                Name = "Spain"
            });
            context.SaveChanges();

            var countryToUpdate = context.Countries.Find(11);

            //Act
            countryToUpdate.Name = "Spain";
            var updateResult = countries.Update(11, countryToUpdate);

            //Assert
            Assert.IsFalse(updateResult);
            Assert.AreEqual("Venezuela", context.Countries.Find(11).Name);
        }

        [Test]
        public void DeleteMethod_FailsWhenCountryBelongsToAnAddress()
        {
            //Arrange
            context.Countries.Add(new Country()
            {
                Id = 13,
                Name = "Mexico"
            });

            context.Addresses.Add(new Address()
            {
                Id = 10,
                BuildingName = "Building1",
                StreetName = "Street1",
                CityId = 5,
                CountryId = 13,
                ClientId = 1,
                FullAddress = "fullAddress1"
            });
            context.SaveChanges();

            //Act
            var deleteResult = countries.Delete(13);

            //Assert
            Assert.IsFalse(deleteResult);
            Assert.IsTrue(context.Countries.Count(x => x.Id == 13) > 0);
            Assert.IsTrue(context.Countries.Include(x => x.Addresses).Any(x => x.Addresses.Count > 0));

        }

        [Test]
        public void DeleteMethod_ReturnFalseWhenCountryHasAtLeastOneCityRelated()
        {
            //Arrange
            context.Countries.Add(new Country()
            {
                Id = 14,
                Name = "Argentina"
            });

            context.Cities.Add(new City()
            {
                Id = 20,
                Name = "Buenos Aires",
                CountryId = 14
            });
            context.SaveChanges();

            //Act
            var deleteResult = countries.Delete(14);

            //Assert
            Assert.IsFalse(deleteResult);
            Assert.IsTrue(context.Countries.Any(x => x.Id == 14));
            Assert.IsTrue(context.Countries.Include(x => x.Cities).Any(x => x.Cities.Count > 0));
        }

    }
}
