using CM.Dominio.Repositories;
using CM.DominioApi.Port.Models.Addreses;
using CM.DominioApi.Port.Ports;
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
        AddressRepository addresses;

        [SetUp]
        public void Setup()
        {
            context = new ApplicationDbContext();
            addresses = new AddressRepository(context);
        }

        [Test] 
        // Prueba para confirmar que no se agreguen dos direcciones iguales a un mismo cliente...
        public void AddMethod_ReturnsNullWhenClientAlreadyHasThisAddress()
        {
            //Arrange
            string fullAddress = "Country1, City1, Street1, Building1";

            Address newAddress = new Address()
            {
                Id = 1,
                CityId = 1,
                CountryId = 1,
                ClientId = 1,
                StreetName = "Street1",
                BuildingName = "Building1",
                FullAddress = fullAddress
            };
            var result = addresses.Add(newAddress);

            var address = new Address()
            {
                Id = 2,
                CityId = 1,
                CountryId = 1,
                ClientId = 1,
                StreetName = "Street1",
                BuildingName = "Building1",
                FullAddress = fullAddress
            };

            //Act
            var result2 = addresses.Add(address);

            //Assert
            Assert.AreEqual(result.FullAddress, fullAddress); //la primera direccion que se guardó
            Assert.IsNull(result2); //la segunda, no se guarda, y por ende devuelve nulo.
            Assert.IsNull(context.Addresses.Find(2)); //Confirmamos que no existe la segunda direccion que se intentó agregar.
        }

        [Test] 
        // Prueba para confirmar que no se actualice una direccion con la misma informacion
        // de otra direccion ya existente para el mismo cliente.
        public void UpdateMethod_ReturnsFalseWhenClientAlreadyHasThisAddress()
        {

            //Arrange
            context.Addresses.AddRange(new List<Address>()
            {
              new Address()
                {
                    Id = 3,
                    CityId = 1,
                    CountryId = 1,
                    ClientId = 1,
                    StreetName = "Street1",
                    BuildingName = "Building1",
                    FullAddress = "fullAddress 1"
                },

             new Address()
                {
                    Id = 4,
                    CityId = 1,
                    CountryId = 1,
                    ClientId = 1,
                    StreetName = "Street1",
                    BuildingName = "Building1",
                    FullAddress = "different"
                 }

            });

            context.SaveChanges();
            var firstAddress = context.Addresses.Find(3);
            var addressToUpdate = context.Addresses.Find(4);

            //Act
            addressToUpdate.FullAddress = firstAddress.FullAddress;
            var updateResult = addresses.Update(addressToUpdate.Id, addressToUpdate);

            //Assert
            Assert.IsTrue(addresses.ExistsSameAddressForSameClientOnUpdating("fullAddress 1",4, 1)); //se confirma que ya existe una direccion con "fullAddress 1"
            Assert.IsFalse(updateResult); //Entonces el metodo devuelve falso, indicando que no se realizaron los cambios.
            Assert.AreNotEqual(context.Addresses.Find(3).FullAddress, context.Addresses.Find(4).FullAddress); //Lo confirmamos evaluando la propiedad 'FullAddress' de cada registro.
        }
    }
}
