﻿using Data;
using Flux.Dispatchables;
using Flux.Dispatcher;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver.Core.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Collections;

namespace Flux.Stores
{
    public class UserStore : IUserStore
    {
        private readonly IQueries<User> queries;
        private readonly ICommands<User> commands;
        public Action? OnChange { get; set; }
        public Session? Session { get; private set; }
        
        public UserStore(IDispatcher dispatchatcher, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("Weekly");
            queries = new Queries<User>(connectionString, "Weekly", "User");
            commands = new Commands<User>(connectionString, "Weekly", "User");
            Session = null;
            

            dispatchatcher.Action += async payload =>
            {
                switch (payload.ActionType)
                {
                    case ActionType.LOGIN_USER:
                        Session = await LoginUser((User)payload);
                        OnChange?.Invoke();
                        break;
                    case ActionType.LOGOUT_USER:
                        Session = null;
                        OnChange?.Invoke();
                        break;
                    case ActionType.NEW_USER:
                        Session = await NewUser((User)payload);
                        OnChange?.Invoke();
                        break;
                }
            };
        }

        private async Task<Session?> NewUser(User user)
        {
            var existingUser = await queries.GetOne(x => x.Username, user.Username);
            if (existingUser != null)
            {
                return null;
            }

            var random = new Random();
            var salt = new byte[48];
            random.NextBytes(salt);

            var encryptedPassword = KeyDerivation.Pbkdf2(user.Password, salt, new KeyDerivationPrf(), 10000, 64);

            user.UserId = Guid.NewGuid();
            user.Salt = Convert.ToHexString(salt);
            user.Password = Convert.ToHexString(encryptedPassword);

            bool success = await commands.AddOne(user);
            if (success)
                return new Session
                {
                    UserId = user.UserId,
                    Username = user.Username
                };
            return null;
        }

        private async Task<Session?> LoginUser(User userToAuthenticate)
        {
       
            User user = await queries.GetOne(x => x.Username, userToAuthenticate.Username);
            if (user == null)
            {
                return null;
            }

            var salt = Convert.FromHexString(user.Salt);
            var encryptedPassword = Convert.ToHexString(KeyDerivation.Pbkdf2(userToAuthenticate.Password, salt, new KeyDerivationPrf(), 10000, 64));

            if (encryptedPassword == user.Password)
            {
                return new Session
                {
                    Username = user.Username,
                    UserId = user.UserId
                };
            }
            return null;         
        }


    }
}
