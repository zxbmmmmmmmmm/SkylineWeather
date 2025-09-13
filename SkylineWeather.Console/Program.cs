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
using SkylineWeather.SDK;
using System.Reflection;
using System.Text.Json;
using UnitsNet;

Console.WriteLine("Welcome to Skyline Weather Console!");

var builder = Host.CreateDefaultBuilder(args)
    .UseContentRoot(Directory.GetCurrentDirectory())
    .ConfigureAppConfiguration(config =>
    {
        config.AddJsonFile("appsettings.json", false, true);
    })
    .ConfigureServices((context, services) => {
        services.AddHostedService<WeatherService>();
        services.AddSingleton<ISettingsService, ConfigurationSettingsService>();
        services.AddOptions<CommonSettings>()
            .Bind(context.Configuration);
        services.AddSingleton(sp => sp.GetRequiredService<Microsoft.Extensions.Options.IOptions<CommonSettings>>().Value);
        var settingsForConfiguration = new CommonSettings();
        context.Configuration.Bind(settingsForConfiguration);
        services.AddProviders(settingsForConfiguration);
        services.AddDataAnalyzers();
    });


Program.AppHost = builder.Build();
await Program.AppHost.RunAsync().ConfigureAwait(false);


public static partial class Program
{
    public static IHost AppHost { get; set; } = null!;
}

internal static class BuilderExtensions
{
    public static IServiceCollection AddProviders(this IServiceCollection services, CommonSettings settings)
    {
        // 在这里定义所有可用的提供程序类型
        List<Type> builtinProviders = [typeof(QWeatherProvider.QWeatherProvider), typeof(OpenMeteoProvider.OpenMeteoProvider)];

        foreach (var providerType in builtinProviders)
        {
            var providerAttr = providerType.GetCustomAttribute<ProviderAttribute>();
            if (providerAttr is null) continue;

            // 1. 注册有键的接口实现
            RegisterKeyedProvider(providerType, services, providerAttr.Id);

            // 2. 自动注册该提供程序所需的配置
            RegisterProviderConfiguration(providerType, services, settings, providerAttr.Id);
        }

        // 3. 将无键接口映射到配置中指定的有键服务
        MapProvider<ICurrentWeatherProvider>(services, settings);
        MapProvider<IDailyWeatherProvider>(services, settings);
        MapProvider<IHourlyWeatherProvider>(services, settings);
        MapProvider<IAlertProvider>(services, settings);
        MapProvider<IGeolocationProvider>(services, settings);
        MapProvider<IAirQualityProvider>(services, settings);
        MapProvider<IPrecipitationProvider>(services, settings);

        return services;
    }

    private static void RegisterProviderConfiguration(Type providerType, IServiceCollection services, CommonSettings settings, string providerId)
    {
        var configAttr = providerType.GetCustomAttribute<ProviderConfigurationAttribute>();
        if (configAttr is null) return;

        var configType = configAttr.ConfigurationType;
        if (settings.ProviderConfigurations.TryGetValue(providerId, out var configValue))
        {
            var json = JsonSerializer.Serialize(configValue);
            var configObject = JsonSerializer.Deserialize(json, configType);

            if (configObject != null)
            {
                services.AddSingleton(configType, configObject);
            }
        }
    }

    private static void RegisterKeyedProvider(Type implementationType, IServiceCollection services, string key)
    {
        // 查找该实现所继承的所有 IProvider 接口
        var providerInterfaces = implementationType.GetInterfaces()
            .Where(i => i != typeof(IProvider) && typeof(IProvider).IsAssignableFrom(i));

        foreach (var interfaceType in providerInterfaces)
        {
            // 将每个接口都用同一个 key 指向该实现
            services.AddKeyedSingleton(interfaceType, key, implementationType);
        }
    }
    private static void MapProvider<T>(IServiceCollection services, CommonSettings settings) where T : class
    {
        var providerId = settings.ProviderMappings.GetValueOrDefault(typeof(T).Name);
        if (string.IsNullOrEmpty(providerId))
        {
            // 如果接口未在配置中映射，则不进行注册
            return;
        }

        // 将接口的默认实现（无key）指向有key的实现
        services.AddTransient<T>(sp =>
        sp.GetRequiredKeyedService<T>(providerId));
    }
    public static IServiceCollection AddDataAnalyzers(this IServiceCollection services)
    {
        return services.AddSingleton<ITrendAnalyzer<Temperature, TemperatureTrend>, SingleTemperatureTrendAnalyzer>()
            .AddSingleton<ITrendAnalyzer<(Temperature, Temperature), TemperatureTrend>, CompositeTemperatureTrendAnalyzer>()
            .AddSingleton<IAqiAnalyzer, ChinaAqiAnalyzer>()
            .AddSingleton<IAqiAnalyzer, UsaAqiAnalyzer>()
            .AddSingleton<IAqiAnalyzer, EuropeAqiAnalyzer>();
    }
}