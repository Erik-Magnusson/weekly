using Data;
using Data.Models;
using Microsoft.AspNetCore.Http.HttpResults;
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
        public async Task<IActionResult> Get()
        {
            var userId = HttpContext.Items["UserId"];
            if (userId == null)
                return Unauthorized();
            var result = await queries.GetAll(x => x.UserId, Guid.Parse((string)userId));
            return Ok(result);
        }


        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Todo todo)
        {
            var userId = HttpContext.Items["UserId"];
            if (userId == null)
                return Unauthorized();
            if (Guid.Parse((string)userId) != todo.UserId)
                return Unauthorized();
            await commands.AddOne(todo);
            return Ok(todo);
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] Todo todo)
        {
            var userId = HttpContext.Items["UserId"];
            if (userId == null)
                return Unauthorized();
            if (Guid.Parse((string)userId) != todo.UserId)
                return Unauthorized();
            await commands.ReplaceOne(todo);
            return Ok(todo);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var userId = HttpContext.Items["UserId"];
            if (userId == null)
                return Unauthorized();
            var item = await queries.GetOne(id);
            if (Guid.Parse((string)userId) != item.UserId)
                return Unauthorized();
            var success = await commands.RemoveOne(item);
            if (!success)
                return StatusCode(StatusCodes.Status500InternalServerError);
            return Ok(item);
        }
    }
}
