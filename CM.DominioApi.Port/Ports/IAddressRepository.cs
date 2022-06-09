using CM.DominioApi.Port.Models.Addreses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CM.DominioApi.Port.Ports
{
    public interface IAddressRepository :
        IBaseRepository<Address,int>
    {
        string SetFullAddressString(Address address);
        IEnumerable<Address> GetAddressesByClientId(int clientId);
        void RemoveClientAddresses(IEnumerable<Address> addresses);
    }
}
