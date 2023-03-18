using Web.Models;
using Flux.Store;

namespace Web.Client.Stores
{
    public interface ITodoStore : IStore
    {        
        IList<Todo> Todos { get; }
        int Year { get; }
        int Week { get; }
        
    }
}
