using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CM.ApiREST.Adapter.Common
{
    public class ApiRoutes
    {
        public const string Root = "api";
        public const string Version = "v1";
        public const string Base = Root + "/" + Version;
        public static class Enterprise
        {
            public const string GetAll = Base + "/enterprisesapi";

            public const string Get = Base + "/enterprisesapi/{id}";

            public const string Create = Base + "/enterprisesapi";

            public const string Update = Base + "/enterprisesapi/{id}";

            public const string Delete = Base + "/enterprisesapi/{id}";
        }

        public static class Client
        {
            public const string GetAll = Base + "/clientsapi";

            public const string Get = Base + "/clientsapi/{id}";

            public const string Create = Base + "/clientsapi";

            public const string Update = Base + "/clientsapi/{id}";

            public const string Delete = Base + "/clientsapi/{id}{ensureDelete}";
        }

        public static class City
        {
            public const string GetAll = Base + "/citiesapi";

            public const string Get = Base + "/citiesapi/{id}";

            public const string Create = Base + "/citiesapi";

            public const string Update = Base + "/citiesapi/{id}";

            public const string Delete = Base + "/citiesapi/{id}";
        }

        public static class Country
        {
            public const string GetAll = Base + "/countriesapi";

            public const string Get = Base + "/countriesapi/{id}";

            public const string Create = Base + "/countriesapi";

            public const string Update = Base + "/countriesapi/{id}";

            public const string Delete = Base + "/countriesapi/{id}";
        }

        public static class Address
        {
            public const string GetAll = Base + "/addressesapi";

            public const string Get = Base + "/addressesapi/{id}";

            public const string Create = Base + "/addressesapi";

            public const string Update = Base + "/addressesapi/{id}";

            public const string Delete = Base + "/addressesapi/{id}";
        }
    }
}
