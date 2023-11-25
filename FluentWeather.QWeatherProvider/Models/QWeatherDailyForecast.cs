using System;
using System.Collections.Generic;
using FluentWeather.Abstraction.Interfaces.Weather;
using FluentWeather.Abstraction.Models;
using FluentWeather.QWeatherProvider.Helpers;

namespace FluentWeather.QWeatherProvider.Models;
public class QWeatherDailyForecast : WeatherDailyBase,ICloudAmount
{
    public override WeatherType WeatherType => WeatherTypeConverter.GetWeatherTypeByDescription(Description);
    public int? CloudAmount { get; set; }
    public List<WeatherHourlyBase> HourlyForecasts { get; set; } 
}