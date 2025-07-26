using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using SkylineWeather.Abstractions.Models;
using SkylineWeather.Abstractions.Models.AirQuality;
using SkylineWeather.Abstractions.Models.Alert;
using SkylineWeather.Abstractions.Models.Weather;
using SkylineWeather.Abstractions.Provider.Interfaces;
using SkylineWeather.DataAnalyzer.Analyzers;
using SkylineWeather.DataAnalyzer.Models;
using UnitsNet;

namespace SkylineWeather.ViewModels;

public partial class WeatherViewModel(
    Geolocation geolocation,
    IWeatherProvider weatherProvider,
    IAlertProvider alertProvider,
    IAirQualityProvider airQualityProvider,
    ITrendAnalyzer<(Temperature min,Temperature max), TemperatureTrend> temperatureTrendAnalyzer,
    ILogger logger) : ObservableObject
{
    public Geolocation Geolocation { get; init; } = geolocation;

    [ObservableProperty]
    public partial IReadOnlyList<DailyWeather>? Dailies { get; set; }

    [ObservableProperty]
    public partial IReadOnlyList<HourlyWeather>? Hourlies { get; set; }

    [ObservableProperty]
    public partial CurrentWeather? Current { get; set; }

    [ObservableProperty]
    public partial IReadOnlyList<Alert>? Alerts { get; set; }   

    [ObservableProperty]
    public partial AirQuality? AirQuality { get; set; }

    public TemperatureTrend? DailyTemperatureTrend =>
        Dailies is null ? null : temperatureTrendAnalyzer.GetTrend(Dailies.Select(p => (p.LowTemperature, p.HighTemperature)));

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(Loaded))]
    public partial DateTimeOffset? RefreshedTime { get; set; }

    public bool Loaded => RefreshedTime is not null;

    [RelayCommand]
    public async Task RefreshAsync()
    {
        await Task.WhenAll(
            GetDailiesAsync(),
            GetHourliesAsync(),
            GetCurrentAsync(),
            GetAlertsAsync(),
            GetAirQualityAsync());
        RefreshedTime = DateTimeOffset.Now;

    }

    [RelayCommand]
    public async Task GetDailiesAsync()
    {
        var result = await weatherProvider.GetDailyWeatherAsync(Geolocation.Location);
        result.IfSucc(f => 
        {
            Dailies = f;
        });
        result.IfFail(f =>
        {
            logger.LogError(f, "Failed to get daily weather for {Location}", Geolocation.Location);
        });
    }

    [RelayCommand]
    public async Task GetHourliesAsync()
    {
        var result = await weatherProvider.GetHourlyWeatherAsync(Geolocation.Location);
        result.IfSucc(f =>
        {
            Hourlies = f;
        });
        result.IfFail(f =>
        {
            logger.LogError(f, "Failed to get hourly weather for {Location}", Geolocation.Location);
        });
    }

    [RelayCommand]
    public async Task GetCurrentAsync()
    {
        var result = await weatherProvider.GetCurrentWeatherAsync(Geolocation.Location);
        result.IfSucc(f =>
        {
            Current = f;
        });
        result.IfFail(f =>
        {
            logger.LogError(f, "Failed to get current weather for {Location}", Geolocation.Location);
        });
    }

    public async Task GetAlertsAsync()
    {
        var result = await alertProvider.GetAlertsAsync(Geolocation.Location);
        result.IfSucc(f =>
        {
            Alerts = f;
        });
        result.IfFail(f =>
        {
            logger.LogError(f, "Failed to get alerts for {Location}", Geolocation.Location);
        });
    }

    public async Task GetAirQualityAsync()
    {
        var result = await airQualityProvider.GetCurrentAirQualityAsync(Geolocation.Location);
        result.IfSucc(f =>
        {
            AirQuality = f;
        });
        result.IfFail(f =>
        {
            logger.LogError(f, "Failed to get air quality for {Location}", Geolocation.Location);
        });
    }
}