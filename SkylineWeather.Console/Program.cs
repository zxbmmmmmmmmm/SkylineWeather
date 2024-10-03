using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenMeteoProvider;
using QWeatherProvider;
using SkylineWeather.Abstractions.Provider.Interfaces;
using SkylineWeather.Abstractions.Services;
using SkylineWeather.Console;
using SkylineWeather.Console.Modules;

Console.WriteLine("Welcome to Skyline Weather Console!");

var builder = Host.CreateApplicationBuilder(args);
builder.Environment.ContentRootPath = AppDomain.CurrentDomain.BaseDirectory;
var qWeatherConfig = new QWeatherProviderConfig("b18d888d25b4437cbae4bbf36990092e")
{
    Domain = "devapi.qweather.com"
};
builder.Services.AddSingleton(qWeatherConfig);

builder.Services.AddHostedService<WeatherService>();
builder.Services.AddSingleton<IDailyWeatherProvider, OpenMeteoProvider.OpenMeteoProvider>();
builder.Services.AddSingleton<IHourlyWeatherProvider, OpenMeteoProvider.OpenMeteoProvider>();
builder.Services.AddSingleton<ICurrentWeatherProvider, OpenMeteoProvider.OpenMeteoProvider>();
builder.Services.AddSingleton<IHourlyWeatherProvider, QWeatherProvider.QWeatherProvider>();
builder.Services.AddSingleton<IAlertProvider, QWeatherProvider.QWeatherProvider>();
builder.Services.AddSingleton<IGeolocationProvider, QWeatherProvider.QWeatherProvider>();
builder.Configuration.AddJsonFile("appsettings.json", false, true);

AppHost = builder.Build();
await AppHost.RunAsync().ConfigureAwait(false);
public static partial class Program
{
    public static IHost AppHost { get; set; } = null!;
}
