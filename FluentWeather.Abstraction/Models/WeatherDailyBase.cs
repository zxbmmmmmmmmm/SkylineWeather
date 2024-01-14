using FluentWeather.Abstraction.Interfaces.Weather;
using System.Collections.Generic;
using System;

namespace FluentWeather.Abstraction.Models;

public class WeatherDailyBase : WeatherBase, IWeatherNight, ITemperatureRange, IWind, ITime, IAstronomic,ICloudAmount,IVisibility,IPressure,IHumidity
{
    public  int MaxTemperature { get; set; }
    public  int MinTemperature { get; set; }
    public  DateTime Time { get; set; }
    public WeatherBase? WeatherNight { get; set; }
    public string? WindDirectionDescription { get; set; }
    public string? WindScale { get; set; }
    public  int WindSpeed { get; set; }
    public  DateTime SunRise { get; set; }
    public  DateTime SunSet { get; set; }
    public  WindDirection WindDirection { get; set; }
    public List<WeatherHourlyBase>? HourlyForecasts { get; set; }
    public int? CloudAmount { get; set; }
    public int? Visibility { get; set; }
    public int? Pressure { get; set; }
    public int? Humidity { get; set ; }
}