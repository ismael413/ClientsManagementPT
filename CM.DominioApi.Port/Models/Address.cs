using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CM.DominioApi.Port.Models
{
    public class Address
    {
        public int Id { get; set; }
        [Required, MaxLength(100, ErrorMessage = "Name cannot exceed 100 characters.") ]
        public string Name { get; set; }

        //relaciones
        public ICollection<Client> Clients { get; set; }
    }
}
