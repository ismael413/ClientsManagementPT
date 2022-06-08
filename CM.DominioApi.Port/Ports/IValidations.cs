using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CM.DominioApi.Port.Ports
{
    public interface IValidations<T> where T: class
    {
        bool IsValidTo(T entidad, Actions action);
        bool HasRelatedEntityOnDatabase(int id);
        bool ExistsWithNameOnCreating(string name);
        bool ExistsWithNameOnUpdating(string name, int id);
    }

    public enum Actions
    {
        Creating,
        Updating,
        Deleting
    }
}
