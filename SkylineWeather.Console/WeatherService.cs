using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SkylineWeather.Abstractions.Provider.Interfaces;
using SkylineWeather.Console.Modules;
using SkylineWeather.DataAnalyzer.Models;
using Spectre.Console;
using System.Threading;
using UnitsNet;

namespace SkylineWeather.Console;

public class WeatherService : IHostedService
{
    private CancellationToken _cancellationToken;
    private readonly Dictionary<FeatureType, IFeatureModule> _features = new();

    private IFeatureModule AskFeature()
    {
        var features = Enum.GetValues<FeatureType>();
        var featureType = AnsiConsole.Prompt(
            new SelectionPrompt<FeatureType>()
                .Title("请选择功能")
                .PageSize(10)
                .MoreChoicesText("更多")
                .AddChoices(features)
                .UseConverter(FeatureToString));

        return GetOrCreateFeature(featureType);

        static string FeatureToString(FeatureType feature)
        {
            return feature switch
            {
                FeatureType.Current => "当前天气",
                FeatureType.Daily => "每日预报",
                FeatureType.Hourly => "每小时预报",
                FeatureType.AirQuality => "空气质量",
                FeatureType.Geocoding => "地理位置",
                FeatureType.Alerts => "预警",
                FeatureType.Precipitation => "降水",
                _ => throw new NotSupportedException(),
            };
        }
    }

    private async Task InitializeAsync()
    {
        var feature = AskFeature();
        AnsiConsole.Clear();
        await feature.RunAsync().ConfigureAwait(false);
    }
    private IFeatureModule GetOrCreateFeature(FeatureType type)
    {
        if (!_features.TryGetValue(type, out var feature))
        {
            feature = type switch
            {
                FeatureType.Daily => new DailyWeatherModule(
                    Program.AppHost.Services.GetService<IDailyWeatherProvider>()!,
                    Program.AppHost.Services.GetService<ITrendAnalyzer<(Temperature, Temperature), TemperatureTrend>>()!,
                    BackToFeatureSelectionAsync,
                    _cancellationToken),
                FeatureType.Current => new CurrentWeatherModule(
                    Program.AppHost.Services.GetService<ICurrentWeatherProvider>()!,
                    BackToFeatureSelectionAsync,
                    _cancellationToken),
                FeatureType.Hourly => new HourlyWeatherModule(
                    Program.AppHost.Services.GetService<IHourlyWeatherProvider>()!,
                    BackToFeatureSelectionAsync,
                    _cancellationToken),
                FeatureType.Alerts => new AlertModule(
                    Program.AppHost.Services.GetService<IAlertProvider>()!,
                    BackToFeatureSelectionAsync,
                    _cancellationToken),
                FeatureType.Geocoding => new GeolocationModule(
                    Program.AppHost.Services.GetService<IGeolocationProvider>()!,
                    BackToFeatureSelectionAsync,
                    _cancellationToken),
                FeatureType.AirQuality => new AirQualityModule(
                    Program.AppHost.Services.GetService<IAirQualityProvider>()!,
                    BackToFeatureSelectionAsync,
                    _cancellationToken),
                FeatureType.Precipitation => new PrecipitationModule(
                    Program.AppHost.Services.GetService<IPrecipitationProvider>()!,
                    BackToFeatureSelectionAsync,
                    _cancellationToken),
                _ => throw new NotSupportedException(),
            };

            _features[type] = feature;
        }

        return feature;
    }

    private async Task BackToFeatureSelectionAsync(string? lastMessage)
    {
        AnsiConsole.Clear();
        if (!string.IsNullOrEmpty(lastMessage))
        {
            AnsiConsole.MarkupLine(lastMessage);
        }

        await InitializeAsync().ConfigureAwait(false);
    }
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _cancellationToken = cancellationToken;
        await InitializeAsync().ConfigureAwait(false);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
internal enum FeatureType
{
    Current,
    Daily,
    Hourly,
    AirQuality,
    Alerts,
    Precipitation,
    Geocoding,
}