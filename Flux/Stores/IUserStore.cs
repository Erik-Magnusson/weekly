
namespace Flux.Stores
{
    public interface IUserStore : IStore
    {
        string? Token { get; }
    }
}
