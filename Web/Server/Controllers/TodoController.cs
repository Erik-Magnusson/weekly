using Data;
using Data.Models;
using Microsoft.AspNetCore.Mvc;


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


        [HttpGet]
        public async Task<IEnumerable<Todo>> Get()
        {
            var userId = HttpContext.Items["UserId"];
            if (userId == null)
                return null;
            var result = await queries.GetAll(x => x.UserId, Guid.Parse((string)userId));
            return result;
        }

  
        [HttpPost]
        public async Task Post([FromBody] Todo todo)
        {
            await commands.AddOne(todo);
        }

  
        [HttpPut]
        public async Task Put([FromBody] Todo todo)
        {
            await commands.ReplaceOne(todo);
        }


        [HttpDelete("{id}")]
        public async Task Delete(string id)
        {
            await commands.RemoveOne(x => x.Id, id);
        }
    }
}
