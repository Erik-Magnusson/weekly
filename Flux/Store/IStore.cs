

namespace Flux.Store
{
    public interface IStore
    {
        Action? OnChange { get; set; }
    }
}
