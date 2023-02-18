using Flux.Dispatcher;
using Flux.Stores;
using Data;
using WeeklyWeb;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Components.Authorization;
using WeeklyWeb.Authentication;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAuthenticationCore();
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.Configure<IConfiguration>(builder.Configuration);
builder.Services.AddScoped<IUserStore, UserStore>();
builder.Services.AddScoped<ITodoStore, TodoStore>();
builder.Services.AddScoped<ITemplateStore, TemplateStore>();
builder.Services.AddScoped<IDispatcher, Dispatcher>();
builder.Services.AddScoped<ProtectedSessionStorage>();
builder.Services.AddScoped<AuthenticationStateProvider, WeeklyAuthenticationStateProvider>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
