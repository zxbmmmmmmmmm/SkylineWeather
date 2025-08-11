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

var builder = Host.CreateApplicationBuilder(args);
builder.Environment.ContentRootPath = AppDomain.CurrentDomain.BaseDirectory;

builder.Services.AddHostedService<WeatherService>();

builder.Configuration.AddJsonFile("appsettings.json", false, true);
builder.Services.AddSingleton<ISettingsService, ConfigurationSettingsService>();

builder.Services.AddSingleton<CommonSettings>();

builder.Services.AddProviders(builder.Configuration);
builder.Services.AddDataAnalyzers();
Program.AppHost = builder.Build();
await Program.AppHost.RunAsync().ConfigureAwait(false);


public static partial class Program
{
    public static IHost AppHost { get; set; } = null!;
}

internal static class BuilderExtensions
{
    public static IServiceCollection AddProviders(this IServiceCollection services, IConfiguration config)
    {
        var settings = config.GetSection("CommonSettings").Get<CommonSettings>() ?? new CommonSettings(new ConfigurationSettingsService(config));
        var mappings = settings.ProviderMappings;
        List<Type> builtinProviders = [typeof(QWeatherProvider.QWeatherProvider), typeof(OpenMeteoProvider.OpenMeteoProvider) ];

        // 注册内置的天气提供程序
        foreach (var providerType in builtinProviders)
        {
            if (providerType.GetCustomAttributes(typeof(ProviderAttribute), false)
                    .FirstOrDefault() is not ProviderAttribute providerAttribute) continue;
            RegisterKeyedProvider(providerType, services, providerAttribute.Id);
            RegisterProviderConfiguration(providerType, services, settings, providerAttribute.Id);

        }

        MapProvider<ICurrentWeatherProvider>(services, settings);
        MapProvider<IDailyWeatherProvider>(services, settings);
        MapProvider<IHourlyWeatherProvider>(services, settings);
        MapProvider<IAlertProvider>(services, settings);
        MapProvider<IGeolocationProvider>(services, settings);
        MapProvider<IAirQualityProvider>(services, settings);

        foreach(var providerConfig in settings.ProviderConfigurations)
        {
            services.AddKeyedSingleton(providerConfig.Key, (object)providerConfig.Value);
        }

        return services;
    }

    private static void RegisterProviderConfiguration(Type providerType, IServiceCollection services, CommonSettings settings, string providerId)
    {
        var configAttr = providerType.GetCustomAttribute<ProviderConfigurationAttribute>();
        if (configAttr is null) return;

        var configType = configAttr.ConfigurationType;
        var providerConfigData = settings.ProviderConfigurations
            .FirstOrDefault(p => p.Key.Equals(providerId, StringComparison.OrdinalIgnoreCase));

        var configObject = JsonSerializer.Deserialize(providerConfigData.Value.ToString(), configType);
        if (configObject != null)
        {
            // 将具体类型的配置对象注册到DI容器
            services.AddSingleton(configType, configObject);
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
            // DI 容器会自动处理 TImplementation 的单例生命周期
            services.AddKeyedSingleton(interfaceType, key, implementationType);
        }
    }
    private static void MapProvider<T>(IServiceCollection services, CommonSettings settings) where T : class
    {
        var providerId = settings.ProviderMappings.GetValueOrDefault(typeof(T).Name);
        if (string.IsNullOrEmpty(providerId))
        {
            throw new InvalidOperationException($"Provider mapping for interface {typeof(T).Name} not found in settings.");
        }

        // 将接口的默认实现（无key）指向有key的实现
        services.AddTransient<T>(sp => sp.GetRequiredKeyedService<T>(providerId));
    }
    public static IServiceCollection AddDataAnalyzers(this IServiceCollection services)
    {
        return services.AddSingleton<ITrendAnalyzer<Temperature, TemperatureTrend>, SingleTemperatureTrendAnalyzer>()
            .AddSingleton<ITrendAnalyzer<(Temperature, Temperature), TemperatureTrend>, CompositeTemperatureTrendAnalyzer>();
    }
}