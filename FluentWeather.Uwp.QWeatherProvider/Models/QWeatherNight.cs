using FluentWeather.Abstraction.Interfaces.Weather;
using FluentWeather.Abstraction.Models;
using FluentWeather.Uwp.QWeatherProvider.Helpers;

namespace FluentWeather.Uwp.QWeatherProvider.Models;

public sealed class QWeatherNight : WeatherBase, IWind
{
    public override WeatherCode WeatherType => WeatherTypeConverter.GetWeatherTypeByDescription(Description);
    public string WindDirectionDescription { get; set; }
    public WindDirection WindDirection { get; set; }
    public string WindScale { get; set; }
    public int WindSpeed { get; set; }
}