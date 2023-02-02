using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class Commands<T> : ICommands<T>
    {

        public Task<bool> AddOne(T item)
        {
            //todo: Add call to mongo
            
        }
    }
}
