using Microsoft.Extensions.Logging;
using SkylineWeather.Abstractions.Models;
using SkylineWeather.Abstractions.Provider.Interfaces;
using SkylineWeather.Abstractions.Services;
using SkylineWeather.DataAnalyzer.Models;
using UnitsNet;

namespace SkylineWeather.ViewModels;

public class WeatherViewModelFactory(
    ICurrentWeatherProvider currentWeatherProvider,
    IDailyWeatherProvider dailyWeatherProvider,
    IHourlyWeatherProvider hourlyWeatherProvider,
    IAlertProvider alertProvider,
    IAirQualityProvider airQualityProvider,
    ITrendAnalyzer<(Temperature min, Temperature max), TemperatureTrend> temperatureTrendAnalyzer,
    ICacheService cacheService,
    ILogger logger)
{
    public WeatherViewModel Create(Geolocation geolocation)
    {
        return new WeatherViewModel(
            geolocation,
            currentWeatherProvider,
            dailyWeatherProvider,
            hourlyWeatherProvider,
            alertProvider,
            airQualityProvider,
            temperatureTrendAnalyzer,
            cacheService,
            logger);
    }
}