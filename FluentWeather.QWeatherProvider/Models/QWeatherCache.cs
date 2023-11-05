using System;
using System.Collections.Generic;
using FluentWeather.Abstraction.Models;

namespace FluentWeather.QWeatherProvider.Models;

public class QWeatherCache:WeatherCacheBase
{
    public new List<QWeatherDailyForecast> DailyForecasts { get; set; } = new();
    public new List<QWeatherHourlyForecast> HourlyForecasts { get; set; } = new();
    public new List<QWeatherWarning> Warnings { get; set; }
    public new QWeatherNow WeatherNow { get; set; }
    public new QWeatherPrecipitation Precipitation { get; set; }
    public new QAirCondition AirCondition { get; set; }
}
