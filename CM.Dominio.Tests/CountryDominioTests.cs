using CM.Dominio.Repositories;
using CM.Persistence.Adapter.Context;
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

            //Act

            //Assert
        }

        [Test]
        public void UpdateMethod_ReturnsFalseWhenCountryNameAlreadyExists()
        {
            //Arrange

            //Act

            //Assert
        }

        [Test]
        public void DeleteMethod_FailsWhenCountryBelongsToAnAddress()
        {
            //Arrange

            //Act

            //Assert
        }

        [Test]
        public void DeleteMethod_ReturnFalseWhenCountryHasAtLeastOneCityRelated()
        {
            //Arrange

            //Act

            //Assert
        }

    }
}
