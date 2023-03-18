using Web.Models;

namespace Flux.Services
{
    public interface IApiService
    {
        Task<string?> LoginUser(Credentials credentials);
        Task<string?> RegisterUser(Credentials credentials);
        Task<IList<T>> Get<T>();
        Task<bool> Add<T>(T item);
        Task<bool> Delete<T>(T item);
        Task<bool> Update<T>(T item); 

    }
}