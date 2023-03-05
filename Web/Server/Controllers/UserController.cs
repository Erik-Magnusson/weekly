using Data.Models;
using Data;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Web.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly IQueries<User> queries;
        private readonly ICommands<User> commands;

        public UserController(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("Weekly");
            queries = new Queries<User>(connectionString, "Weekly", "User");
            commands = new Commands<User>(connectionString, "Weekly", "User");
        }
    
        // GET api/<UserController>/username
        [HttpGet("{username}")]
        public async Task<User> Get(string username)
        {
            var result = await queries.GetAll((x => x.Username), username);
            return result.FirstOrDefault();
        }

        // POST api/<UserController>
        [HttpPost]
        public async Task Post([FromBody] User user)
        {
            await commands.AddOne(user);
        }

        // PUT api/<UserController>/5
        [HttpPut]
        public async Task Put([FromBody] User user)
        {
            await commands.ReplaceOne(user);
        }

        // DELETE api/<TodoController>/5
        [HttpDelete("{id}")]
        public async Task Delete(string id)
        {
            await commands.RemoveOne(x => x.Id, id);
        }

    }
}
