
using Data;
using Data.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Web.Models;

namespace Web.Server.Services
{
    public class UserService
    {
        private ICommands<User> commands;
        private IQueries<User> queries;
        public UserService(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("Weekly");
            queries = new Queries<User>(connectionString, "Weekly", "User");
            commands = new Commands<User>(connectionString, "Weekly", "User");
        }

        public async Task<User?> GetUser(Guid userId)
        {
            var user = await queries.GetOne(x => x.UserId, userId);
            if (user == null)
                return null;

            return user;
        }

        public async Task<User?> AuthenticateUser(Credentials credentials)
        {
            var user = await queries.GetOne(x => x.Username, credentials.Username);
            if (user == null)
                return null;

            var salt = Convert.FromHexString(user.Salt);
            var encryptedPassword = Convert.ToHexString(KeyDerivation.Pbkdf2(credentials.Password, salt, new KeyDerivationPrf(), 10000, 64));

            if (encryptedPassword != user.Password)
                return null;

            return user;
        }

    public async Task<User?> CreateUser(Credentials credentials)
        {
            var existingUser = await queries.GetOne(x => x.Username, credentials.Username);
            if (existingUser != null)
                return null;

            var user = new User { Username = credentials.Username };

            var random = new Random();
            var salt = new byte[64];
            random.NextBytes(salt);

            var encryptedPassword = KeyDerivation.Pbkdf2(credentials.Password, salt, new KeyDerivationPrf(), 10000, 64);

            user.UserId = Guid.NewGuid();
            user.Salt = Convert.ToHexString(salt);
            user.Password = Convert.ToHexString(encryptedPassword);

            var success = await commands.AddOne(user);
            if (!success)
                return null;

            return user;
            
        }
    }
}
