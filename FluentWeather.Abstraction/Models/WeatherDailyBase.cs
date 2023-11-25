using FluentWeather.Abstraction.Interfaces.Weather;
using System.Collections.Generic;
using System;

namespace FluentWeather.Abstraction.Models;

public class WeatherDailyBase : WeatherBase, IWeatherNight, ITemperatureRange, IWind, ITime, IHumidity, IPressure,
    IVisibility, IAstronomic
{
    public int Humidity { get; set; }
    public int Pressure { get; set; }
    public int MaxTemperature { get; set; }
    public int MinTemperature { get; set; }
    public DateTime Time { get; set; }
    public int Visibility { get; set; }
    public WeatherBase WeatherNight { get; set; }
    public string WindDirection { get; set; }
    public string WindScale { get; set; }
    public int WindSpeed { get; set; }
    public DateTime SunRise { get; set; }
    public DateTime SunSet { get; set; }
}