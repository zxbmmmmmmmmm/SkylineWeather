using CommunityToolkit.Mvvm.ComponentModel;
using FluentWeather.Abstraction.Interfaces.Weather;
using FluentWeather.Abstraction.Interfaces.WeatherProvider;
using FluentWeather.Abstraction.Models;
using FluentWeather.DIContainer;
using FluentWeather.Tasks;
using FluentWeather.Uwp.Helpers;
using FluentWeather.Uwp.Shared;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Telerik.Geospatial;

namespace FluentWeather.Uwp.ViewModels;

public partial class MainPageViewModel : ObservableObject
{
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(DailyForecasts7D))]
    private List<WeatherBase> dailyForecasts =new();
    public List<WeatherBase> DailyForecasts7D =>(DailyForecasts.Count <7)? DailyForecasts.GetRange(0,DailyForecasts.Count) : DailyForecasts.GetRange(0, 7);

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HourlyForecasts24H))]
    private List<WeatherBase> hourlyForecasts = new();
    public List<WeatherBase> HourlyForecasts24H => (HourlyForecasts.Count < 24) ? HourlyForecasts.GetRange(0, HourlyForecasts.Count) : HourlyForecasts.GetRange(0, 24);

    [ObservableProperty]
    private List<WeatherWarningBase> warnings ;

    [ObservableProperty]
    private WeatherBase weatherNow;

    [ObservableProperty]
    private string weatherDescription;
    [ObservableProperty]
    private DateTime sunRise;
    [ObservableProperty]
    private DateTime sunSet;

    [ObservableProperty]
    private GeolocationBase currentLocation;
    [ObservableProperty]
    private List<IndicesBase> indices;
    [ObservableProperty]
    private PrecipitationBase precipitation;
    [ObservableProperty]
    private AirConditionBase airCondition;
    public static MainPageViewModel Instance{ get; private set; }

    public MainPageViewModel()
    {
        Instance = this;
    }

    partial void OnCurrentLocationChanged(GeolocationBase oldValue, GeolocationBase newValue)
    {
        GetWeather(CurrentLocation);
    }
    public async Task GetDailyForecast(double lon, double lat)
    {
        var dailyProvider = Locator.ServiceProvider.GetService<IDailyForecastProvider>();
        DailyForecasts = await dailyProvider.GetDailyForecasts(lon, lat);
        if (DailyForecasts[0] is IAstronomic astronomic)
        {
            SunRise = astronomic.SunRise;
            SunSet = astronomic.SunSet;
        }

    }
    public async Task GetHourlyForecast(double lon,double lat)
    {
        var hourlyProvider = Locator.ServiceProvider.GetService<IHourlyForecastProvider>();
        HourlyForecasts = await hourlyProvider.GetHourlyForecasts(lon, lat);
    }
    public async Task GetWeatherNow(double lon, double lat)
    {
        IsLoading = true;
        var nowProvider = Locator.ServiceProvider.GetService<ICurrentWeatherProvider>();
        WeatherNow = await nowProvider.GetCurrentWeather(lon, lat);
        IsLoading = false;
    }
    public async Task GetWeatherWarnings(double lon, double lat)
    {
        var warningProvider = Locator.ServiceProvider.GetService<IWeatherWarningProvider>();
        Warnings = await warningProvider.GetWeatherWarnings(lon, lat);
    }
    public async Task GetIndices(double lon, double lat)
    {
        var indicesProvider = Locator.ServiceProvider.GetService<IIndicesProvider>();
        var i = await indicesProvider.GetIndices(lon, lat);
        foreach (var item in i)
        {
            item.Name = item.Name.Replace("指数", "");
        }
        Indices = i;
    }
    public async Task GetWeatherPrecipitations(double lon, double lat)
    {
        var precipProvider = Locator.ServiceProvider.GetService<IPrecipitationProvider>();
        Precipitation = await precipProvider.GetPrecipitations(lon, lat);
    }
    public async Task GetAirCondition(double lon, double lat)
    {
        var airConditionProvider = Locator.ServiceProvider.GetService<IAirConditionProvider>();

        AirCondition = await airConditionProvider.GetAirCondition(lon, lat);

    }
    public async void GetWeather(GeolocationBase geo)
    
    {
        if (Common.Settings.QWeatherToken is "" || Common.Settings.QGeolocationToken is "")
        {
            IsLoading = false;
            return;
        }
        var lon = geo.Longitude;
        var lat = geo.Latitude;
        var cacheData = await CacheHelper.GetWeatherCache(CurrentLocation);
        if (cacheData is not null)
        {
            DailyForecasts = cacheData.DailyForecasts.ConvertAll(p => p as WeatherBase);
            SunRise = cacheData.SunRise;
            SunSet = cacheData.SunSet;
            AirCondition = cacheData.AirCondition;
            HourlyForecasts = cacheData.HourlyForecasts.ConvertAll(p => p as WeatherBase);
            Indices = cacheData.Indices;
            Precipitation = cacheData.Precipitation;
            Warnings = cacheData.Warnings.ConvertAll(p => p as WeatherWarningBase);
            WeatherDescription = cacheData.WeatherDescription;
            WeatherNow = cacheData.WeatherNow;
            IsLoading = false;
        }
        else
        {
            //await Task.Run(() => GetTasks(lon, lat));
            List<Task> tasks = new()
            {
                GetWeatherNow(lon, lat),
                GetWeatherWarnings(lon, lat),
                GetDailyForecast(lon, lat),
                GetHourlyForecast(lon, lat),
                GetWeatherPrecipitations(lon, lat),
                GetAirCondition(lon, lat),
                GetIndices(lon, lat),
            };
            await Task.WhenAll(tasks.ToArray());
            CacheHelper.Cache(this);
        }

        if (DailyForecasts[0] is ITemperatureRange currentTemperatureRange)
        {
            WeatherDescription = $"{WeatherNow.Description} {currentTemperatureRange.MinTemperature}° / {currentTemperatureRange.MaxTemperature}°";
        }
        if(CurrentLocation.Name == Common.Settings.DefaultGeolocation.Name)
        {
            TileHelper.UpdateTiles(DailyForecasts);
        }
    }
    [ObservableProperty]
    private bool isLoading = true;
}
