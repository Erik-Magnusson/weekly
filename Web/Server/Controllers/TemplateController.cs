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

        private readonly IQueries<Todo> queries;
        private readonly ICommands<Todo> commands;

        public TemplateController(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("Weekly");
            queries = new Queries<Todo>(connectionString, "Weekly", "Todo");
            commands = new Commands<Todo>(connectionString, "Weekly", "Todo");
        }
        // GET: api/<TemplateController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<TemplateController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<TemplateController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<TemplateController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<TemplateController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
