using Microsoft.AspNetCore.Mvc;
using Web.Server.Services;
using Web.Models;


namespace Web.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService userService;
        private readonly IJwtService jwtService;

        public UserController(IUserService userService, IJwtService jwtService)
        {
            this.userService = userService;
            this.jwtService = jwtService;
        }

        [Route("login")]
        [HttpPost]
        public async Task<string?> Login([FromBody] Credentials credentials)
        {
            var user = await userService.AuthenticateUser(credentials);
            return jwtService.GenerateToken(user);
        }

        [Route("register")]
        [HttpPost]
        public async Task<string?> Register([FromBody] Credentials credentials)
        {
            var user = await userService.CreateUser(credentials);
            return jwtService.GenerateToken(user);
        }


    }
}
