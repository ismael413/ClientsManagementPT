using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http;


namespace CM.ApiREST.Adapter.Tests
{
    public class IntegrationTests
    {
        public readonly HttpClient TestClient;
        public IntegrationTests()
        {
            var appFactory = new WebApplicationFactory<Startup>();
            TestClient = appFactory.CreateClient();
        }
    }
}
