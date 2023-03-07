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

        public UserStore(IDispatcher dispatcher, HttpClient httpClient)
        {
            this.httpClient = httpClient;
            Session = null;

            dispatcher.Action += async dispatchable =>
            {
                switch (dispatchable.ActionType)
                {
                    case ActionType.LOGIN_USER:
                        Session = await GetSession(((Dispatchable<Credentials>)dispatchable).Payload, "login");
                        OnChange?.Invoke();
                        break;
                    case ActionType.LOGOUT_USER:
                        Session = null;
                        OnChange?.Invoke();
                        break;
                    case ActionType.REGISTER_USER:
                        Session = await GetSession(((Dispatchable<Credentials>)dispatchable).Payload, "register");
                        OnChange?.Invoke();
                        break;
                }
            };
        }

        private async Task<Session?> GetSession(Credentials credentials, string action)
        {
            var response = await httpClient.PostAsJsonAsync($"/api/user/{action}", credentials);
            Session? session = null;
            if (response.IsSuccessStatusCode)
                session = await response.Content.ReadFromJsonAsync<Session?>();
            return session;      
        }

    }
}
