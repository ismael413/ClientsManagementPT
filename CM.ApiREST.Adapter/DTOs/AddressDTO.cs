using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CM.ApiREST.Adapter.DTOs
{
    public class AddressDTO
    {
        public int Id { get; set; }
        public string FullAddress { get; set; }
        public int CityId { get; set; }
        public int CountryId { get; set; }
    }
}
