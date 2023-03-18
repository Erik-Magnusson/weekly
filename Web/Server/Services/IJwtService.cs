using Data.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace Web.Server.Services
{
    public interface IJwtService
    {
        string? GenerateToken(User? user);
        JwtPayload? ReadTokenPayload(string? token);
    }
}