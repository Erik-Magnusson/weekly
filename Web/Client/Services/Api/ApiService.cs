using Web.Models;
using System.Net.Http.Json;
using Flux.Dispatchable;
using Flux.Dispatcher;
using Web.Client.Services.Misc;

namespace Web.Client.Services.Api
{
    public class ApiService : IApiService
    {
        private HttpClient httpClient;
        public ApiService(HttpClient httpClient, IDispatcher<ActionType> dispatcher, ICookieService cookieService)
        {
            this.httpClient = httpClient;
            dispatcher.Action += async dispatchable =>
            {
                switch (dispatchable.ActionType)
                {
                    case ActionType.LOGIN_USER:
                        var token = await LoginUser(((Dispatchable<ActionType, Credentials>)dispatchable).Payload);
                        await cookieService.SetValueAsync("weeklyAuth", token);
                        dispatcher.Dispatch(new Dispatchable<ActionType, string?>(ActionType.AUTHENTICATE_USER, token));
                        break;
                    case ActionType.LOGOUT_USER:
                        await cookieService.SetValueAsync("weeklyAuth", string.Empty);
                        break;
                    case ActionType.REGISTER_USER:
                        token = await RegisterUser(((Dispatchable<ActionType, Credentials>)dispatchable).Payload);
                        await cookieService.SetValueAsync("weeklyAuth", token);
                        dispatcher.Dispatch(new Dispatchable<ActionType, string?>(ActionType.AUTHENTICATE_USER, token));
                        break;
                }
            };
        }

        public async Task<string?> LoginUser(Credentials credentials)
        {
            var response = await httpClient.PostAsJsonAsync("api/user/login", credentials);
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string?> RegisterUser(Credentials credentials)
        {
            var response = await httpClient.PostAsJsonAsync("api/user/register", credentials);
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<IList<T>> Get<T>() where T : ApiEntityBase
        {
            var url = $"api/{typeof(T).Name}".ToLower();
            var response = await httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadFromJsonAsync<IList<T>>();
            return new List<T>();
        }
        public async Task<T?> Add<T>(T item) where T : ApiEntityBase
        {
            var url = $"api/{typeof(T).Name}".ToLower();
            var response = await httpClient.PostAsJsonAsync(url, item);
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadFromJsonAsync<T>();
            return null;
        }

        public async Task<T?> Delete<T>(T item) where T : ApiEntityBase
        {
            var url = $"api/{typeof(T).Name}/{item.Id}".ToLower();
            var response = await httpClient.DeleteAsync(url);
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadFromJsonAsync<T>();
            return null;
        }

        public async Task<T?> Update<T>(T item) where T : ApiEntityBase
        {
            var url = $"api/{typeof(T).Name}".ToLower();
            var response = await httpClient.PutAsJsonAsync(url, item);
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadFromJsonAsync<T>();
            return null;
        }

    }
}
