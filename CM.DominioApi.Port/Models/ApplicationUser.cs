using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CM.DominioApi.Port.Models
{
    public class ApplicationUser : IdentityUser
    {
        //relaciones
        public int AddressId { get; set; }
        public Address Address { get; set; }
    }
}
