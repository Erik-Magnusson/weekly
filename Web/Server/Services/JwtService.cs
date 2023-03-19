using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Data.Models;

namespace Web.Server.Services
{
    public class JwtService : IJwtService
    {
        public readonly string jwtSecurityKey;
        public const int JWT_TOKE_VALIDITY_MINS = 43200; // 30 days

        public JwtService(IConfiguration configuration) {
            jwtSecurityKey = configuration["JwtSecurityKey"];
        }

        public string? GenerateToken(User? user)
        {
            
            if (user == null)
                return null;

            var tokenExpiryTimeStamp = DateTime.Now.AddMinutes(JWT_TOKE_VALIDITY_MINS);
            var tokenKey = Encoding.ASCII.GetBytes(jwtSecurityKey);
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

        public JwtPayload? ReadTokenPayload(string? token)
        {
            if (string.IsNullOrEmpty(token))
                return null;

            try
            {
                var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
                var jwt = jwtSecurityTokenHandler.ReadJwtToken(token);
                return jwt.Payload;
            }
            catch 
            {
                return null;
            }
        }
    }

   
}
