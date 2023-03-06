using Data;
using Flux.Dispatchables;
using Flux.Dispatcher;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Models;
using System.Net.Http.Json;

namespace Flux.Stores
{
    public class TemplateStore : ITemplateStore
    {
        private readonly IUserStore userStore;
        private readonly HttpClient httpClient;
        public IList<Template> Templates { get; private set; }
        public Action? OnChange { get; set; }

        public TemplateStore(IDispatcher dispatcher, IUserStore userStore, HttpClient httpClient)
        {
            this.httpClient = httpClient;
            this.userStore = userStore;
            this.userStore.OnChange += Load;

            Templates = new List<Template>();

            Load();

            dispatcher.Action += async payload =>
            {
                switch (payload.ActionType)
                {
                    case ActionType.ADD_TEMPLATE:
                        await AddTemplate(((Dispatchable<Template>)payload).Value);
                        OnChange?.Invoke();
                        break;
                    case ActionType.DELETE_TEMPLATE:
                        await DeleteTemplate(((Dispatchable<Template>)payload).Value);
                        OnChange?.Invoke();
                        break;
                    case ActionType.UPDATE_TEMPLATE:
                        await UpdateTemplate(((Dispatchable<Template>)payload).Value);
                        OnChange?.Invoke();
                        break;
                }
            };

        }

        private async Task AddTemplate(Template template)
        {
            template.UserId = this.userStore.Session.UserId;
            var response = await httpClient.PostAsJsonAsync<Template>("/api/template", template);
            if (response.IsSuccessStatusCode)
            {
                Templates.Add(template);
            }
            
        }

        private async Task DeleteTemplate(Template template)
        {
            var response = await httpClient.DeleteAsync($"/api/todo/{template.Id}");
            if (response.IsSuccessStatusCode)
            {
                Templates.Remove(template);
            }
        }

        private async Task UpdateTemplate(Template template)
        {
            var response = await httpClient.PutAsJsonAsync<Template>($"/api/todo", template);
            if (response.IsSuccessStatusCode)
            {
                var idx = Templates.IndexOf(Templates.FirstOrDefault(x => x.Id == template.Id));
                if (idx != -1)
                {
                    Templates[idx] = template;
                }
            }
        }

        public async void Load()
        {
            var response = await httpClient.GetAsync($"/api/template/{userStore.Session?.UserId}");
            Templates = await response.Content.ReadFromJsonAsync<IList<Template>>();
            OnChange?.Invoke();
        }
    }
}
