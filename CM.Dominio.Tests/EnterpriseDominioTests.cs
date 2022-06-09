using CM.Dominio.Repositories;
using CM.DominioApi.Port.Models;
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
            context.Enterprises.Add(new Enterprise()
            {
                Id = 1,
                Name = "Enterprise 1",
                Description = "Enterprise description 1"
            });
            context.SaveChanges();

            //Act
            var addresult = enterprises.Add(new Enterprise()
            {
                Id = 2,
                Name = "Enterprise 1",
                Description = "asd"
            });

            //Assert
            Assert.IsNull(addresult);
            Assert.AreEqual(null, context.Enterprises.Find(2));
        }

        [Test]
        public void UpdateMethod_ReturnsFalseWhenEnterpriseWithTheSameNameExists()
        {
            //Arrange
            context.Enterprises.AddRange(new List<Enterprise>()
            {
                new Enterprise()
                {
                    Id = 2,
                    Name = "Enterprise 2",
                    Description = "Enterprise description 1"
                },
                 new Enterprise()
                {
                    Id = 3,
                    Name = "Enterprise 3",
                    Description = "Enterprise description 1"
                }

            });
            context.SaveChanges();

            //Act
            var enterpriseToUpdate = context.Enterprises.Find(3);
            enterpriseToUpdate.Name = "Enterprise 2";
            bool updateResult = enterprises.Update(3, enterpriseToUpdate);

            //Assert
            Assert.IsFalse(updateResult);
            Assert.AreNotEqual(context.Enterprises.Find(2).Name, context.Enterprises.Find(3).Name);
        }

        [Test]
        public void DeleteMethod_ReturnsFalseWhenHasClientRelatedToThis()
        {
            //Arrange
            context.Enterprises.Add(new Enterprise()
            {
                Id = 4,
                Name = "Enterprise 4",
                Description = "Enterprise description 1"
            });

            context.Clients.Add(new Client()
            {
                Id = 15,
                Name = "Name 1",
                LastName = "Lastname 1",
                Genre = "M",
                Age = 32,
                EnterpriseId = 4
            });
            context.SaveChanges();

            //Act
            var deleteResult = enterprises.Delete(4);

            //Assert
            Assert.IsFalse(deleteResult);
            Assert.IsTrue(context.Enterprises.Any(x => x.Id == 4));
        }
    }
}
