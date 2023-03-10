using Data.Models;
using Flux.Stores;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace Flux.Services
{
    public class ApiService : IApiService
    {
        private HttpClient httpClient;
        public ApiService(IConfiguration configuration)
        { 
            httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(configuration["ApiBaseUrl"]);
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
            var response = await httpClient.PostAsJsonAsync($"/api/user/reg{url}", credentials);
            Session? session = null;
            if (response.IsSuccessStatusCode)
                session = await response.Content.ReadFromJsonAsync<Session?>();
            return session;
        }

        public async Task<IList<T>> Get<T>(Guid? userId) where T : DataEntityBase
        {
            var response = await httpClient.GetAsync($"/api/{typeof(T).Name}/{userId}");
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
