using CM.DominioApi.Port.Models.Addreses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CM.DominioApi.Port.Ports
{
    public interface IClientRepository
    {
        IEnumerable<Address> GetAddresses(int clientId);
        bool RemoveAddresses(int clientId);
    }
}
