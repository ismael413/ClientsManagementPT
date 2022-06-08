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
    public class ClientDominioTests
    {
        ApplicationDbContext context;
        ClientRepository clients;

        [SetUp]
        public void Setup()
        {
            context = new ApplicationDbContext();
            clients = new ClientRepository(context);
        }

        [Test]
        public void AddMethod_ReturnsNullWhenClientWithTheSameFullNameExists()
        {
            //Arrange

            //Act

            //Assert
        }

        [Test]
        public void UpdateMethod_ReturnsFalseWhenClientWithTheSameFullNameExists()
        {
            //Arrange

            //Act

            //Assert
        }
    }
}
