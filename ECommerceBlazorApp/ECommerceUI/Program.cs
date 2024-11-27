using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using ECommerceUI;
using System.Net.Http;
using ECommerceUI.Services; // Ensure this using statement is present

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

// Register HttpClient
builder.Services.AddHttpClient("ECommerceAPI", client =>
{
    client.BaseAddress = new Uri("https://localhost:5001/"); // Ensure this matches your API's base URL
});

// Register a scoped HttpClient for injection
builder.Services.AddScoped(sp =>
    sp.GetRequiredService<IHttpClientFactory>().CreateClient("ECommerceAPI"));

// Register ECommerceService
builder.Services.AddScoped<ECommerceService>();

// Register CacheService as Singleton
builder.Services.AddSingleton<CacheService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
