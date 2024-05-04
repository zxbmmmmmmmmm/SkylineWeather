using FluentWeather.Abstraction.Helpers;
using FluentWeather.Abstraction.Models;
using FluentWeather.Uwp.QWeatherProvider.Helpers;
using FluentWeather.Uwp.QWeatherProvider.Models;
using QWeatherApi.ApiContracts;

namespace FluentWeather.Uwp.QWeatherProvider.Mappers;

public static class HourlyForecastItemMapper
{
    public static WeatherHourlyBase MapToHourlyForecast(this WeatherHourlyResponse.HourlyForecastItem item)
    {
        return new WeatherHourlyBase
        {
            WeatherType = WeatherTypeConverter.GetWeatherTypeByIcon(int.Parse(item.Icon)),
            WindDirection = UnitConverter.GetWindDirectionFromAngle(item.Wind360),
            Description = item.Text,
            WindDirectionDescription = item.WindDir,
            WindScale = item.WindScale,
            WindSpeed = item.WindSpeed,
            Humidity = item.Humidity,
            Pressure = item.Pressure,
            Temperature = item.Temp,
            Time = item.FxTime,
            PrecipitationProbability = item.Pop,
            CloudAmount = item.Cloud,          
        };
    }
}