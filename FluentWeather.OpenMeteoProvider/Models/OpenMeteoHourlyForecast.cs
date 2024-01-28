using FluentWeather.Abstraction.Interfaces.Weather;
using FluentWeather.Abstraction.Models;

namespace FluentWeather.OpenMeteoProvider.Models;

public class OpenMeteoHourlyForecast : WeatherHourlyBase, ICloudAmount
{
    public int? CloudAmount { get ; set; }
}