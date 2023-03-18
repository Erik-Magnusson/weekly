using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Data.Models;

namespace Web.Server.Services
{
    public class JwtService : IJwtService
    {
        public const string JWT_SECURITY_KEY = "sdfhsdoifhh784yr843yr843yr834yr87y348r7y3487ry348ry8347rfsdoif";
        public const int JWT_TOKE_VALIDITY_MINS = 20;

        public string? GenerateToken(User? user)
        {
            if (user == null)
                return null;

            var tokenExpiryTimeStamp = DateTime.Now.AddMinutes(JWT_TOKE_VALIDITY_MINS);
            var tokenKey = Encoding.ASCII.GetBytes(JWT_SECURITY_KEY);
            var claimsIdentity = new ClaimsIdentity(new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString())
            });
            var signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(tokenKey),
                SecurityAlgorithms.HmacSha256Signature);
            var securityTokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claimsIdentity,
                Expires = tokenExpiryTimeStamp,
                SigningCredentials = signingCredentials
            };
            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var securityToken = jwtSecurityTokenHandler.CreateToken(securityTokenDescriptor);
            var token = jwtSecurityTokenHandler.WriteToken(securityToken);

            return token;
        }
    }
}
