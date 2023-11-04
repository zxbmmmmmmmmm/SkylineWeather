using System;
using System.Collections.Generic;
using FluentWeather.Abstraction.Models;

namespace FluentWeather.QWeatherProvider.Models;

public class QWeatherData
{
    public List<QWeatherDailyForecast> DailyForecasts { get; set; } = new();
    public List<QWeatherHourlyForecast> HourlyForecasts { get; set; } = new();
    public List<QWeatherWarning> Warnings { get; set; }
    public QWeatherNow WeatherNow { get; set; }
    public string WeatherDescription { get; set; }
    public DateTime SunRise { get; set; }
    public DateTime SunSet { get; set; }
    public GeolocationBase Location { get; set; }
    public List<IndicesBase> Indices { get; set; }
    public QWeatherPrecipitation Precipitation { get; set; }
    public QAirCondition AirCondition { get; set; }
    public DateTime UpdatedTime { get; set; }
}
