using Flux.Dispatcher;
using Flux.Dispatchables;
using Data;
using Microsoft.Extensions.Configuration;
using System.Globalization;

namespace Flux.Stores
{
    public class TodoStore : ITodoStore
    {
        private readonly IUserStore userStore;
        private readonly IQueries<TodoDispatchable> queries;
        private readonly ICommands<TodoDispatchable> commands;
        public IList<TodoDispatchable> Todos { get; private set; }  
        public int Year { get; private set; }
        public int Week { get; private set; }
        public Action? OnChange { get; set; }

        private IList<TodoDispatchable> allTodos;

        private Calendar calendar;

        public TodoStore(IDispatcher dispatcher, IConfiguration configuration, IUserStore userStore)
        {
            var connectionString = configuration.GetConnectionString("Weekly");
            queries = new Queries<TodoDispatchable>(connectionString, "Weekly", "Todo");
            commands = new Commands<TodoDispatchable>(connectionString, "Weekly", "Todo");
            this.userStore = userStore;
            this.userStore.OnChange += Load;

            CultureInfo culture = new CultureInfo("sv-SE");
            calendar = culture.Calendar;
            Year = calendar.GetYear(DateTime.Now);
            Week = calendar.GetWeekOfYear(DateTime.Now, CalendarWeekRule.FirstFullWeek, DateTime.Now.DayOfWeek);
            

            allTodos = Todos = new List<TodoDispatchable>();

            Load();

            dispatcher.Action += async payload =>
            {
                switch (payload.ActionType)
                {
                    case ActionType.ADD_TODO:
                        var todo = (TodoDispatchable)payload;
                        todo.UserId = this.userStore.Session.UserId;
                        todo.Week = Week;
                        todo.Year= Year;
                        bool success = await commands.AddOne(todo);
                        if(success)
                        {
                            allTodos.Add(todo);
                            FilterTodos();
                        }
                        break;
                    case ActionType.DELETE_TODO:
                        await commands.RemoveOne((TodoDispatchable)payload);
                        allTodos.Remove((TodoDispatchable)payload);
                        FilterTodos();
                        break;
                    case ActionType.UPDATE_TODO:
                        await commands.ReplaceOne((TodoDispatchable)payload);
                        var idx = allTodos.IndexOf(allTodos.FirstOrDefault(x => x.Id == ((TodoDispatchable)payload).Id));
                        if(idx != -1)
                        {
                            allTodos[idx] = (TodoDispatchable)payload;
                            FilterTodos();
                        }
                        break;
                    case ActionType.UPDATE_WEEK:
                        Week = ((Week)payload).WeekNr;
                        if (Week == 0)
                        {
                            Year--;
                            Week = 52;
                        }
                        if (Week > 52)
                        {
                            Year++;
                            Week = 1;
                        }
                        FilterTodos();
                        break;
                }
                return;
            };
        }

        private void FilterTodos()
        {
            Todos = allTodos.Where(x => x.Week == Week && x.Year == Year).ToList();
            OnChange?.Invoke();
        }

        private async void Load()
        {
            allTodos = await queries.GetAll(x => x.UserId, userStore.Session?.UserId);
            FilterTodos();
        }

    }
}
