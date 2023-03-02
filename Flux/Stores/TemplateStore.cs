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
        private readonly IUserStore userStore;
        private readonly IQueries<Todo> queries;
        private readonly ICommands<Todo> commands;
        public IList<Todo> Templates { get; private set; }
        public Action? OnChange { get; set; }

        public TemplateStore(IDispatcher dispatcher, IConfiguration configuration, IUserStore userStore)
        {
            var connectionString = configuration.GetConnectionString("Weekly");
            queries = new Queries<Todo>(connectionString, "Weekly", "Template");
            commands = new Commands<Todo>(connectionString, "Weekly", "Template");
            this.userStore = userStore;
            this.userStore.OnChange += Load;

            Templates = new List<Todo>();

            Load();

            dispatcher.Action += async payload =>
            {
                switch (payload.ActionType)
                {
                    case ActionType.ADD_TEMPLATE:
                        ((Todo)payload).UserId = this.userStore.Session.UserId;
                        bool success = await commands.AddOne((Todo)payload);
                        if (success)
                            Templates.Add((Todo)payload);
                        OnChange?.Invoke();
                        break;
                    case ActionType.DELETE_TEMPLATE:
                        await commands.RemoveOne((Todo)payload);
                        Templates.Remove((Todo)payload);
                        OnChange?.Invoke();
                        break;
                    case ActionType.UPDATE_TEMPLATE:
                        await commands.ReplaceOne((Todo)payload);
                        Templates = await queries.GetAll(x => x.UserId, this.userStore.Session?.UserId);
                        OnChange?.Invoke();
                        break;
                }
            };

        }

        public async void Load()
        {
            Templates = await queries.GetAll(x => x.UserId, userStore.Session?.UserId);
        }
    }
}
