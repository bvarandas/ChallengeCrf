using CashFlow.Frontend.Clients;
using CashFlow.Frontend.Components;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services
    .AddRazorComponents()
    .AddInteractiveServerComponents();

var apiUrl = builder.Configuration["CashFlowApiUrl"] ?? throw new Exception("CashFlowApiUrl is not set");

builder.Services.AddHttpClient<CashFlowClient>(client=>client.BaseAddress = new Uri(apiUrl));
builder.Services.AddHttpClient<DailyConsolidatedClient>(client=>client.BaseAddress = new Uri(apiUrl));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();


app.Run();
