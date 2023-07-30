using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FluentWeather.Abstraction.Interfaces.GeolocationProvider;
using FluentWeather.Abstraction.Interfaces.Helpers;
using FluentWeather.Abstraction.Interfaces.Weather;
using FluentWeather.Abstraction.Interfaces.WeatherProvider;
using FluentWeather.Abstraction.Models;
using FluentWeather.DIContainer;
using FluentWeather.QWeatherProvider.Models;
using FluentWeather.Uwp.Helpers;
using Microsoft.AppCenter.Ingestion.Models;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FluentWeather.Uwp.ViewModels;

public partial class MainPageViewModel : ObservableObject
{
    [ObservableProperty]
    private List<WeatherBase> dailyForecasts =new();

    [ObservableProperty]
    private List<WeatherBase> hourlyForecasts ;
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
    public async void GetDailyForecast(double lon, double lat)
    {
        var dailyProvider = Locator.ServiceProvider.GetService<IDailyForecastProvider>();
        DailyForecasts = await dailyProvider.GetDailyForecasts(lon, lat);
        if (DailyForecasts[0] is IAstronomic astronomic)
        {
            SunRise = astronomic.SunRise;
            SunSet = astronomic.SunSet;
        }
        if (DailyForecasts[0] is ITemperatureRange currentTemperatureRange)
        {
            WeatherDescription = $"{WeatherNow.Description} {currentTemperatureRange.MinTemperature}° / {currentTemperatureRange.MaxTemperature}°";
        }
    }
    public async void GetHourlyForecast(double lon,double lat)
    {
        var hourlyProvider = Locator.ServiceProvider.GetService<IHourlyForecastProvider>();
        HourlyForecasts = await hourlyProvider.GetHourlyForecasts(lon, lat);
    }
    public async void GetWeatherNow(double lon, double lat)
    {
        IsLoading = true;
        var nowProvider = Locator.ServiceProvider.GetService<ICurrentWeatherProvider>();
        WeatherNow = await nowProvider.GetCurrentWeather(lon, lat);
        IsLoading = false;
    }
    public async void GetWeatherWarnings(double lon, double lat)
    {
        var warningProvider = Locator.ServiceProvider.GetService<IWeatherWarningProvider>();
        Warnings = await warningProvider.GetWeatherWarnings(lon, lat);
    }
    public async void GetIndices(double lon, double lat)
    {
        var indicesProvider = Locator.ServiceProvider.GetService<IIndicesProvider>();
        var i = await indicesProvider.GetIndices(lon, lat);
        foreach (var item in i)
        {
            item.Name = item.Name.Replace("指数", "");
        }
        Indices = i;
    }
    public async void GetWeatherPrecipitations(double lon, double lat)
    {
        var precipProvider = Locator.ServiceProvider.GetService<IPrecipitationProvider>();
        Precipitation = await precipProvider.GetPrecipitations(lon, lat);
    }
    public async void GetAirCondition(double lon, double lat)
    {
        var airConditionProvider = Locator.ServiceProvider.GetService<IAirConditionProvider>();
        AirCondition = await airConditionProvider.GetAirCondition(lon, lat);
        if (AirCondition is not null)
        {
            WeatherDescription += $"  空气质量-{AirCondition.AqiCategory}";
        }
    }
    public void GetWeather(GeolocationBase geo)
    {
        var lon = geo.Longitude;
        var lat = geo.Latitude;
        GetWeatherNow(lon, lat);
        GetWeatherWarnings(lon, lat);
        GetDailyForecast(lon, lat);
        GetHourlyForecast(lon, lat);
        GetWeatherPrecipitations(lon, lat);
        GetAirCondition(lon, lat);
        GetIndices(lon, lat);     
    }
    [ObservableProperty]
    private bool isLoading = true;
}