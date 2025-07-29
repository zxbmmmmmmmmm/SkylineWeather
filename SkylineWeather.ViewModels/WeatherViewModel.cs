using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LanguageExt.Common;
using Microsoft.Extensions.Logging;
using SkylineWeather.Abstractions.Models;
using SkylineWeather.Abstractions.Models.AirQuality;
using SkylineWeather.Abstractions.Models.Alert;
using SkylineWeather.Abstractions.Models.Weather;
using SkylineWeather.Abstractions.Provider.Interfaces;
using SkylineWeather.Abstractions.Services;
using SkylineWeather.DataAnalyzer.Models;
using UnitsNet;

namespace SkylineWeather.ViewModels;

public partial class WeatherViewModel : ObservableObject
{
    public WeatherViewModel(Geolocation geolocation,
        IWeatherProvider weatherProvider,
        IAlertProvider alertProvider,
        IAirQualityProvider airQualityProvider, 
        ITrendAnalyzer<(Temperature min, Temperature max), TemperatureTrend> temperatureTrendAnalyzer, 
        ICacheService cacheService,
        ILogger logger)
    {
        _weatherProvider = weatherProvider;
        _alertProvider = alertProvider;
        _airQualityProvider = airQualityProvider;
        _cacheService = cacheService;
        _temperatureTrendAnalyzer = temperatureTrendAnalyzer;
        _logger = logger;
        Geolocation = geolocation;

        _refreshJobs = new Dictionary<string, IRefreshJob>
        {
            [nameof(Dailies)] = new RefreshJob<IReadOnlyList<DailyWeather>>(
                nameof(Dailies),
                TimeSpan.FromHours(6), 
                () => _weatherProvider.GetDailyWeatherAsync(Geolocation.Location),
                value => Dailies = value),

            [nameof(Hourlies)] = new RefreshJob<IReadOnlyList<HourlyWeather>>(
                nameof(Hourlies),
                TimeSpan.FromHours(1), 
                () => _weatherProvider.GetHourlyWeatherAsync(Geolocation.Location), 
                value => Hourlies = value),

            [nameof(Current)] = new RefreshJob<CurrentWeather>(
                nameof(Current),
                TimeSpan.FromMinutes(1),
                () => _weatherProvider.GetCurrentWeatherAsync(Geolocation.Location),
                value => Current = value),

            [nameof(Alerts)] = new RefreshJob<IReadOnlyList<Alert>>(
                nameof(Alerts), 
                TimeSpan.FromMinutes(15),
                () => _alertProvider.GetAlertsAsync(Geolocation.Location), 
                value => Alerts = value),

            [nameof(AirQuality)] = new RefreshJob<AirQuality>(
                nameof(AirQuality),
                TimeSpan.FromHours(1),
                () => _airQualityProvider.GetCurrentAirQualityAsync(Geolocation.Location),
                value => AirQuality = value),
        };

    }

    public Geolocation Geolocation { get; init; }

    private readonly IWeatherProvider _weatherProvider;
    private readonly IAlertProvider _alertProvider;
    private readonly IAirQualityProvider _airQualityProvider;
    private readonly ITrendAnalyzer<(Temperature min, Temperature max), TemperatureTrend> _temperatureTrendAnalyzer;
    private readonly ICacheService _cacheService;
    private readonly ILogger _logger;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(DailyTemperatureTrend))]
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
        Dailies is null ? null : _temperatureTrendAnalyzer.GetTrend(Dailies.Select(p => (p.LowTemperature, p.HighTemperature)));

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(Loaded))]
    public partial DateTimeOffset? RefreshedTime { get; set; }

    public bool Loaded => RefreshedTime is not null;


    private readonly Dictionary<string, IRefreshJob> _refreshJobs;

    private class RefreshJob<T>(
        string dataType,
        TimeSpan expiration,
        Func<Task<Result<T>>> dataProvider,
        Action<T> successAction)
        : IRefreshJob
    {
        public string DataType { get; } = dataType;

        public TimeSpan Expiration { get; } = expiration;

        public DateTimeOffset? LastRefreshTime { get; set; }

        public async Task RefreshAsync(Geolocation geolocation, ICacheService cacheService, ILogger logger)
        {
            var cacheKey = $"{geolocation.Name}_{DataType}";
            var result = await cacheService.GetOrCreateAsync(cacheKey, dataProvider, Expiration);

            result.IfSucc(value =>
            {
                successAction(value);
                LastRefreshTime = DateTimeOffset.UtcNow;
            });

            result.IfFail(error => LogDataFetchError(logger, error, DataType, geolocation.Location));
        }
    }

    // 基接口，用于在列表中存储不同类型的刷新任务
    private interface IRefreshJob
    {
        string DataType { get; }
        TimeSpan Expiration { get; }
        DateTimeOffset? LastRefreshTime { get; set; }
        Task RefreshAsync(Geolocation geolocation, ICacheService cacheService, ILogger logger);
    }


    [LoggerMessage(
        EventId = 1,
        Level = LogLevel.Error,
        Message = "Failed to get {DataType} for {Location}")]
    static partial void LogDataFetchError(ILogger logger, Exception error, string dataType, Location location);

    [RelayCommand]
    public async Task RefreshAllAsync()
    {
        var invalidationTasks = _refreshJobs.Keys.Select(dataType =>
            _cacheService.InvalidateAsync($"{Geolocation.Name}_{dataType}"));
        
        await Task.WhenAll(invalidationTasks);

        await Task.WhenAll(_refreshJobs.Values.Select(job => job.RefreshAsync(Geolocation, _cacheService, _logger)));
        RefreshedTime = DateTimeOffset.UtcNow;
    }

    [RelayCommand]
    public async Task RefreshIfNeededAsync()
    {
        var now = DateTimeOffset.UtcNow;
        var tasksToRun = new List<Task>();

        foreach (var job in _refreshJobs.Values)
        {
            if (job.LastRefreshTime is null || (job.LastRefreshTime.Value + job.Expiration < now))
            {
                tasksToRun.Add(job.RefreshAsync(Geolocation, _cacheService, _logger));
            }
        }

        if (tasksToRun.Count > 0)
        {
            await Task.WhenAll(tasksToRun);
        }
    }

    [RelayCommand]
    public Task GetDailiesAsync() => _refreshJobs[nameof(Dailies)].RefreshAsync(Geolocation, _cacheService, _logger);

    [RelayCommand]
    public Task GetHourliesAsync() => _refreshJobs[nameof(Hourlies)].RefreshAsync(Geolocation, _cacheService, _logger);

    [RelayCommand]
    public Task GetCurrentAsync() => _refreshJobs[nameof(Current)].RefreshAsync(Geolocation, _cacheService, _logger);

    [RelayCommand]
    public Task GetAlertsAsync() => _refreshJobs[nameof(Alerts)].RefreshAsync(Geolocation, _cacheService, _logger);

    [RelayCommand]
    public Task GetAirQualityAsync() => _refreshJobs[nameof(AirQuality)].RefreshAsync(Geolocation, _cacheService, _logger);
}