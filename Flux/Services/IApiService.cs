using Data.Models;

namespace Flux.Services
{
    public interface IApiService
    {
        Task<Session?> LoginUser(Credentials credentials);
        Task<Session?> RegisterUser(Credentials credentials);
        Task<IList<T>> Get<T>() where T : DataEntityBase;
        Task<bool> Add<T>(T item) where T : DataEntityBase;
        Task<bool> Delete<T>(T item) where T : DataEntityBase;
        Task<bool> Update<T>(T item) where T : DataEntityBase;

    }
}