using Data.Models;
using Web.Models;

namespace Web.Server.Services
{
    public interface IUserService
    {
        Task<User?> AuthenticateUser(Credentials credentials);
        Task<User?> CreateUser(Credentials credentials);
        Task<User?> GetUser(Guid userId);
    }
}