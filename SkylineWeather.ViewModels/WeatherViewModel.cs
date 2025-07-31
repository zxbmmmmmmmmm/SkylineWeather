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
        //TODO: ICacheService可更改为获取当前容器，即当前位置下使用单独一个文件进行缓存
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
                cancellationToken => _weatherProvider.GetDailyWeatherAsync(Geolocation.Location, cancellationToken),
                value => Dailies = value),

            [nameof(Hourlies)] = new RefreshJob<IReadOnlyList<HourlyWeather>>(
                nameof(Hourlies),
                TimeSpan.FromHours(1),
                cancellationToken => _weatherProvider.GetHourlyWeatherAsync(Geolocation.Location, cancellationToken),
                value => Hourlies = value),

            [nameof(Current)] = new RefreshJob<CurrentWeather>(
                nameof(Current),
                TimeSpan.FromMinutes(1),
                cancellationToken => _weatherProvider.GetCurrentWeatherAsync(Geolocation.Location, cancellationToken),
                value => Current = value),

            [nameof(Alerts)] = new RefreshJob<IReadOnlyList<Alert>>(
                nameof(Alerts),
                TimeSpan.FromMinutes(15),
                cancellationToken => _alertProvider.GetAlertsAsync(Geolocation.Location, cancellationToken),
                value => Alerts = value),

            [nameof(AirQuality)] = new RefreshJob<AirQuality>(
                nameof(AirQuality),
                TimeSpan.FromHours(1),
                cancellationToken => _airQualityProvider.GetCurrentAirQualityAsync(Geolocation.Location, cancellationToken),
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
        Func<CancellationToken, Task<Result<T>>> dataProvider,
        Action<T> successAction)
        : IRefreshJob
    {
        public string DataType { get; } = dataType;

        public TimeSpan Expiration { get; } = expiration;

        public DateTimeOffset? LastRefreshTime { get; set; }

        public async Task RefreshAsync(Geolocation geolocation, ICacheService cacheService, ILogger logger, CancellationToken cancellationToken)
        {
            var cacheKey = $"{geolocation.Name}_{DataType}";
            var result = await cacheService.GetOrCreateAsync(cacheKey, dataProvider, Expiration, cancellationToken);

            result.IfSucc(value =>
            {
                successAction(value);
                LastRefreshTime = DateTimeOffset.UtcNow;
            });

            result.IfFail(error =>
            {
                if (error is not OperationCanceledException)
                {
                    LogDataFetchError(logger, error, DataType, geolocation.Location);
                }
            });
        }
    }

    // 基接口，用于在列表中存储不同类型的刷新任务
    private interface IRefreshJob
    {
        string DataType { get; }
        TimeSpan Expiration { get; }
        DateTimeOffset? LastRefreshTime { get; set; }
        Task RefreshAsync(Geolocation geolocation, ICacheService cacheService, ILogger logger, CancellationToken cancellationToken);
    }


    [LoggerMessage(
        EventId = 1,
        Level = LogLevel.Error,
        Message = "Failed to get {DataType} for {Location}")]
    static partial void LogDataFetchError(ILogger logger, Exception error, string dataType, Location location);

    [RelayCommand(IncludeCancelCommand = true)]
    public async Task RefreshAllAsync(CancellationToken cancellationToken = default)
    {
        var invalidationTasks = _refreshJobs.Keys.Select(dataType =>
            _cacheService.InvalidateAsync($"{Geolocation.Name}_{dataType}", cancellationToken));
        await Task.WhenAll(invalidationTasks);
        await Task.WhenAll(_refreshJobs.Values.Select(job => job.RefreshAsync(Geolocation, _cacheService, _logger, cancellationToken)));
        RefreshedTime = DateTimeOffset.UtcNow;
    }



    [RelayCommand(IncludeCancelCommand = true)]
    public async Task RefreshIfNeededAsync(CancellationToken cancellationToken = default)
    {
        var now = DateTimeOffset.UtcNow;

        var tasksToRun = _refreshJobs.Values
            .Where(p => p.LastRefreshTime is null || (p.LastRefreshTime.Value + p.Expiration < now))
            .Select(p => p.RefreshAsync(Geolocation, _cacheService, _logger, cancellationToken))
            .ToList();

        if (tasksToRun.Count > 0)
        {
            await Task.WhenAll(tasksToRun);
        }
    }

    [RelayCommand(IncludeCancelCommand = true)]
    public Task GetDailiesAsync(CancellationToken cancellationToken = default)
        => _refreshJobs[nameof(Dailies)].RefreshAsync(Geolocation, _cacheService, _logger, cancellationToken);

    [RelayCommand(IncludeCancelCommand = true)]
    public Task GetHourliesAsync(CancellationToken cancellationToken = default)
        => _refreshJobs[nameof(Hourlies)].RefreshAsync(Geolocation, _cacheService, _logger, cancellationToken);

    [RelayCommand(IncludeCancelCommand = true)]
    public Task GetCurrentAsync(CancellationToken cancellationToken = default)
        => _refreshJobs[nameof(Current)].RefreshAsync(Geolocation, _cacheService, _logger, cancellationToken);

    [RelayCommand(IncludeCancelCommand = true)]
    public Task GetAlertsAsync(CancellationToken cancellationToken = default)
        => _refreshJobs[nameof(Alerts)].RefreshAsync(Geolocation, _cacheService, _logger, cancellationToken);

    [RelayCommand(IncludeCancelCommand = true)]
    public Task GetAirQualityAsync(CancellationToken cancellationToken = default)
        => _refreshJobs[nameof(AirQuality)].RefreshAsync(Geolocation, _cacheService, _logger, cancellationToken);
}