using CommunityToolkit.Mvvm.ComponentModel;
using FluentWeather.Abstraction.Interfaces.Helpers;
using FluentWeather.Abstraction.Interfaces.Weather;
using FluentWeather.Abstraction.Interfaces.WeatherProvider;
using FluentWeather.Abstraction.Models;
using FluentWeather.DIContainer;
using FluentWeather.QWeatherProvider.Models;
using FluentWeather.Uwp.Helpers;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

namespace FluentWeather.Uwp.ViewModels;

public partial class MainPageViewModel : ObservableObject
{
    [ObservableProperty]
    private List<WeatherBase> dailyForecasts = new();

    [ObservableProperty]
    private List<WeatherBase> hourlyForecasts = new();

    [ObservableProperty]
    private WeatherBase weatherNow;

    [ObservableProperty]
    private string weatherDescription;

    [ObservableProperty]
    private GeolocationBase currentLocation;
    public static MainPageViewModel Instance{ get; private set; }

    public MainPageViewModel()
    {
        GetWeather();
        Instance = this;
    }

    private async void GetWeather()
    {
        await LocationHelper.GetLocation();
        var settingsHelper = Locator.ServiceProvider.GetService<ISettingsHelper>();
        var lon = settingsHelper.ReadLocalSetting(AppSettings.Longitude.ToString(), 116.0);
        var lat = settingsHelper.ReadLocalSetting(AppSettings.Latitude.ToString(), 40.0);

        var dailyProvider = Locator.ServiceProvider.GetService<IDailyForecastProvider>();
        DailyForecasts = await dailyProvider.GetDailyForecasts(lon, lat);
        var hourlyProvider = Locator.ServiceProvider.GetService<IHourlyForecastProvider>();
        HourlyForecasts = await hourlyProvider.GetHourlyForecasts(lon, lat);
        var nowProvider = Locator.ServiceProvider.GetService<ICurrentWeatherProvider>();
        WeatherNow = await nowProvider.GetCurrentWeather(lon,lat);
        if(DailyForecasts[0] is ITemperatureRange currentTemperatureRange)
        {
            WeatherDescription = $"{WeatherNow.Description} {currentTemperatureRange.MinTemperature}° / {currentTemperatureRange.MaxTemperature}°";
        }
    }
}