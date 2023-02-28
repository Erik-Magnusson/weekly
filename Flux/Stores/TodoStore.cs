using Flux.Dispatcher;
using Flux.Dispatchables;
using Data;
using Microsoft.Extensions.Configuration;
using System.Globalization;

namespace Flux.Stores
{
    public class TodoStore : ITodoStore
    {
        private IUserStore UserStore { get; set; }
        private IQueries<Todo> Queries { get; set; }
        private ICommands<Todo> Commands { get; set; }
        public IList<Todo> Todos { get; private set; } 
        public int Week { get; private set; }
        public Action? OnChange { get; set; }

        private Calendar calendar;


        public TodoStore(IDispatcher dispatcher, IConfiguration configuration, IUserStore userStore)
        {
            var connectionString = configuration.GetConnectionString("Weekly");
            Queries = new Queries<Todo>(connectionString, "Weekly", "Todo");
            Commands = new Commands<Todo>(connectionString, "Weekly", "Todo");
            UserStore = userStore;
            UserStore.OnChange += Load;

            CultureInfo culture = new CultureInfo("sv-SE");
            calendar = culture.Calendar;
            Week = calendar.GetWeekOfYear(DateTime.Now, CalendarWeekRule.FirstFullWeek, DateTime.Now.DayOfWeek);

            Todos = new List<Todo>();

            Load();

            dispatcher.Action += async payload =>
            {
                switch (payload.ActionType)
                {
                    case ActionType.ADD_TODO:
                        ((Todo)payload).UserId = UserStore.Session.UserId;
                        bool success = await Commands.AddOne((Todo)payload);
                        if(success)
                            Todos.Add((Todo)payload);
                        OnChange?.Invoke();
                        break;
                    case ActionType.DELETE_TODO:
                        await Commands.RemoveOne((Todo)payload);
                        Todos.Remove((Todo)payload);
                        OnChange?.Invoke();
                        break;
                    case ActionType.UPDATE_TODO:
                        await Commands.ReplaceOne((Todo)payload);
                        Todos = await Queries.GetAll(x => x.UserId, UserStore.Session?.UserId);
                        OnChange?.Invoke();
                        break;
                    case ActionType.UPDATE_WEEK:
                        Week = ((Week)payload).WeekNr;
                        Load();
                        break;
                }
                return;
            };
        }

        private async void Load()
        {
            Todos = await Queries.GetAll(x => x.UserId, UserStore.Session?.UserId);
            Todos = Todos.Where(x => x.Week == Week).ToList();
            OnChange?.Invoke();
        }

    }
}
