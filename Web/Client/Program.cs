using Flux.Dispatcher;
using Flux.Stores;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using System.Net.Http;
using Web.Client;
using Web.Client.Auth;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.Services.AddSingleton<IDispatcher, Dispatcher>();
builder.Services.AddSingleton<IUserStore, UserStore>();
builder.Services.AddSingleton<ITodoStore, TodoStore>();
builder.Services.AddSingleton<ITemplateStore, TemplateStore>();

builder.Services.AddHttpClient("Web.ServerAPI", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress));

// Supply HttpClient instances that include access tokens when making requests to the server project
builder.Services.AddSingleton(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("Web.ServerAPI"));
builder.Services.AddScoped<AuthenticationStateProvider, ClientAuthenticationStateProvider>();
builder.Services.AddAuthorizationCore();
await builder.Build().RunAsync();
