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
    class EnterpriseDominioTests
    {
        ApplicationDbContext context;
        EnterpriseRepository enterprises;

        [SetUp]
        public void Setup()
        {
            context = new ApplicationDbContext();
            enterprises = new EnterpriseRepository(context);
        }

        [Test]
        public void AddMethod_ReturnsNullWhenEnterpriseWithTheSameNameExists()
        {
            //Arrange

            //Act

            //Assert
        }

        [Test]
        public void UpdateMethod_ReturnsFalseWhenEnterpriseWithTheSameNameExists()
        {
            //Arrange

            //Act

            //Assert
        }
    }
}
