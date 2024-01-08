using FluentWeather.Abstraction.Interfaces.Weather;
using System;

namespace FluentWeather.Abstraction.Models;

public class WeatherHourlyBase :WeatherBase, ITemperature, ITime, IWind, IHumidity, IPressure,IPrecipitationProbability
{
    public int? Humidity { get; set; }
    public int? Pressure { get; set; }
    public required int Temperature { get; set; }
    public required DateTime Time { get; set; }
    public string? WindDirectionDescription { get; set; }
    public string? WindScale { get; set; }
    public required int WindSpeed { get; set; }
    public int? PrecipitationProbability { get; set; }
    public required WindDirection WindDirection { get; set; }
}