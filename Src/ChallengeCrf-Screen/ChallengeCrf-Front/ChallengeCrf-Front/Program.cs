using ChallengeCrf_Front.Clients;
using ChallengeCrf_Front.ViewModels;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
//builder.Services.AddSingleton<WeatherForecastService>();


var apiUrl = builder.Configuration["ApiUrl"] ?? throw new Exception("ApiUrl is not set");

builder.Services.AddHttpClient<CashFlowClient>(client => client.BaseAddress = new Uri(apiUrl));
builder.Services.AddHttpClient<DailyConsolidatedClient>(client => client.BaseAddress = new Uri(apiUrl));
//builder.Services.AddHttpClient<CustomAuthenticationStateProvider>(client => client.BaseAddress = new Uri(apiUrl));
builder.Services.AddHttpClient<LoginViewModel>(client => client.BaseAddress = new Uri(apiUrl));

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
