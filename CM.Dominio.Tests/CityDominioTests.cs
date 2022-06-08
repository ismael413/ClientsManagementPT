using CM.Dominio.Repositories;
using CM.Persistence.Adapter.Context;
using NUnit.Framework;

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
        public void AddMethod_ReturnsNullWhenCityNameAlreadyExists()
        {
            //Arrange

            //Act

            //Assert
        }

        [Test]
        public void UpdateMethod_ReturnsNullWhenCityNameAlreadyExists()
        {
            //Arrange

            //Act

            //Assert
        }

        [Test]
        public void DeleteMethod_FailsWhenCityBelongsToACountryOrToAnAddress()
        {
            //Arrange

            //Act

            //Assert
        }
    }
}