using Web.Models;
using Flux.Store;

namespace Web.Client.Stores
{
    public interface ITodoStore : IStore
    {        
        IList<Todo> Todos { get; }
        Week Week { get; }
        
    }
}
