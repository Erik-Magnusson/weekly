using Data;
using Flux.Dispatchables;
using Flux.Dispatcher;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flux.Stores
{
    public class AdminStore : IAdminStore
    {
        private IQueries<Todo> Queries { get; set; }
        private ICommands<Todo> Commands { get; set; }
        public IList<Todo> Todos { get; private set; }

        public Action? OnChange { get; set; }
        public AdminStore(IDispatcher dispatcher, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("Todo");
            Queries = new Queries<Todo>(connectionString, "Weekly", "Todo");
            Commands = new Commands<Todo>(connectionString, "Weekly", "Todo");

            Todos = new List<Todo>();

            Load();

        }

        public async void Load()
        {
            Todos = await Queries.GetAll();
        }
    }
}
