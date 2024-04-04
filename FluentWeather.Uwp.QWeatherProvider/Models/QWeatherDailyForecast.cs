using FluentWeather.Abstraction.Models;
using FluentWeather.Uwp.QWeatherProvider.Helpers;

namespace FluentWeather.Uwp.QWeatherProvider.Models;
public sealed class QWeatherDailyForecast : WeatherDailyBase
{
    public override WeatherCode WeatherType => WeatherTypeConverter.GetWeatherTypeByDescription(Description);
}