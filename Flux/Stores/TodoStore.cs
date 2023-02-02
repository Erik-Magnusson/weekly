using Flux.Dispatcher;
using Flux.Dispatchables;
using Data;


namespace Flux.Stores
{
    public class TodoStore : ITodoStore
    {
        private IQueries<Todo> Queries { get; set; }
        private ICommands<Todo> Commands { get; set; }
        public IList<Todo> Todos { get; private set; }
     
        public Action? OnChange { get; set; }

        public TodoStore(IDispatcher dispatcher, IQueries<Todo> queries, ICommands<Todo> commands)
        { 
            Queries = queries;
            Commands = commands;

            Initialize();

            dispatcher.Action += async payload =>
            {
                switch (payload.ActionType)
                {
                    case ActionType.ADD:
                        await Commands.AddOne((Todo)payload);
                        Todos.Add((Todo)payload);
                        OnChange?.Invoke();
                        break;
                    case ActionType.REMOVE:
                        await Commands.RemoveOne((Todo)payload);
                        Todos.Remove((Todo)payload);
                        OnChange?.Invoke();
                        break;
                    case ActionType.UPDATE:
                        await Commands.ReplaceOne((Todo)payload);
                        Todos = await Queries.GetAll();
                        OnChange?.Invoke();
                        break;
                }
            };
        }

        private async void Initialize()
        {
            Todos = await Queries.GetAll();
        }

    }
}
