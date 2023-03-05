using Data;
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
using Data.Models;
using System.Net.Http.Json;

namespace Flux.Stores
{
    public class UserStore : IUserStore
    {
        private readonly HttpClient httpClient;
        public Action? OnChange { get; set; }
        public Session? Session { get; private set; }
        
        public UserStore(IDispatcher dispatcher)
        {
            httpClient = new HttpClient();    
            Session = null;    

            dispatcher.Action += async payload =>
            {
                switch (payload.ActionType)
                {
                    case ActionType.LOGIN_USER:
                        Session = await LoginUser(((Dispatchable<User>)payload).Value);
                        OnChange?.Invoke();
                        break;
                    case ActionType.LOGOUT_USER:
                        Session = null;
                        OnChange?.Invoke();
                        break;
                    case ActionType.NEW_USER:
                        Session = await NewUser(((Dispatchable<User>)payload).Value);
                        OnChange?.Invoke();
                        break;
                }
            };
        }

        private async Task<Session?> NewUser(User user)
        {
            var response = await httpClient.GetAsync($"/api/user/{user.Username}");
            var existingUser = await response.Content.ReadFromJsonAsync<User>();
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

            response = await httpClient.PostAsJsonAsync<User>("/api/user", user);

            if (response.IsSuccessStatusCode)
                return new Session
                {
                    UserId = user.UserId,
                    Username = user.Username
                };
            return null;
        }

        private async Task<Session?> LoginUser(User userToAuthenticate)
        {
       
            var response = await httpClient.GetAsync($"/api/user/{userToAuthenticate.Username}");
            var user = await response.Content.ReadFromJsonAsync<User>();
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
