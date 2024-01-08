using FluentWeather.Abstraction.Interfaces.Weather;
using FluentWeather.Abstraction.Models;
using FluentWeather.QWeatherProvider.Helpers;

namespace FluentWeather.QWeatherProvider.Models;

public class QWeatherNow : WeatherNowBase
{
    public override WeatherType WeatherType => WeatherTypeConverter.GetWeatherTypeByDescription(Description);
}