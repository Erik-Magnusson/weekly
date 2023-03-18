using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using Web.Client.Services.Misc;

namespace Web.Client.Services.Auth
{
    public class ClientAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly CookieService cookieService;
        private readonly ClaimsPrincipal anonymous;

        public ClientAuthenticationStateProvider(CookieService cookieService)
        {
            this.cookieService = cookieService;
            anonymous = new ClaimsPrincipal(new ClaimsIdentity());
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                var token = await cookieService.GetValueAsync<string>("weeklyAuth");
                if (token == null)
                    return await Task.FromResult(new AuthenticationState(anonymous));
                var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, token),
                }, "JwtAuth"));

                return await Task.FromResult(new AuthenticationState(claimsPrincipal));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return await Task.FromResult(new AuthenticationState(anonymous));
            }
        }

    }
}
