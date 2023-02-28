using Flux.Dispatchables;
using Flux.Dispatcher;
using Flux.Stores;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System.Security.Claims;

namespace WeeklyWeb.Authentication
{
    public class WeeklyAuthenticationStateProvider : AuthenticationStateProvider
    {
        private IDispatcher Dispatcher { get; set; }
        private IUserStore UserStore { get; set; }

        private readonly ProtectedSessionStorage sessionStorage;

        private ClaimsPrincipal notAuthenticated = new ClaimsPrincipal(new ClaimsIdentity());

        public WeeklyAuthenticationStateProvider(ProtectedSessionStorage sessionStorage, IUserStore userStorage, IDispatcher dispatcher)
        {
            this.sessionStorage = sessionStorage;
            this.Dispatcher = dispatcher;
            this.UserStore = userStorage;
            this.UserStore.OnChange += UpdateAuthenticationState;
        }
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                var sessionStorageResult = await sessionStorage.GetAsync<Session>("weeklysession");
                var session = sessionStorageResult.Success ? sessionStorageResult.Value : null;
                if (session == null)
                {
                    return await Task.FromResult(new AuthenticationState(notAuthenticated));
                }
                var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                {
                    new Claim(ClaimTypes.Name, session.Username),
                    new Claim(ClaimTypes.NameIdentifier, session.UserId.ToString())
                }, "WeeklyAuth"));
                return await Task.FromResult(new AuthenticationState(claimsPrincipal));
            }
            catch
            {
                return await Task.FromResult(new AuthenticationState(notAuthenticated));
            }
           
        }

        public void Login(string username, string password)
        {
            var user = new User
            {
                ActionType = ActionType.LOGIN_USER,
                Username = username,
                Password = password
            };
            Dispatcher.Dispatch(user);
        }

        public void Logout()
        {
            var user = new User
            {
                ActionType = ActionType.LOGOUT_USER,
            };
            Dispatcher.Dispatch(user);
        }

        public void Register(string username, string password)
        {
            var user = new User
            {
                ActionType = ActionType.NEW_USER,
                Username = username,
                Password = password
            };
            Dispatcher.Dispatch(user);
        }

        public async void UpdateAuthenticationState()
        {

            ClaimsPrincipal claimsPrincipal = notAuthenticated;

            if (UserStore.Session != null)
            {
                claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                {
                    new Claim(ClaimTypes.Name, UserStore.Session.Username),
                    new Claim(ClaimTypes.NameIdentifier, UserStore.Session.UserId.ToString())
                }));
                await SetSession(UserStore.Session);
            }
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claimsPrincipal)));
           
        }

        private async Task SetSession(Session session)
        {
            try
            {
                await sessionStorage.SetAsync("weeklysession", session);
            }
            catch
            {
                //JS Interop Exception
            }
            
        }
    }
}
