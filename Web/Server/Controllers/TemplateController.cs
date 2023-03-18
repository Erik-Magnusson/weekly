using Data.Models;
using Data;
using Microsoft.AspNetCore.Mvc;
using Web.Server.Services;


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
   
        [HttpGet]
        public async Task<IEnumerable<Template>> Get()
        {
            var result = await queries.GetAll(x => x.NrTotal, 2);
            return result;
        }


        [HttpPost]
        public async Task Post([FromBody] Template template)
        {
            await commands.AddOne(template);
        }

        [HttpPut]
        public async Task Put([FromBody] Template template)
        {
            await commands.ReplaceOne(template);
        }


        [HttpDelete("{id}")]
        public async void Delete(string id)
        {
            await commands.RemoveOne(x => x.Id, id);
        }
    }
}
