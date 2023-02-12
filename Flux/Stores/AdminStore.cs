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
        private IQueries<Todo> TodoQueries { get; set; }
        private ICommands<Todo> TodoCommancs { get; set; }
        private IQueries<Icon> IconQueries { get; set; }
        public IList<Todo> Todos { get; private set; }
        public IList<Icon> Icons { get; private set; }

        public Action? OnChange { get; set; }
        public AdminStore(IDispatcher dispatcher, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("Todo");
            TodoQueries = new Queries<Todo>(connectionString, "Weekly", "Todo");
            TodoCommancs = new Commands<Todo>(connectionString, "Weekly", "Todo");
            IconQueries = new Queries<Icon>(connectionString, "Weekly", "Icons");

            Todos = new List<Todo>();
            Icons = new List<Icon>();

            Load();

        }

        public async void Load()
        {
            Todos = await TodoQueries.GetAll();
            Icons = await IconQueries.GetAll();
        }
    }
}
