using CM.DominioApi.Port.Models;
using CM.DominioApi.Port.Models.Addreses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CM.DominioApi.Port.Ports
{
    public interface IClientRepository<T, TKey>:
        IClientsValidations
    {
        IEnumerable<T> GetAll();
        T GetOne(TKey id);
        T Add(T entidad);
        bool Update(TKey id, T entidad);
        void Delete(T id);
        bool SaveChanges();
        IEnumerable<Address> GetAddresses(TKey id);
        bool ConfirmStatesEntriesAreDeleted(T entidad);
    }
}
