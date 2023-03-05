using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public interface ICommands<T>
    {

        Task<bool> AddOne(T item);

        Task<bool> RemoveOne(T item);

        Task<bool> RemoveOne<U>(Expression<Func<T, U>> expression, U value);

        Task<bool> ReplaceOne(T item);  

    }
}
