using Web.Models;

namespace Web.Client.Services.Api
{
    public interface IApiService
    {
        Task<string?> LoginUser(Credentials credentials);
        Task<string?> RegisterUser(Credentials credentials);
        Task<IList<T>> Get<T>() where T : ApiEntityBase;
        Task<T?> Add<T>(T item) where T : ApiEntityBase;
        Task<T?> Delete<T>(T item) where T : ApiEntityBase;
        Task<T?> Update<T>(T item) where T : ApiEntityBase;

    }
}