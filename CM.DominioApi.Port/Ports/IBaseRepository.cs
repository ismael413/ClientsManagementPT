using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CM.DominioApi.Port.Ports
{
    public interface IBaseRepository<T, TKey>  
    {
        IEnumerable<T> GetAll();
        T GetOne(TKey id);
        T Add(T entidad);
        bool Update(TKey id, T entidad);
        bool Delete(TKey id);
    }
}
