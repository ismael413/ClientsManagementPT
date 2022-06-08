using CM.DominioApi.Port.Models.Addreses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CM.DominioApi.Port.Ports
{
    public interface IAddressRepository
    {
        IEnumerable<Address> GetClientAddresses(int clientId);
        bool AddClientAddress(Address address, int clientId);
        bool RemoveClientAddress(int addressId, int clientId);
        void RemoveClientAddresses(int clientId);
    }
}
