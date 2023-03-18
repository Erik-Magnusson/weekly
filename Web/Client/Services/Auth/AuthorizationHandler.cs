using System.Net.Http.Headers;
using Web.Client.Services.Misc;

namespace Web.Client.Services.Auth
{
    public class AuthorizationHandler : DelegatingHandler
    {
        private readonly CookieService cookieService;

        public AuthorizationHandler(CookieService cookieService)
        {
            this.cookieService = cookieService;
        }
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var token = await cookieService.GetValueAsync<string?>("weeklyAuth");
            request.Headers.Authorization = new AuthenticationHeaderValue("bearer", token);
            Console.WriteLine(request);
            return await base.SendAsync(request, cancellationToken);
        }
    }
}
