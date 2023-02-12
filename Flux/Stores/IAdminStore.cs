using Flux.Dispatchables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flux.Stores
{
    public interface IAdminStore
    {
        Action? OnChange { get; set; }

        IList<Icon> Icons { get; }

        IList<Todo> Todos { get; }

    }
}
