namespace Web.Client.Services.Misc
{
    public interface ICookieService
    {
        ValueTask DisposeAsync();
        Task<T?> GetValueAsync<T>(string key);
        Task SetValueAsync<T>(string key, T value);
    }
}