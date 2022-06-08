using CM.DominioApi.Port.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CM.DominioApi.Port.Models
{
    public class Enterprise : BaseEntityProperties
    {
        public string Description { get; set; }

        //relaciones
        public ICollection<Client> Clients { get; set; }
    }
}
