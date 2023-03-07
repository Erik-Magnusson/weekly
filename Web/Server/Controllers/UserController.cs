using Data.Models;
using Data;
using Microsoft.AspNetCore.Mvc;
using Web.Server.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Web.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly IQueries<User> queries;
        private readonly ICommands<User> commands;
        private readonly UserService userService;
        private readonly JwtService jwtService;

        public UserController(IConfiguration configuration, UserService userService, JwtService jwtService)
        {
            var connectionString = configuration.GetConnectionString("Weekly");
            queries = new Queries<User>(connectionString, "Weekly", "User");
            commands = new Commands<User>(connectionString, "Weekly", "User");
            this.userService = userService;
            this.jwtService = jwtService;
        }

        [Route("login")]
        [HttpPost]
        public async Task<Session?> Login([FromBody] Credentials credentials)
        {
            return await jwtService.GenerateToken(credentials);
        }

        [Route("register")]
        [HttpPost]
        public async Task<Session?> Register([FromBody] Credentials credentials)
        {
            var user = await userService.CreateUser(credentials);
            if (user == null)
                return null;
            return await jwtService.GenerateToken(credentials);
        }


    }
}
