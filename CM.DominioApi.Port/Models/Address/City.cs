using CM.DominioApi.Port.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CM.DominioApi.Port.Models.Addreses
{
    public class City : BaseEntityProperties
    {
        //relaciones
        public int CountryId { get; set; }
        public Country Country { get; set; }
        public ICollection<Address> Addresses { get; set; }
    }
}
