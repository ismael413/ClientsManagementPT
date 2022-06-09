using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CM.ApiREST.Adapter.DTOs
{
    public class CountryDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }

        //propiedades no mapeadas en la BD
        public List<string> CitiesNames { get; set; }

    }
}
