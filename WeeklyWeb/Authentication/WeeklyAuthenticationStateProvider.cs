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
        private IDispatcher dispatcher { get; set; }
        private IUserStore userStore { get; set; }

        private readonly ProtectedSessionStorage sessionStorage;

        private ClaimsPrincipal notAuthenticated = new ClaimsPrincipal(new ClaimsIdentity());

        public WeeklyAuthenticationStateProvider(ProtectedSessionStorage sessionStorage, IUserStore userStorage, IDispatcher dispatcher)
        {
            this.sessionStorage = sessionStorage;
            this.dispatcher = dispatcher;
            this.userStore = userStorage;
            this.userStore.OnChange += UpdateAuthenticationState;
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
            dispatcher.Dispatch(user);
        }

        public void Logout()
        {
            var user = new User
            {
                ActionType = ActionType.LOGOUT_USER,
            };
            dispatcher.Dispatch(user);
        }

        public void Register(string username, string password)
        {
            var user = new User
            {
                ActionType = ActionType.NEW_USER,
                Username = username,
                Password = password
            };
            dispatcher.Dispatch(user);
        }

        public void UpdateAuthenticationState()
        {

            ClaimsPrincipal claimsPrincipal = notAuthenticated;

            if (userStore.Session != null)
            {
                claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                {
                    new Claim(ClaimTypes.Name, userStore.Session.Username),
                    new Claim(ClaimTypes.NameIdentifier, userStore.Session.UserId.ToString())
                }));
            }
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claimsPrincipal)));
        }

        public async Task SetSession(Session session)
        {
            await sessionStorage.SetAsync("weeklysession", session);
        }
    }
}
