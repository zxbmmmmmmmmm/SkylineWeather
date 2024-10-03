using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenMeteoProvider;
using QWeatherProvider;
using SkylineWeather.Abstractions.Provider.Interfaces;
using SkylineWeather.Abstractions.Services;
using SkylineWeather.Console;
using SkylineWeather.Console.Modules;
using System.Runtime.CompilerServices;

Console.WriteLine("Welcome to Skyline Weather Console!");

var builder = Host.CreateApplicationBuilder(args);
builder.Environment.ContentRootPath = AppDomain.CurrentDomain.BaseDirectory;
var qWeatherConfig = new QWeatherProviderConfig("b18d888d25b4437cbae4bbf36990092e")
{
    Domain = "devapi.qweather.com"
};
builder.Services.AddSingleton(qWeatherConfig);

builder.Services.AddHostedService<WeatherService>();

builder.Configuration.AddJsonFile("appsettings.json", false, true);
builder.Services.AddProviders();
AppHost = builder.Build();
await AppHost.RunAsync().ConfigureAwait(false);
public static partial class Program
{
    public static IHost AppHost { get; set; } = null!;
}
internal static class BuilderExtensions
{
    public static IServiceCollection AddProviders(this IServiceCollection services)
    {
        return services.AddSingleton<IDailyWeatherProvider, OpenMeteoProvider.OpenMeteoProvider>()
        .AddSingleton<IHourlyWeatherProvider, OpenMeteoProvider.OpenMeteoProvider>()
        .AddSingleton<ICurrentWeatherProvider, OpenMeteoProvider.OpenMeteoProvider>()
        .AddSingleton<IHourlyWeatherProvider, QWeatherProvider.QWeatherProvider>()
        .AddSingleton<IAlertProvider, QWeatherProvider.QWeatherProvider>()
        .AddSingleton<IGeolocationProvider, QWeatherProvider.QWeatherProvider>();
    }
}