using Data;
using Data.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Web.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoController : ControllerBase
    {

        private readonly IQueries<Todo> queries;
        private readonly ICommands<Todo> commands;

        public TodoController(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("Weekly");
            queries = new Queries<Todo>(connectionString, "Weekly", "Todo");
            commands = new Commands<Todo>(connectionString, "Weekly", "Todo");
        }
        // GET: api/<TodoController>/userId
        [HttpGet("{userId}")]
        public async Task<IEnumerable<Todo>> Get(string userId)
        {
            var result = await queries.GetAll(x => x.UserId, new Guid(userId));
            return result;
        }

  
        // POST api/<TodoController>
        [HttpPost]
        public async Task Post([FromBody] Todo todo)
        {
            await commands.AddOne(todo);
        }

        // PUT api/<TodoController>/5
        [HttpPut]
        public async Task Put([FromBody] Todo todo)
        {
            await commands.ReplaceOne(todo);
        }

        // DELETE api/<TodoController>/5
        [HttpDelete("{id}")]
        public async Task Delete(string id)
        {
            await commands.RemoveOne(x => x.Id, id);
        }
    }
}
