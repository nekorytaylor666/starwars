using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ado_net
{
    public interface IGetFromAPI<T>
    {
        T GetObject(int id);

        List<T> GetListObjects(string url, out string urlNext);
    }
}
