using Amazon.Runtime.Internal;
using System.Globalization;
using Web.Server.Services;

namespace Web.Server.Middleware
{
    public class JwtTokenReaderMiddleware
    {

        private readonly RequestDelegate next;
        private readonly IJwtService jwtService;

        public JwtTokenReaderMiddleware(RequestDelegate next, IJwtService jwtService)
        {
            this.next = next;
            this.jwtService = jwtService;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var authHeader = context.Request.Headers["Authorization"];
            var bearer = authHeader.FirstOrDefault(s => s != null && s.Contains("bearer"));

            if (string.IsNullOrEmpty(bearer))
            {
                await next(context);
                return;
            }

            var splitBearer = bearer.Split(" ");

            if (splitBearer.Length != 2)
            {
                await next(context);
                return;
            }

            var payload = jwtService.ReadTokenPayload(splitBearer.Last());

            if (payload == null)
            {
                await next(context);
                return;
            }

            context.Items["UserId"] = payload["nameid"];
            await next(context);
        }


    }

    public static class JwtTokenReaderMiddlewareExtensions
    {
        public static IApplicationBuilder UseJwtTokenReader(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<JwtTokenReaderMiddleware>();
        }
    }
}
