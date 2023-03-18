using Flux.Dispatchables;
using Flux.Dispatcher;
using Flux.Services;
using Web.Models;

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
