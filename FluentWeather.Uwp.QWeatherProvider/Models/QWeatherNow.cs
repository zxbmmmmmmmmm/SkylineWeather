using FluentWeather.Abstraction.Models;
using FluentWeather.Uwp.QWeatherProvider.Helpers;

namespace FluentWeather.Uwp.QWeatherProvider.Models;

public sealed class QWeatherNow : WeatherNowBase
{
    public override WeatherCode WeatherType => WeatherTypeConverter.GetWeatherTypeByDescription(Description);
}