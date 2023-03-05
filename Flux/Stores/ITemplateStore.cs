using Flux.Dispatchables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Models;

namespace Flux.Stores
{
    public interface ITemplateStore : IStore
    {
        IList<Template> Templates { get; }

    }
}
