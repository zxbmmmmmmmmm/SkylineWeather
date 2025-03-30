using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FluentWeather.Abstraction.Interfaces.Weather;
using FluentWeather.Abstraction.Interfaces.WeatherProvider;
using FluentWeather.Abstraction.Models;
using FluentWeather.Abstraction.Models.Exceptions;
using FluentWeather.DIContainer;
using FluentWeather.Uwp.Helpers;
using FluentWeather.Uwp.Shared;
using FluentWeather.Uwp.ViewModels.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Windows.ApplicationModel.Resources;
using FluentWeather.Uwp.Shared.Helpers;

namespace FluentWeather.Uwp.ViewModels;

public sealed partial class MainPageViewModel : ObservableObject, IMainPageViewModel
{
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(DailyForecasts7D))]
    [NotifyPropertyChangedFor(nameof(WeatherToday))]
    public partial List<WeatherDailyBase> DailyForecasts { get; set; } = new();

    public List<WeatherDailyBase> DailyForecasts7D => (DailyForecasts.Count < 7) ? DailyForecasts.GetRange(0, DailyForecasts.Count) : DailyForecasts.GetRange(0, 7);

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HourlyForecasts24H))]
    public partial List<WeatherHourlyBase> HourlyForecasts { get; set; } = new();

    public List<WeatherHourlyBase> HourlyForecasts24H => (HourlyForecasts.Count < 24) ? HourlyForecasts.GetRange(0, HourlyForecasts.Count) : HourlyForecasts.GetRange(0, 24);

    public WeatherDailyBase WeatherToday => DailyForecasts.FirstOrDefault();

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(WeatherToday))]
    public partial HistoricalDailyWeatherBase HistoricalWeather { get; set; }

    [ObservableProperty]
    public partial List<WeatherWarningBase> Warnings { get; set; }

    [ObservableProperty]
    public partial WeatherNowBase WeatherNow { get; set; }

    [ObservableProperty]
    public partial string WeatherDescription { get; set; }

    [ObservableProperty]
    public partial DateTime? SunRise { get; set; }

    [ObservableProperty]
    public partial DateTime? SunSet { get; set; }

    [ObservableProperty]
    public partial GeolocationBase CurrentGeolocation { get; set; }

    [ObservableProperty]
    public partial List<IndicesBase> Indices { get; set; }

    [ObservableProperty]
    public partial List<Announcement> Announcements { get; set; } = Announcement.GetAvailableAnnouncements().Where(p => !Common.Settings.ClosedAnnouncements.Contains(p.Name)).ToList();

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(TotalPrecipitation))]
    [NotifyPropertyChangedFor(nameof(HasPrecipitation))]
    public partial PrecipitationBase Precipitation { get; set; }

    [ObservableProperty]
    public partial AirConditionBase AirCondition { get; set; }

    public double? TotalPrecipitation => Precipitation?.Precipitations?.Sum(p => p.Precipitation);
    public bool HasPrecipitation => TotalPrecipitation > 0;
    public static MainPageViewModel Instance { get; private set; }

    [ObservableProperty]
    public partial DateTimeOffset UpdateTime { get; private set; }

    public MainPageViewModel()
    {
        Instance = this;
    }

    partial void OnCurrentGeolocationChanged(GeolocationBase oldValue, GeolocationBase newValue)
    {
        GetWeather(CurrentGeolocation);
    }

    [RelayCommand]
    public async Task GetDailyForecast(Location location)
    {
        var dailyProvider = Locator.ServiceProvider.GetService<IDailyForecastProvider>();
        if (dailyProvider is null) return;
        DailyForecasts = await dailyProvider.GetDailyForecasts(location.Longitude, location.Latitude);
        if (DailyForecasts[0] is IAstronomic astronomic)
        {
            SunRise = astronomic.SunRise;
            SunSet = astronomic.SunSet;
        }
    }

    [RelayCommand]
    public async Task GetHourlyForecast(Location location)
    {
        var hourlyProvider = Locator.ServiceProvider.GetService<IHourlyForecastProvider>();
        if (hourlyProvider is null) return;
        var hourlyForecasts = await hourlyProvider.GetHourlyForecasts(location.Longitude, location.Latitude);

        foreach (var forecast in hourlyForecasts)
        {
            if (CurrentGeolocation.UtcOffset is not null)
            {
                if (forecast.Time.Kind is not DateTimeKind.Utc)
                {
                    forecast.Time = forecast.Time.ToUniversalTime();
                }
                forecast.Time += (TimeSpan)CurrentGeolocation.UtcOffset;
            }
        }
        HourlyForecasts = hourlyForecasts;
    }

    [RelayCommand]
    public async Task GetWeatherNow(Location location)
    {
        var nowProvider = Locator.ServiceProvider.GetService<ICurrentWeatherProvider>();
        if (nowProvider is null) return;
        WeatherNow = await nowProvider.GetCurrentWeather(location.Longitude, location.Latitude);
    }

    [RelayCommand]
    public async Task GetWeatherWarnings(Location location)
    {
        var warningProvider = Locator.ServiceProvider.GetService<IWeatherWarningProvider>();
        if (warningProvider is null) return;
        Warnings = await warningProvider.GetWeatherWarnings(location.Longitude, location.Latitude);
    }

    [RelayCommand]
    public async Task GetIndices(Location location)
    {
        var indicesProvider = Locator.ServiceProvider.GetService<IIndicesProvider>();
        if (indicesProvider is null) return;
        var indices = await indicesProvider.GetIndices(location.Longitude, location.Latitude);
        indices?.ForEach(p => p.Name = p.Name.Replace("指数", ""));
        Indices = indices;
    }

    [RelayCommand]
    public async Task GetWeatherPrecipitations(Location location)
    {
        var precipProvider = Locator.ServiceProvider.GetService<IPrecipitationProvider>();
        if (precipProvider is null) return;
        var precip = await precipProvider.GetPrecipitations(location.Longitude, location.Latitude);
        if (precip.Summary is "" or null)
        {
            precip.Summary = ResourceLoader.GetForCurrentView().GetString(precip?.Precipitations?.Sum(p => p.Precipitation) > 0 ? "HasPrecipitationText" : "NoPrecipitationText");
        }
        Precipitation = precip;
    }

    [RelayCommand]
    public async Task GetAirCondition(Location location)
    {
        var airConditionProvider = Locator.ServiceProvider.GetService<IAirConditionProvider>();
        if (airConditionProvider is null) return;
        AirCondition = await airConditionProvider.GetAirCondition(location.Longitude, location.Latitude);
    }

    [RelayCommand]
    public async Task Refresh()
    {
        List<Task> tasks = new()
        {
            GetWeatherNowCommand.ExecuteAsync(CurrentGeolocation.Location),
            GetWeatherWarningsCommand.ExecuteAsync(CurrentGeolocation.Location),
            GetDailyForecastCommand.ExecuteAsync(CurrentGeolocation.Location),
            GetHourlyForecastCommand.ExecuteAsync(CurrentGeolocation.Location),
            GetWeatherPrecipitationsCommand.ExecuteAsync(CurrentGeolocation.Location),
            GetIndicesCommand.ExecuteAsync(CurrentGeolocation.Location),
            GetAirConditionCommand.ExecuteAsync(CurrentGeolocation.Location),
        };
        try
        {
            try
            {
                await Task.WhenAll(tasks.ToArray());
            }
            finally
            {
                if (DailyForecasts[0] is ITemperatureRange currentTemperatureRange)
                {
                    WeatherDescription = $"{WeatherNow.Description} {currentTemperatureRange.MinTemperature}° / {currentTemperatureRange.MaxTemperature}°";
                }
                if (CurrentGeolocation.Name == Common.Settings.DefaultGeolocation?.Name)
                {
                    try
                    {
                        TileHelper.UpdateForecastTile(DailyForecasts);
                    }
                    catch
                    {
                    }
                }
                foreach (var hourly in HourlyForecasts)
                {
                    var daily = DailyForecasts.Find(p => p.Time.Date == hourly.Time.Date);
                    if (daily is null) continue;
                    daily.HourlyForecasts ??= new List<WeatherHourlyBase>();
                    daily.HourlyForecasts?.Add(hourly);
                }
                await CacheHelper.CacheAsync(this);
            }
            UpdateTime = DateTimeOffset.Now;
        }
        catch (HttpResponseException e)
        {
            InfoBarHelper.Error(ResourceLoader.GetForCurrentView().GetString("GetDataFailed"), e.Message);
        }
    }

    [RelayCommand]
    public async Task GetWeather(GeolocationBase geo)
    {
        var cacheData = await CacheHelper.GetCacheAsync(CurrentGeolocation);
        if (cacheData is not null)
        {
            DailyForecasts = cacheData.DailyForecasts;
            SunRise = cacheData.SunRise;
            SunSet = cacheData.SunSet;
            AirCondition = cacheData.AirCondition!;
            HourlyForecasts = cacheData.HourlyForecasts;
            Indices = cacheData.Indices!;
            Precipitation = cacheData.Precipitation!;
            Warnings = cacheData.Warnings!;
            WeatherNow = cacheData.WeatherNow;
            UpdateTime = cacheData.UpdatedTime;
        }
        else
        {
            await Refresh();
        }
        await GetHistoricalWeather(CurrentGeolocation.Location);
    }

    [RelayCommand]
    public async Task GetHistoricalWeather(Location location)
    {
        HistoricalWeather = await HistoricalWeatherHelper.GetHistoricalWeatherAsync(location, DateTime.Now);
    }

    [RelayCommand]
    public void SpeechWeather()
    {
        var loader = ResourceLoader.GetForCurrentView();
        var text = $"{CurrentGeolocation.Name},{DailyForecasts[0].Description},{loader.GetString("HighestTemperature")}:{DailyForecasts[0].MaxTemperature}°,{loader.GetString("LowestTemperature")}:{DailyForecasts[0].MinTemperature}°";
        text += $",{loader.GetString("AirQuality")}:{AirCondition.AqiCategory}";
        if (!TTSHelper.IsPlaying)
        {
            InfoBarHelper.Info(loader.GetString("SpeechWeather"), text, 9000, false);
        }
        TTSHelper.Speech(text);
    }
}