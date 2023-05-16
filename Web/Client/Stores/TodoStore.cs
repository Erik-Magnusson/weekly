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
        
        public IList<Todo>? Todos
        { 
            get
            {
                return _todos?.Where(x => x.Week.WeekNr == Week.WeekNr && x.Week.Year == Week.Year).ToList();
            }
            private set
            { 
                _todos = value; 
            }
        }  
        public Action? OnChange { get; set; }
        public Week Week { get; private set; }

        private readonly Calendar calendar;
        private readonly IApiService apiService;
        private IList<Todo>? _todos;
        

        public TodoStore(IDispatcher<ActionType> dispatcher, IApiService apiService)
        {
            this.apiService = apiService;

            CultureInfo culture = new CultureInfo("sv-SE");
            calendar = culture.Calendar;
            Week = new Week
            {
                Year = calendar.GetYear(DateTime.Now),
                WeekNr = calendar.GetWeekOfYear(DateTime.Now, CalendarWeekRule.FirstFullWeek, DateTime.Now.DayOfWeek)
            };

            _todos = null;

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
                        Week = ((Dispatchable<ActionType, Week>)dispatchable).Payload;
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
                Text= template.Text,
                NrDone = 0,
                NrTotal = template.NrTotal,
                Unit = template.Unit,
                Week = Week,
            };

            var addedTodo = await apiService.Add(todo);
            if (addedTodo == null)
                return;
            
            _todos.Add(addedTodo);
        }

        private async Task DeleteTodo(Todo todo)
        {
            var deletedTodo = await apiService.Delete(todo);
            if (deletedTodo == null)
                return;
            _todos = _todos.Where(t => t.Id != deletedTodo.Id).ToList();
        }

        private async Task UpdateTodo(Todo todo)
        {
            var updatedTodo = await apiService.Update(todo);
            if (updatedTodo == null)
                return;

            var idx = _todos.IndexOf(_todos.FirstOrDefault(x => x.Id == todo.Id));
            if (idx < 0)
                return;

            _todos[idx] = todo;
        }

        private async void Load()
        {
            _todos = await apiService.Get<Todo>();
            OnChange?.Invoke();
        }

    }
}
