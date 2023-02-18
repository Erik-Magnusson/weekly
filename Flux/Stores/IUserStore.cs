using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flux.Dispatchables;

namespace Flux.Stores
{
    public interface IUserStore : IStore
    {
        Session? Session { get; }
    }
}
