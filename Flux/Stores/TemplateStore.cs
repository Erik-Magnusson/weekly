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
    public class TemplateStore : ITemplateStore
    {
        private IQueries<Todo> Queries { get; set; }
        private ICommands<Todo> Commands { get; set; }
        public IList<Todo> Templates { get; private set; }

        public Action? OnChange { get; set; }
        public TemplateStore(IDispatcher dispatcher, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("Todo");
            Queries = new Queries<Todo>(connectionString, "Weekly", "Template");
            Commands = new Commands<Todo>(connectionString, "Weekly", "Template");

            Templates = new List<Todo>();

            Load();

            dispatcher.Action += async payload =>
            {
                switch (payload.ActionType)
                {
                    case ActionType.ADD_TEMPLATE:
                        await Commands.AddOne((Todo)payload);
                        Templates.Add((Todo)payload);
                        OnChange?.Invoke();
                        break;
                    case ActionType.DELETE_TEMPLATE:
                        await Commands.RemoveOne((Todo)payload);
                        Templates.Remove((Todo)payload);
                        OnChange?.Invoke();
                        break;
                    case ActionType.UPDATE_TEMPLATE:
                        await Commands.ReplaceOne((Todo)payload);
                        Templates = await Queries.GetAll();
                        OnChange?.Invoke();
                        break;
                }
            };

        }

        public async void Load()
        {
            Templates = await Queries.GetAll();
        }
    }
}
