using CM.DominioApi.Port.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CM.DominioApi.Port.Models.Addreses
{
    public class Country : BaseEntityProperties
    {
        //relaciones
        public ICollection<City> Cities { get; set; }
        public ICollection<Address> Addresses { get; set; }
    }
}
