using Data.Models;

namespace Web.Server.Services
{
    public interface IJwtService
    {
        string? GenerateToken(User? user);
    }
}