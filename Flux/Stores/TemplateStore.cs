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
        private IUserStore UserStore { get; set; }
        private IQueries<Todo> Queries { get; set; }
        private ICommands<Todo> Commands { get; set; }
        public IList<Todo> Templates { get; private set; }

        public Action? OnChange { get; set; }
        public TemplateStore(IDispatcher dispatcher, IConfiguration configuration, IUserStore userStore)
        {
            var connectionString = configuration.GetConnectionString("Weekly");
            Queries = new Queries<Todo>(connectionString, "Weekly", "Template");
            Commands = new Commands<Todo>(connectionString, "Weekly", "Template");
            UserStore = userStore;
            UserStore.OnChange += Load;

            Templates = new List<Todo>();

            Load();

            dispatcher.Action += async payload =>
            {
                switch (payload.ActionType)
                {
                    case ActionType.ADD_TEMPLATE:
                        ((Todo)payload).UserId = UserStore.Session.UserId;
                        bool success = await Commands.AddOne((Todo)payload);
                        if (success)
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
                        Templates = await Queries.GetAll(x => x.UserId, UserStore.Session?.UserId);
                        OnChange?.Invoke();
                        break;
                }
            };

        }

        public async void Load()
        {
            Templates = await Queries.GetAll(x => x.UserId, UserStore.Session?.UserId);
        }
    }
}
