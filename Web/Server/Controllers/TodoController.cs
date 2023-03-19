using Data;
using Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;



namespace Web.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
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
            if (!Guid.TryParse((string?)userId, out var userIdGuid))
                return Unauthorized();
            var result = await queries.GetAll(x => x.UserId, userIdGuid);
            return Ok(result);
        }


        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Todo todo)
        {
            var userId = HttpContext.Items["UserId"];
            if (!Guid.TryParse((string?)userId, out var userIdGuid))
                return Unauthorized();
            todo.UserId = userIdGuid;
            await commands.AddOne(todo);
            return Ok(todo);
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] Todo todo)
        {
            var userId = HttpContext.Items["UserId"];
            if (!Guid.TryParse((string?)userId, out var userIdGuid))
                return Unauthorized();
            if (userIdGuid != todo.UserId)
                return Unauthorized();
            await commands.ReplaceOne(todo);
            return Ok(todo);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var userId = HttpContext.Items["UserId"];
            if (!Guid.TryParse((string?)userId, out var userIdGuid))
                return Unauthorized();
            var item = await queries.GetOne(id);
            if (item == null)
                return BadRequest();
            if (userIdGuid != item.UserId)
                return Unauthorized();
            var success = await commands.RemoveOne(item);
            if (!success)
                return StatusCode(StatusCodes.Status500InternalServerError);
            return Ok(item);
        }
    }
}
