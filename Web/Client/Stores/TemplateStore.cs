using Flux.Dispatchable;
using Flux.Dispatcher;
using Web.Client.Services.Api;
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
            var addedTemplate = await apiService.Add(template);
            if (addedTemplate == null)
                return;
            Templates.Add(addedTemplate);
        }

        private async Task DeleteTemplate(Template template)
        {
            var deletedTemplate = await apiService.Delete(template);
            if (deletedTemplate == null)
                return;
            Templates.Remove(deletedTemplate);
        }

        private async Task UpdateTemplate(Template template)
        {
            var updatedTemplate = await apiService.Update(template);
            if (updatedTemplate == null)
                return;

            var idx = Templates.IndexOf(Templates.FirstOrDefault(x => x.Id == updatedTemplate.Id));
            if (idx < 0)
                return;
            
           Templates[idx] = updatedTemplate;
        }
        

        public async void Load()
        {
            Templates = await apiService.Get<Template>();
            OnChange?.Invoke();
        }
    }
}

