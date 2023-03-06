using Flux.Dispatcher;
using Flux.Dispatchables;
using Data;
using Microsoft.Extensions.Configuration;
using System.Globalization;
using Data.Models;
using System.Net.Http;
using System.Net.Http.Json;

namespace Flux.Stores
{
    public class TodoStore : ITodoStore
    {

        private readonly HttpClient httpClient;
        private readonly IUserStore userStore;
        public IList<Todo> Todos { get; private set; }  
        public int Year { get; private set; }
        public int Week { get; private set; }
        public Action? OnChange { get; set; }

        private IList<Todo> allTodos;

        private Calendar calendar;

        public TodoStore(IDispatcher dispatcher, IUserStore userStore, HttpClient httpClient)
        {
            this.httpClient = httpClient;
            this.userStore = userStore;
            this.userStore.OnChange += Load;

            CultureInfo culture = new CultureInfo("sv-SE");
            calendar = culture.Calendar;
            Year = calendar.GetYear(DateTime.Now);
            Week = calendar.GetWeekOfYear(DateTime.Now, CalendarWeekRule.FirstFullWeek, DateTime.Now.DayOfWeek);
            

            allTodos = Todos = new List<Todo>();

            Load();

            dispatcher.Action += async payload =>
            {
                switch (payload.ActionType)
                {
                    case ActionType.ADD_TODO:
                        await AddTodo(((Dispatchable<Template>)payload).Value);
                        OnChange?.Invoke();
                        break;
                    case ActionType.DELETE_TODO:
                        await DeleteTodo(((Dispatchable<Todo>)payload).Value);
                        OnChange?.Invoke();
                        break;
                    case ActionType.UPDATE_TODO:
                        await UpdateTodo(((Dispatchable<Todo>)payload).Value);
                        OnChange?.Invoke();
                        break;
                    case ActionType.UPDATE_WEEK:
                        UpdateWeek(((Dispatchable<Week>)payload).Value);
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
                UserId = this.userStore.Session.UserId,
                Text= template.Text,
                NrDone = 0,
                NrTotal = template.NrTotal,
                Unit = template.Unit,
                Week = Week,
                Year = Year,
            };

            var response = await httpClient.PostAsJsonAsync<Todo>("/api/todo", todo);
            if (response.IsSuccessStatusCode)
            {
                allTodos.Add(todo);
                FilterTodos();
            }
        }

        private async Task DeleteTodo(Todo todo)
        {
            var response = await httpClient.DeleteAsync($"/api/todo/{todo.Id}");
            if (response.IsSuccessStatusCode)
            {
                allTodos.Remove(todo);
                FilterTodos();
            }
           
        }

        private async Task UpdateTodo(Todo todo)
        {
            var response = await httpClient.PutAsJsonAsync<Todo>($"/api/todo", todo);
            if (response.IsSuccessStatusCode)
            {
                var idx = allTodos.IndexOf(allTodos.FirstOrDefault(x => x.Id == todo.Id));
                if (idx != -1)
                {
                    allTodos[idx] = todo;
                    FilterTodos();
                }
            }
            
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
            var response = await httpClient.GetAsync($"/api/todo/{userStore.Session?.UserId}");
            allTodos = await response.Content.ReadFromJsonAsync<IList<Todo>>();
            FilterTodos();
            OnChange?.Invoke();
        }

    }
}
