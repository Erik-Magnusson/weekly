using Flux.Dispatchables;

namespace Flux.Stores
{
    public interface ITodoStore : IStore
    {        
        IList<Todo> Todos { get; }
        
    }
}
