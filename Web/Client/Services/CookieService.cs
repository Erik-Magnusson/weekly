using Microsoft.JSInterop;
using System.Text.Json;


namespace Web.Client.Services
{
    public class CookieService
    {
        private Lazy<IJSObjectReference> accessorJsRef = new();
        private readonly IJSRuntime js;

        public CookieService(IJSRuntime js)
        {
            this.js = js;
        }

        private async Task WaitForReference()
        {
            if (accessorJsRef.IsValueCreated is false)
            {
                accessorJsRef = new(await js.InvokeAsync<IJSObjectReference>("import", "/js/CookieService.js"));
            }
        }

        public async ValueTask DisposeAsync()
        {
            if (accessorJsRef.IsValueCreated)
            {
                await accessorJsRef.Value.DisposeAsync();
            }
        }

        public async Task<T?> GetValueAsync<T>(string key)
        {
            await WaitForReference();
            var result = await accessorJsRef.Value.InvokeAsync<string>("get", key);
            if (string.IsNullOrEmpty(result))
                return default;

            try
            {
                var cookies = result.Split('=');
                var cookie = cookies[Array.IndexOf(cookies, key) + 1];
                var value = JsonSerializer.Deserialize<T>(cookie);
                
                return value;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return default;
            }
            
        }

        public async Task SetValueAsync<T>(string key, T value)
        {
            var valueJson = JsonSerializer.Serialize(value);

            await WaitForReference();
            await accessorJsRef.Value.InvokeVoidAsync("set", key, valueJson);
        }
    }
}
