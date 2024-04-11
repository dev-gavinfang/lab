using AsyncEnumerableSample;
using AsyncEnumerableSample.Client.Contracts;
using Microsoft.FluentUI.AspNetCore.Components;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveWebAssemblyComponents();
builder.Services.AddFluentUIComponents();
builder.Services.AddHttpClient();

var app = builder.Build();

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
    .AddAdditionalAssemblies(typeof(AsyncEnumerableSample.Client._Imports).Assembly);

app.MapGet("/api/async-enumerable", GenerateData);

app.Run();

static async IAsyncEnumerable<SampleDto> GenerateData()
{
    for (int i = 0; i < 50; i++)
    {
        var obj = new SampleDto { Id = i, Name = $"Item {i}" };
        yield return obj;
        await Task.Delay(200);
    }
}
