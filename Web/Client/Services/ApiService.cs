using Data.Models;
using System.Net.Http.Json;
using Flux.Services;
using Microsoft.JSInterop;
using System.Text.Json;
using System.Text;
using Amazon.Runtime;
using static System.Net.WebRequestMethods;
using System.Net.Http.Headers;

namespace Web.Client.Services
{
    public class ApiService : IApiService
    {
        private HttpClient httpClient;
        private CookieService cookieService;
        public ApiService(HttpClient httpClient, CookieService cookieService)
        {
            this.httpClient = httpClient;
            this.cookieService = cookieService;
        }

        public async Task<Session?> LoginUser(Credentials credentials)
        {
            return await GetSession("/api/user/login", credentials);
        }

        public async Task<Session?> RegisterUser(Credentials credentials)
        {
            return await GetSession("/api/user/register", credentials);
        }

        private async Task<Session?> GetSession(string url, Credentials credentials)
        {
            var response = await httpClient.PostAsJsonAsync(url, credentials);
            Session? session = null;
            if (response.IsSuccessStatusCode)
            {
                
                session = await response.Content.ReadFromJsonAsync<Session?>();
                await SetSessionCookie(session);
            }
            return session;
        }

       

        private async Task SetSessionCookie(Session? session)
        {
            if (session == null)
                return;
            await cookieService.SetValueAsync("weeklySession", session.Token);
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", session.Token);
        }


        public async Task<IList<T>> Get<T>() where T : DataEntityBase
        {
            var response = await httpClient.GetAsync($"/api/{typeof(T).Name}");
            return await response.Content.ReadFromJsonAsync<IList<T>>();
        }

        public async Task<bool> Add<T>(T item) where T : DataEntityBase
        {
            var response = await httpClient.PostAsJsonAsync($"/api/{typeof(T).Name}", item);
            if (response.IsSuccessStatusCode)
                return true;
            return false;  
        }

        public async Task<bool> Delete<T>(T item) where T : DataEntityBase
        {
            var response = await httpClient.DeleteAsync($"/api/{typeof(T).Name}/{item.Id}");
            if (response.IsSuccessStatusCode)
                return true;
            return false;
        }

        public async Task<bool> Update<T>(T item) where T : DataEntityBase
        {
            var response = await httpClient.PutAsJsonAsync($"/api/{typeof(T).Name}", item);
            if (response.IsSuccessStatusCode)
                return true;
            return false;
        }



       

      
    }
}
