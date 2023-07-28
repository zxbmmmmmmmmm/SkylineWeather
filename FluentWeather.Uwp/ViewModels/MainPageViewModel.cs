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
    private GeolocationBase currentLocation;
    [ObservableProperty]
    private List<IndicesBase> indices;
    
    public static MainPageViewModel Instance{ get; private set; }

    public MainPageViewModel()
    {
        Instance = this;
    }

    partial void OnCurrentLocationChanged(GeolocationBase oldValue, GeolocationBase newValue)
    {
        GetWeather(CurrentLocation);
    }
    public async void GetWeather(GeolocationBase geo)
    {
        IsLoading = true;
        var dailyProvider = Locator.ServiceProvider.GetService<IDailyForecastProvider>();
        DailyForecasts = await dailyProvider.GetDailyForecasts(geo.Longitude, geo.Latitude);
        var hourlyProvider = Locator.ServiceProvider.GetService<IHourlyForecastProvider>();
        HourlyForecasts = await hourlyProvider.GetHourlyForecasts(geo.Longitude, geo.Latitude);
        var nowProvider = Locator.ServiceProvider.GetService<ICurrentWeatherProvider>();
        WeatherNow = await nowProvider.GetCurrentWeather(geo.Longitude, geo.Latitude);
        var warningProvider = Locator.ServiceProvider.GetService<IWeatherWarningProvider>();
        Warnings = await warningProvider.GetWeatherWarnings(geo.Longitude,geo.Latitude);
        var indicesProvider = Locator.ServiceProvider.GetService<IIndicesProvider>();
        var i = await indicesProvider.GetIndices(geo.Longitude, geo.Latitude);
        foreach(var item in i)
        {
            item.Name = item.Name.Replace("指数", "");
        }
        Indices = i;
        if (DailyForecasts[0] is ITemperatureRange currentTemperatureRange)
        {
            WeatherDescription = $"{WeatherNow.Description} {currentTemperatureRange.MinTemperature}° / {currentTemperatureRange.MaxTemperature}°";
        }
        IsLoading = false;
    }
    [ObservableProperty]
    private bool isLoading = true;
}