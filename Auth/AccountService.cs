using Auth.Models;
using Data;
using Data.Models;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver.Core.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth
{
    public class AccountService
    {
        private ICommands<User> commands;
        private IQueries<User> queries;
        public AccountService(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("Weekly");
            queries = new Queries<User>(connectionString, "Weekly", "User");
            commands = new Commands<User>(connectionString, "Weekly", "User");

        }

        public Account? GetUser(string username, string password)
        {
            return null;

        }
    }
}
