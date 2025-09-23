using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenMeteoProvider;
using QWeatherProvider;
using SkylineWeather.Abstractions.Provider;
using SkylineWeather.Abstractions.Provider.Interfaces;
using SkylineWeather.Abstractions.Services;
using SkylineWeather.Console;
using SkylineWeather.DataAnalyzer.Analyzers;
using SkylineWeather.DataAnalyzer.Models;
using SkylineWeather.SDK.Services;
using SkylineWeather.SDK;
using System.Reflection;
using System.Text.Json;
using UnitsNet;
using SkylineWeather.SDK.Utilities;

Console.WriteLine("Welcome to Skyline Weather Console!");

var builder = Host.CreateDefaultBuilder(args)
    .UseContentRoot(Directory.GetCurrentDirectory())
    .ConfigureAppConfiguration(config =>
    {
        config.AddJsonFile("appsettings.json", false, true);
    })
    .ConfigureServices((context, services) => {
        services.AddHostedService<WeatherService>();
        services.AddOptions<CommonSettings>()
            .Bind(context.Configuration);
        services.AddSingleton(sp => sp.GetRequiredService<Microsoft.Extensions.Options.IOptions<CommonSettings>>().Value);
        var settingsForConfiguration = new CommonSettings();
        context.Configuration.Bind(settingsForConfiguration);
        services.AddProviders(settingsForConfiguration, typeof(OpenMeteoProvider.OpenMeteoProvider), typeof(QWeatherProvider.QWeatherProvider));
        services.AddSingleton<ITrendAnalyzer<Temperature, TemperatureTrend>, SingleTemperatureTrendAnalyzer>()
            .AddSingleton<ITrendAnalyzer<(Temperature, Temperature), TemperatureTrend>, CompositeTemperatureTrendAnalyzer>()
            .AddSingleton<IAqiAnalyzer, ChinaAqiAnalyzer>()
            .AddSingleton<IAqiAnalyzer, UsaAqiAnalyzer>()
            .AddSingleton<IAqiAnalyzer, EuropeAqiAnalyzer>();
    });


Program.AppHost = builder.Build();
await Program.AppHost.RunAsync().ConfigureAwait(false);


public static partial class Program
{
    public static IHost AppHost { get; set; } = null!;
}

