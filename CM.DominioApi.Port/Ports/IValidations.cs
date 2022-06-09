using CM.DominioApi.Port.Models;
using CM.DominioApi.Port.Models.Addreses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CM.DominioApi.Port.Ports
{
    public interface IValidations<T> where T: class
    {
        bool HasRelatedEntityOnDatabase(int id);
        bool ExistsWithNameOnCreating(string name);
        bool ExistsWithNameOnUpdating(string name, int id);
    }

    public interface IAddressValidations
    {
        bool ExistsSameAddressForSameClientOnCreating(string fullAddress, int clientId);
        bool ExistsSameAddressForSameClientOnUpdating(string fullAddress, int id, int clientId);
    }

    public interface IClientsValidations
    {
        bool ExistsWithNameOnCreating(string name);
        bool ExistsWithNameOnUpdating(string name, int id);
    }

    public interface ICitiesValidation
    {
        bool HasRelatedEntityOnDatabase(int id);
        bool ExistsWithNameForSameCountryOnCreating(string name, int countryId);
        bool ExistsWithNameForSameCountryOnUpdating(string name, int id, int countryId);
    }
  
}
