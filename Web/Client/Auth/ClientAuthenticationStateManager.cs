using Flux.Stores;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;


namespace Web.Client.Auth
{
    public class ClientAuthenticationStateProvider : AuthenticationStateProvider
    {

        private readonly IUserStore userStore;
        private readonly ClaimsPrincipal anonymous; 

        public ClientAuthenticationStateProvider(IUserStore userStore)
        {
            this.userStore = userStore;
            this.userStore.OnChange += UpdateAuthenticationState;
            this.anonymous = new ClaimsPrincipal(new ClaimsIdentity());
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                if (userStore.Session == null)
                    return await Task.FromResult(new AuthenticationState(anonymous));

                var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                {
                    new Claim(ClaimTypes.Name, userStore.Session.Username),
                    new Claim(ClaimTypes.NameIdentifier, userStore.Session.UserId.ToString())
                }, "JwtAuth"));

                return await Task.FromResult(new AuthenticationState(claimsPrincipal));
            }
            catch
            {
                return await Task.FromResult(new AuthenticationState(anonymous));
            }
        }

        private void UpdateAuthenticationState()
        {
            ClaimsPrincipal claimsPrincipal = anonymous;

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

        public string GetToken()
        {
            if (userStore.Session != null && DateTime.Now < userStore.Session.ExpiryTimeStamp)
                return userStore.Session.Token; 
            return string.Empty;
        }
    }
}
