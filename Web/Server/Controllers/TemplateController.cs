using Data.Models;
using Data;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Web.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
        // GET: api/<TemplateController>/userId
        [HttpGet("{userId}")]
        public async Task<IEnumerable<Template>> Get(Guid userId)
        {
            var result = await queries.GetAll(x => x.UserId, userId);
            return result;
        }


        // POST api/<TemplateController>
        [HttpPost]
        public async Task Post([FromBody] Template template)
        {
            await commands.AddOne(template);
        }

        // PUT api/<TemplateController>/5
        [HttpPut]
        public async Task Put([FromBody] Template template)
        {
            await commands.ReplaceOne(template);
        }

        // DELETE api/<TemplateController>/5
        [HttpDelete("{id}")]
        public async void Delete(string id)
        {
            await commands.RemoveOne(x => x.Id, id);
        }
    }
}
