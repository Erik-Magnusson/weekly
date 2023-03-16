using Data.Models;
using Microsoft.JSInterop;
using System.Text.Json;
using System.Text;

namespace Web.Client.Services
{
    public class CookieService
    {
        private Lazy<IJSObjectReference> _accessorJsRef = new();
        private readonly IJSRuntime _jsRuntime;

        public CookieService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        private async Task WaitForReference()
        {
            if (_accessorJsRef.IsValueCreated is false)
            {
                _accessorJsRef = new(await _jsRuntime.InvokeAsync<IJSObjectReference>("import", "/js/CookieService.js"));
            }
        }

        public async ValueTask DisposeAsync()
        {
            if (_accessorJsRef.IsValueCreated)
            {
                await _accessorJsRef.Value.DisposeAsync();
            }
        }

        public async Task<string?> GetValueAsync(string key)
        {
            await WaitForReference();
            var result = await _accessorJsRef.Value.InvokeAsync<string>("get", key);
            if (string.IsNullOrEmpty(result))
                return null;

            try
            {
                var cookies = result.Split('=');
                var cookie = cookies[Array.IndexOf(cookies, key) + 1];
                Console.WriteLine($"Cookie: {cookie}");
                //var valueBytes = Convert.FromBase64String(cookie);
                //var valueJson = Encoding.UTF8.GetString(valueBytes);
                //var value = JsonSerializer.Deserialize<T>(cookie);
                
                return cookie;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
            
        }

        public async Task SetValueAsync<T>(string key, T value)
        {
            //var valueJson = JsonSerializer.Serialize(value);
            //var valueBytes = Encoding.UTF8.GetBytes(valueJson);
            //var valueString = Convert.ToBase64String(valueBytes);
            await WaitForReference();
            await _accessorJsRef.Value.InvokeVoidAsync("set", key, value);
        }
    }
}
