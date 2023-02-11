using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public interface ICommands<T>
    {

        Task AddOne(T item);

        Task<bool> RemoveOne(T item);

        Task<bool> ReplaceOne(T item);  

    }
}
