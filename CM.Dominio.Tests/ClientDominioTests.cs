using CM.Dominio.Repositories;
using CM.DominioApi.Port.Models;
using CM.DominioApi.Port.Models.Addreses;
using CM.DominioApi.Port.Ports;
using CM.Persistence.Adapter.Context;
using Microsoft.EntityFrameworkCore;
using Moq;
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
        Mock<IAddressRepository> addressRepository;

        [SetUp]
        public void Setup()
        {
            context = new ApplicationDbContext();
            addressRepository = new Mock<IAddressRepository>();
            addressRepository.Setup(x => x.RemoveClientAddresses(context.Addresses.ToList()));
            addressRepository.Setup(x => x.GetAddressesByClientId(10));

            clients = new ClientRepository(context, addressRepository.Object);
        }

        [Test]
        //Prueba para asegurar que no se ingresen clientes con la misma informacion en la base de datos...
        //En este caso se usa como ejemplo los campos nombre y apellido...
        public void AddMethod_ReturnsNullWhenClientWithTheSameFullNameExists()
        {
            //Arrange
            string[] fullName = new string[2] { "no", "repeat this" };

            context.AddRange(new List<Client>()
            {
                new Client()
                {
                    Id = 1,
                    Name = "Primer Nombre",
                    LastName = "Sgundo Nombre",
                    Age = 32,
                    EnterpriseId = 1,
                    Genre = "M"
                },

                new Client()
                {
                    Id = 2,
                    Name = fullName[0],
                    LastName = fullName[1],
                    Age = 32,
                    EnterpriseId = 1,
                    Genre = "M"
                }
            });
            context.SaveChanges();

            //Act
            var addResult = clients.Add(new Client()
            {
                Id = 3,
                Name = fullName[0],
                LastName = fullName[1],
                Age = 32,
                EnterpriseId = 1,
                Genre = "M"
            });

            //Assert
            Assert.IsNull(addResult);
            Assert.AreEqual(1, context.Clients.Count(x => x.Name == fullName[0] && x.LastName == fullName[1]));
        }

        [Test]
        //Prueba para asegurar que no se actualice un cliente con la informacion de otro cliente ya existente.

        public void UpdateMethod_ReturnsFalseWhenClientWithTheSameFullNameExists()
        {
            //Arrange
            context.Clients.Add(new Client()
            {
                Id = 3,
                Name = "Primer Nombre",
                LastName = "Primer Apellido",
                Age = 32,
                EnterpriseId = 1,
                Genre = "M"
            });
            context.Clients.Add(new Client()
            {
                Id = 4,
                Name = "Nombre",
                LastName = "Apellido",
                Age = 32,
                EnterpriseId = 1,
                Genre = "M"
            });
            context.SaveChanges();

            //Act
            var clientToUpdate = context.Clients.Find(4);
            clientToUpdate.Name = "Primer Nombre";
            clientToUpdate.LastName = "Primer Apellido";
            bool updateResult = clients.Update(4, clientToUpdate);

            //Assert
            Assert.IsFalse(updateResult);
            Assert.AreEqual(1, context.Clients.Count(x => x.Name == "Primer Nombre" && x.LastName == "Primer Apellido"));
        }

        [Test]
        // Prueba para asegurar que se remuevan todas las direcciones pertenecientes a un cliente, antes que se elimine dicho cliente...
        // Hasta que no se elimine por completo el cliente, estas direcciones quedan intactas en la base de datos.
        public void DeleteMethod_RemoveAllTheAddressesRelatedToThisClient()
        {
            //Arrange
            context.Clients.Add(new Client()
            {
                Name = "Client",
                LastName = "With Addresses",
                Age = 32,
                Genre = "M",
                Id = 10
            });

            context.Addresses.Add(new Address()
            {
                BuildingName = "asd",
                StreetName = "asd",
                CityId = 1,
                CountryId = 1,
                ClientId = 10,
                FullAddress = "any",
                Id = 10
            });
            context.SaveChanges();

            var addresses = context.Addresses.Where(x => x.ClientId == 10);

            var clientWithAddresses = context.Clients
                .FirstOrDefault(x => x.Id == 10);

            //Act
            clients.Delete(clientWithAddresses);

            //Assert
            Assert.AreEqual(EntityState.Deleted, context.Entry(addresses.First()).State); //Verificar que realmente se removieron las direcciones del cliente.
            Assert.IsTrue(context.Addresses.Any(x => x.ClientId == 10)); //Como no se han guardado los cambios, las direcciones del cliente aun son legibles.

            //guardamos los cambios
            clients.SaveChanges();
            Assert.IsFalse(context.Addresses.Any(x => x.ClientId == 10)); //Como ya no existe el cliente, tampoco sus direcciones.

        } 
    }
}
