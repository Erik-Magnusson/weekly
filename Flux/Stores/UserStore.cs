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
using Flux.Services;

namespace Flux.Stores
{
    public class UserStore : IUserStore
    {
        private readonly IApiService apiService;
        public Action? OnChange { get; set; }
        public string? Token { get; private set; }

        public UserStore(IDispatcher dispatcher, IApiService apiService)
        {
            this.apiService = apiService;
            Token = null;

            dispatcher.Action += async dispatchable =>
            {
                switch (dispatchable.ActionType)
                {
                    case ActionType.LOGIN_USER:
                        Token = await apiService.LoginUser(((Dispatchable<Credentials>)dispatchable).Payload);
                        OnChange?.Invoke();
                        break;
                    case ActionType.LOGOUT_USER:
                        Token = null;
                        OnChange?.Invoke();
                        break;
                    case ActionType.REGISTER_USER:
                        Token = await apiService.RegisterUser(((Dispatchable<Credentials>)dispatchable).Payload);
                        OnChange?.Invoke();
                        break;
                }
            };
        }

    }
}
