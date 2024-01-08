using FluentWeather.Abstraction.Interfaces.Weather;

namespace FluentWeather.Abstraction.Models;

public class WeatherNowBase:WeatherBase, IWind, ITemperature, IPressure, IHumidity
{
    public int? Humidity { get; set; }
    public int? Pressure { get; set; }
    public string? WindDirectionDescription { get; set; }
    public string? WindScale { get; set; }
    public required int WindSpeed { get; set; }
    public required int Temperature { get; set; }
    public required WindDirection WindDirection { get; set; }
}