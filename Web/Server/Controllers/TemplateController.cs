using Data.Models;
using Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Web.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TemplateController : ControllerBase
    {
        private readonly IQueries<Template> queries;
        private readonly ICommands<Template> commands;

        public TemplateController(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("Weekly");
            queries = new Queries<Template>(connectionString, "Weekly", "Template");
            commands = new Commands<Template>(connectionString, "Weekly", "Template");
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
        public async Task<IActionResult> Post([FromBody] Template template)
        {
            var userId = HttpContext.Items["UserId"];
            if (userId == null)
                return Unauthorized();
            if (Guid.Parse((string)userId) != template.UserId)
                return Unauthorized();
            await commands.AddOne(template);
            return Ok(template);
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] Template template)
        {
            var userId = HttpContext.Items["UserId"];
            if (userId == null)
                return Unauthorized();
            if (Guid.Parse((string)userId) != template.UserId)
                return Unauthorized();
            await commands.ReplaceOne(template);
            return Ok(template);
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
