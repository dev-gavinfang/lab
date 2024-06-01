using HelloAspire.Web.Components;
using Microsoft.Extensions.Hosting;
using Microsoft.FluentUI.AspNetCore.Components;
using Orleans.Runtime;
using OrleansContracts;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.AddKeyedRedisClient("gavin-redis");

builder.UseOrleansClient();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddFluentUIComponents();
builder.Services.AddHttpClient();

var app = builder.Build();

//app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(HelloAspire.Web.Client._Imports).Assembly);

app.MapGet("/api/counter", async (IClusterClient client) =>
{
    var grain = client.GetGrain<ICounterGrain>("dev");
    return await grain.Get();
});
app.MapPost("/api/counter", async (IClusterClient client) =>
{
    var grain = client.GetGrain<ICounterGrain>("dev");
    await grain.Increment();

    return await grain.Get();
});

app.Run();

class CounterService
{
    public int Counter { get; private set; }

    public void Increment()
    {
        Counter++;
    }
}
