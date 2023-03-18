using Flux.Dispatchable;
using Flux.Dispatcher;
using Web.Client.Stores;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Web.Client;
using Web.Client.Services.Auth;
using Web.Client.Services.Http;
using Web.Client.Services.Misc;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.Services.AddSingleton<IDispatcher<ActionType>, Dispatcher<ActionType>>();
builder.Services.AddSingleton<IApiService, ApiService>();
builder.Services.AddSingleton<IUserStore, UserStore>();
builder.Services.AddSingleton<ITodoStore, TodoStore>();
builder.Services.AddSingleton<ITemplateStore, TemplateStore>();
builder.Services.AddSingleton<CookieService>();
builder.Services.AddSingleton<AuthorizationHandler>();
builder.Services.AddHttpClient("Web.ServerAPI", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
    .AddHttpMessageHandler<AuthorizationHandler>();
builder.Services.AddSingleton(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("Web.ServerAPI"));
builder.Services.AddScoped<AuthenticationStateProvider, ClientAuthenticationStateProvider>();
builder.Services.AddAuthorizationCore();
await builder.Build().RunAsync();
