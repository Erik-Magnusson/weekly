using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public interface IQueries<T>
    {
        Task<IList<T>> GetAll();
        Task<T?> GetOne(string id);

    }
}
