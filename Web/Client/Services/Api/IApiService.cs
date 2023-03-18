using Web.Models;

namespace Web.Client.Services.Api
{
    public interface IApiService
    {
        Task<string?> LoginUser(Credentials credentials);
        Task<string?> RegisterUser(Credentials credentials);
        Task<IList<T>> Get<T>() where T : ApiEntityBase;
        Task<bool> Add<T>(T item) where T : ApiEntityBase;
        Task<bool> Delete<T>(T item) where T : ApiEntityBase;
        Task<bool> Update<T>(T item) where T : ApiEntityBase;

    }
}