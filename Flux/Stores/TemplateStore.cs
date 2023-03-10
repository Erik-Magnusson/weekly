﻿using Data;
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
using Flux.Services;

namespace Flux.Stores
{
    public class TemplateStore : ITemplateStore
    {
        private readonly IUserStore userStore;
        private readonly IApiService apiService;
        public IList<Template> Templates { get; private set; }
        public Action? OnChange { get; set; }

        public TemplateStore(IDispatcher dispatcher, IUserStore userStore, IApiService apiService)
        {
            this.apiService = apiService;
            this.userStore = userStore;
            this.userStore.OnChange += Load;

            Templates = new List<Template>();

            Load();

            dispatcher.Action += async dispatchable =>
            {
                switch (dispatchable.ActionType)
                {
                    case ActionType.ADD_TEMPLATE:
                        await AddTemplate(((Dispatchable<Template>)dispatchable).Payload);
                        OnChange?.Invoke();
                        break;
                    case ActionType.DELETE_TEMPLATE:
                        await DeleteTemplate(((Dispatchable<Template>)dispatchable).Payload);
                        OnChange?.Invoke();
                        break;
                    case ActionType.UPDATE_TEMPLATE:
                        await UpdateTemplate(((Dispatchable<Template>)dispatchable).Payload);
                        OnChange?.Invoke();
                        break;
                }
            };

        }

        private async Task AddTemplate(Template template)
        {
            template.UserId = this.userStore.Session.UserId;
            var success = await apiService.Add(template);
            if (success)
                Templates.Add(template);
        }

        private async Task DeleteTemplate(Template template)
        {
            var success = await apiService.Delete(template);
            if (success)
                Templates.Remove(template);
        }

        private async Task UpdateTemplate(Template template)
        {
            var sucess = await apiService.Update(template);
            if (!sucess)
                return;

            var idx = Templates.IndexOf(Templates.FirstOrDefault(x => x.Id == template.Id));
            if (idx == -1)
                return;
            
           Templates[idx] = template;
        }
        

        public async void Load()
        {
            Templates = await apiService.Get<Template>(userStore.Session?.UserId);
            OnChange?.Invoke();
        }
    }
}

