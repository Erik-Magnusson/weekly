using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flux.Dispatchables;
using Data.Models;

namespace Flux.Stores
{
    public interface IUserStore : IStore
    {
        string? Token { get; }
    }
}
