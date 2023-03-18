using Flux.Dispatchable;
using Flux.Dispatcher;
using Web.Client.Services.Http;
using Web.Models;


namespace Web.Client.Stores
{
    public class TemplateStore : ITemplateStore
    {
        private readonly IApiService apiService;
        public IList<Template> Templates { get; private set; }
        public Action? OnChange { get; set; }

        public TemplateStore(IDispatcher<ActionType> dispatcher, IApiService apiService)
        {
            this.apiService = apiService;

            Templates = new List<Template>();

            Load();

            dispatcher.Action += async dispatchable =>
            {
                switch (dispatchable.ActionType)
                {
                    case ActionType.ADD_TEMPLATE:
                        await AddTemplate(((Dispatchable<ActionType, Template>)dispatchable).Payload);
                        OnChange?.Invoke();
                        break;
                    case ActionType.DELETE_TEMPLATE:
                        await DeleteTemplate(((Dispatchable<ActionType, Template>)dispatchable).Payload);
                        OnChange?.Invoke();
                        break;
                    case ActionType.UPDATE_TEMPLATE:
                        await UpdateTemplate(((Dispatchable<ActionType, Template>)dispatchable).Payload);
                        OnChange?.Invoke();
                        break;
                }
            };

        }

        private async Task AddTemplate(Template template)
        {
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
            Templates = await apiService.Get<Template>();
            OnChange?.Invoke();
        }
    }
}

