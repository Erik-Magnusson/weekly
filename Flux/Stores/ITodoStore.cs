using Data.Models;

namespace Flux.Stores
{
    public interface ITodoStore : IStore
    {        
        IList<Todo> Todos { get; }
        int Year { get; }
        int Week { get; }
        
    }
}
