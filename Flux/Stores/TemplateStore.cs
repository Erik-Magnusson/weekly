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
        private readonly IQueries<TodoDispatchable> queries;
        private readonly ICommands<TodoDispatchable> commands;
        public IList<TodoDispatchable> Templates { get; private set; }
        public Action? OnChange { get; set; }

        public TemplateStore(IDispatcher dispatcher, IConfiguration configuration, IUserStore userStore)
        {
            var connectionString = configuration.GetConnectionString("Weekly");
            queries = new Queries<TodoDispatchable>(connectionString, "Weekly", "Template");
            commands = new Commands<TodoDispatchable>(connectionString, "Weekly", "Template");
            this.userStore = userStore;
            this.userStore.OnChange += Load;

            Templates = new List<TodoDispatchable>();

            Load();

            dispatcher.Action += async payload =>
            {
                switch (payload.ActionType)
                {
                    case ActionType.ADD_TEMPLATE:
                        ((TodoDispatchable)payload).UserId = this.userStore.Session.UserId;
                        bool success = await commands.AddOne((TodoDispatchable)payload);
                        if (success)
                            Templates.Add((TodoDispatchable)payload);
                        OnChange?.Invoke();
                        break;
                    case ActionType.DELETE_TEMPLATE:
                        await commands.RemoveOne((TodoDispatchable)payload);
                        Templates.Remove((TodoDispatchable)payload);
                        OnChange?.Invoke();
                        break;
                    case ActionType.UPDATE_TEMPLATE:
                        await commands.ReplaceOne((TodoDispatchable)payload);
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
