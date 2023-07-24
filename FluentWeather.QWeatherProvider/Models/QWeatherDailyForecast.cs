using System;
using FluentWeather.Abstraction.Interfaces.Weather;
using FluentWeather.Abstraction.Models;
using FluentWeather.QWeatherProvider.Helpers;

namespace FluentWeather.QWeatherProvider.Models;

public class QWeatherDailyForecast : WeatherBase, IWeatherNight, ITemperatureRange, IWind, ITime, IHumidity, IPressure,
    IVisibility
{
    public override WeatherType WeatherType => WeatherTypeConverter.GetWeatherTypeByDescription(Description);
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
}