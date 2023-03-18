using Flux.Store;

namespace Web.Client.Stores
{
    public interface IUserStore : IStore
    {
        string? Token { get; }
    }
}
