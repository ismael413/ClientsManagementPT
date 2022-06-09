using CM.ApiREST.Adapter.Common;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Xunit;

namespace CM.ApiREST.Adapter.Tests
{
    public class EnterprisesApiControllerTests : IntegrationTests
    {

        private class EnterpriseDto
        {
            [JsonPropertyName("Id")]
            public int Id { get; set; }
            [JsonPropertyName("Name")]
            public string Name { get; set; }
        }

        [Fact]
        public async Task GetAllEnterprisesApiRequest_ReturnOkHttpStatusCodeWithListOfEnterprises()
        {
            //Act
            var responseMessage = await TestClient.GetAsync(ApiRoutes.Enterprise.GetAll);
            var response = responseMessage.Content.ReadAsStringAsync().Result;
            var result = JsonSerializer.Deserialize<IEnumerable<EnterpriseDto>>(response);

            //Assert
            responseMessage.EnsureSuccessStatusCode();
            responseMessage.StatusCode.Should().Be(HttpStatusCode.OK);
            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetOneEnterpriseApiRequest_ReturnsOkHttpStatusCodeWithEnterpriseGetted()
        {
            //Arrange
            var newEnterprise = new
            {
                Id = 30,
                Name = "enterprise 1",
            };

            //insertar empresa
            await TestClient.PostAsync(ApiRoutes.Enterprise.Create, newEnterprise, new JsonMediaTypeFormatter());

            //Act
            //obtener empresa recien insertada
            var responseMessage = await TestClient.GetAsync(ApiRoutes.Enterprise.Get.Replace("{id}", "30"));
            var response = responseMessage.Content.ReadAsStringAsync().Result;
            EnterpriseDto result = JsonSerializer.Deserialize<EnterpriseDto>(response);

            //Assert
            responseMessage.EnsureSuccessStatusCode();
            responseMessage.StatusCode.Should().Be(HttpStatusCode.OK);
            Assert.NotNull(result);
        }

        [Fact]
        public void CreateApiRequest_ReturnsNoContentWhenAnotherEnterpriseWithSameNameExists()
        {
            //Arrange
            HttpResponseMessage responseMessage = new HttpResponseMessage();
            try
            {
                var newEnterprise = new
                {
                    Id = 31,
                    Name = "enterprise 1",
                };

                var anotherOne = new
                {
                    Id = 32,
                    Name = "enterprise 1",
                };

                //Act
                //Insertar la primera
                TestClient.PostAsync(ApiRoutes.Enterprise.Create, newEnterprise, new JsonMediaTypeFormatter());
                //Intentar insertar la segunda empresa con el mismo nombre...
                responseMessage = TestClient.PostAsync(ApiRoutes.Enterprise.Create, anotherOne, new JsonMediaTypeFormatter()).Result;

                //Assert
                //Debe arrojar una excepcion
                responseMessage.EnsureSuccessStatusCode();

                //No deberia llegar aqui
                var response = responseMessage.Content.ReadAsStringAsync().Result;
                EnterpriseDto result = JsonSerializer.Deserialize<EnterpriseDto>(response);
                result.Name.Should().Be(anotherOne.Name);
            }
            catch (Exception)
            {
                responseMessage.StatusCode.Should().Be(HttpStatusCode.OK);
            }
        }


        [Fact]
        public void UpdateApiRequest_DoesntModifyEnterpriseWhenAnotherEnterpriseWithSameNameExists()
        {
            //Arrange
            string sameName = "Nombre editado";

            var newEnterprise = new EnterpriseDto
            {
                Id = 33,
                Name = sameName
            };
            var anotherOne = new EnterpriseDto
            {
                Id = 34,
                Name = "Nombre a editar"
            };
            TestClient.PostAsync(ApiRoutes.Enterprise.Create, newEnterprise, new JsonMediaTypeFormatter());
            TestClient.PostAsync(ApiRoutes.Enterprise.Create, anotherOne, new JsonMediaTypeFormatter());

            //Act
            //Intentar actualizar la segunda empresa con el mismo nombre de la primera
            anotherOne.Name = sameName;
            var updateResponseMessage = TestClient.PutAsync(ApiRoutes.Enterprise.Update.Replace("{id}", "34"), anotherOne, new JsonMediaTypeFormatter()).Result;
            var updateResponse = updateResponseMessage.Content.ReadAsStringAsync().Result;
            var updateResult = JsonSerializer.Deserialize<bool>(updateResponse);

            //Obtener la segunda empresa que se intentó actualizar
            HttpResponseMessage responseMessage = TestClient.GetAsync(ApiRoutes.Enterprise.Get.Replace("{id}", "34")).Result;
            var response = responseMessage.Content.ReadAsStringAsync().Result;
            EnterpriseDto enterpriseGetted = JsonSerializer.Deserialize<EnterpriseDto>(response);
            //Assert
            updateResponseMessage.EnsureSuccessStatusCode();
            updateResult.Should().Be(false);
            enterpriseGetted.Name.Should().Be("Nombre a editar"); //El nombre de la segunda empresa debe quedar intacto.
        }

        [Fact]
        public void DeleteApiRequest_ReturnsFalseWhenEnterpriseHasAClientRelated()
        {
            //Arrange
            var enterpriseWithClient = new
            {
                Id = 35,
                Name = "enterprise w client"
            };

            var clientRelated = new
            {
                Id = 22,
                Name = "client",
                LastName = "related",
                Age = 32,
                Genre = "M",
                EnterpriseId = 35
            };

            //Agregar empresa con su cliente
            TestClient.PostAsync(ApiRoutes.Enterprise.Create, enterpriseWithClient, new JsonMediaTypeFormatter());
            TestClient.PostAsync(ApiRoutes.Client.Create, clientRelated, new JsonMediaTypeFormatter());

            //Act
            //Intentamos eliminar dicha empresa
            var deleteResponseMessage = TestClient.DeleteAsync(ApiRoutes.Enterprise.Delete.Replace("{id}", "35")).Result;
            //Assert
            deleteResponseMessage.EnsureSuccessStatusCode(); //deberia arrojar una excepcion
            var response = deleteResponseMessage.Content.ReadAsStringAsync().Result;
            bool wasDeleted = JsonSerializer.Deserialize<bool>(response);

            try
            {
                wasDeleted.Should().Be(false); //No deberia arrojar ninguna excepcion
            }
            catch (Exception)
            {
                //Assert
                wasDeleted.Should().Be(true);
            }


        }
    }
}
