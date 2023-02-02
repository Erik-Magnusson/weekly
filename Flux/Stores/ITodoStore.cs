using Flux.Dispatchables;

namespace Flux.Stores
{
    public interface ITodoStore
    {
        Action? OnChange { get; set; }
        
        IList<Todo> Todos { get; }
                
    }
}
