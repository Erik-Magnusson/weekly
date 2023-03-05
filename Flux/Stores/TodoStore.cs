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

        public TodoStore(IDispatcher dispatcher, IConfiguration configuration, IUserStore userStore)
        {
            httpClient = new HttpClient();
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
                        await AddTodo(((Dispatchable<Todo>)payload).Value);
                        break;
                    case ActionType.DELETE_TODO:
                        await DeleteTodo(((Dispatchable<Todo>)payload).Value);
                        break;
                    case ActionType.UPDATE_TODO:
                        await UpdateTodo(((Dispatchable<Todo>)payload).Value);
                        break;
                    case ActionType.UPDATE_WEEK:
                        Week = (((Dispatchable<Week>)payload).Value).WeekNr;
                        UpdateWeek();
                        break;
                }
                return;
            };
        }

        private async Task AddTodo(Todo todo)
        {
            todo.UserId = this.userStore.Session.UserId;
            todo.Week = Week;
            todo.Year = Year;
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

        private void UpdateWeek()
        {
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
        }

        private void FilterTodos()
        {
            Todos = allTodos.Where(x => x.Week == Week && x.Year == Year).ToList();
            OnChange?.Invoke();
        }

        private async void Load()
        {
            var response = await httpClient.GetAsync($"/api/todo/{userStore.Session?.UserId}");
            allTodos = await response.Content.ReadFromJsonAsync<IList<Todo>>();
            FilterTodos();
        }

    }
}
