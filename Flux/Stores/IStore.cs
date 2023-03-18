

namespace Flux.Stores
{
    public interface IStore
    {
        Action? OnChange { get; set; }
    }
}
