using Flux.Dispatcher;
using Flux.Dispatchable;
using Microsoft.Extensions.Configuration;
using System.Globalization;
using Web.Models;
using Web.Client.Services.Api;

namespace Web.Client.Stores
{
    public class TodoStore : ITodoStore
    {
        private readonly IApiService apiService;
        public IList<Todo> Todos { get; private set; }  
        public int Year { get; private set; }
        public int Week { get; private set; }
        public Action? OnChange { get; set; }

        private Session? session;
        private IList<Todo> allTodos;
        private Calendar calendar;

        public TodoStore(IDispatcher<ActionType> dispatcher, IApiService apiService)
        {
            this.apiService = apiService;
            this.session = null;

            CultureInfo culture = new CultureInfo("sv-SE");
            calendar = culture.Calendar;
            Year = calendar.GetYear(DateTime.Now);
            Week = calendar.GetWeekOfYear(DateTime.Now, CalendarWeekRule.FirstFullWeek, DateTime.Now.DayOfWeek);
         
            allTodos = Todos = new List<Todo>();

            Load();

            dispatcher.Action += async dispatchable =>
            {
                switch (dispatchable.ActionType)
                {
                    case ActionType.ADD_TODO:
                        await AddTodo(((Dispatchable<ActionType, Template>)dispatchable).Payload);
                        OnChange?.Invoke();
                        break;
                    case ActionType.DELETE_TODO:
                        await DeleteTodo(((Dispatchable<ActionType, Todo>)dispatchable).Payload);
                        OnChange?.Invoke();
                        break;
                    case ActionType.UPDATE_TODO:
                        await UpdateTodo(((Dispatchable<ActionType, Todo>)dispatchable).Payload);
                        OnChange?.Invoke();
                        break;
                    case ActionType.UPDATE_WEEK:
                        UpdateWeek(((Dispatchable<ActionType, Week>)dispatchable).Payload);
                        OnChange?.Invoke();
                        break;
                }
                return;
            };
        }

        private async Task AddTodo(Template template)
        {
            var todo = new Todo()
            {
                UserId = template.UserId,
                Text= template.Text,
                NrDone = 0,
                NrTotal = template.NrTotal,
                Unit = template.Unit,
                Week = Week,
                Year = Year,
            };

            var success = await apiService.Add(todo);
            if (!success)
                return;
            
            allTodos.Add(todo);
            FilterTodos();
        }

        private async Task DeleteTodo(Todo todo)
        {
            var success = await apiService.Delete(todo);
            if (!success)
                return;
            allTodos.Remove(todo);
            FilterTodos();
        }

        private async Task UpdateTodo(Todo todo)
        {
            var success = await apiService.Update(todo);
            if (!success)
                return;

            var idx = allTodos.IndexOf(allTodos.FirstOrDefault(x => x.Id == todo.Id));
            if (idx == -1)
                return;

            allTodos[idx] = todo;
            FilterTodos();
        }

        private void UpdateWeek(Week week)
        {
            Week = week.WeekNr;
            if (week.WeekNr == 0)
            {
                Year--;
                Week = 52;
            }
            if (week.WeekNr > 52)
            {
                Year++;
                Week = 1;
            }
            FilterTodos();
        }

        private void FilterTodos()
        {
            Todos = allTodos.Where(x => x.Week == Week && x.Year == Year).ToList();
        }

        private async void Load()
        {
            allTodos = await apiService.Get<Todo>();
            FilterTodos();
            OnChange?.Invoke();
        }

    }
}
