using CM.DominioApi.Port.Common;
using CM.DominioApi.Port.Models.Addreses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CM.DominioApi.Port.Models
{
    public class Client : BaseEntityProperties
    {
        public string LastName { get; set; }
        public int Age { get; set; }
        public char Genre { get; set; }

        //relaciones
        public int EnterpriseId { get; set; }
        public Enterprise Enterprise { get; set; }
        public ICollection<Address> Addresses { get; set; }
    }
}
