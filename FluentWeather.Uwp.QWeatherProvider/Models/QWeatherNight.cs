using FluentWeather.Abstraction.Interfaces.Weather;
using FluentWeather.Abstraction.Models;

namespace FluentWeather.Uwp.QWeatherProvider.Models;

public sealed class QWeatherNight : WeatherBase, IWind
{
    public string WindDirectionDescription { get; set; }
    public WindDirection WindDirection { get; set; }
    public string WindScale { get; set; }
    public int WindSpeed { get; set; }
}