using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flux.Stores
{
    public interface IStore
    {
        Action? OnChange { get; set; }
    }
}
