using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public interface IQueries<T>
    {
        Task<IList<T>> GetAll();
        Task<T?> GetOne(string id);
        Task<T?> GetOne<U>(Expression<Func<T, U>> expression, U value);

    }
}
