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
    public class AddressDominioTests
    {
        ApplicationDbContext context;
        AddressRepository countries;

        [SetUp]
        public void Setup()
        {
            context = new ApplicationDbContext();
            countries = new AddressRepository(context);
        }

        [Test]
        public void AddMethod_ReturnsNullWhenClientAlreadyHasThisAddress()
        {
            //Arrange

            //Act

            //Assert
        }

        [Test]
        public void UpdateMethod_ReturnsFalseWhenClientAlreadyHasThisAddress()
        {
            //Arrange

            //Act

            //Assert
        }
    }
}
