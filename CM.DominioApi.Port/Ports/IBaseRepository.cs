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
        void Update(TKey id, T entidad);
        void Delete(TKey id);
    }
}
