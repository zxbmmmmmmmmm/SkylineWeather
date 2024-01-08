using FluentWeather.Abstraction.Interfaces.Weather;
using FluentWeather.Abstraction.Models;
using FluentWeather.QWeatherProvider.Helpers;

namespace FluentWeather.QWeatherProvider.Models;

public class QWeatherNight : WeatherBase, IWind
{
    public override WeatherType WeatherType => WeatherTypeConverter.GetWeatherTypeByDescription(Description);
    public string WindDirectionDescription { get; set; }
    public WindDirection WindDirection { get; set; }
    public string WindScale { get; set; }
    public int WindSpeed { get; set; }
}