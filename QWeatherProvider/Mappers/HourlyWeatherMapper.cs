using QWeatherApi.ApiContracts;
using QWeatherProvider.Helpers;
using SkylineWeather.Abstractions.Models;
using SkylineWeather.Abstractions.Models.Weather;

namespace QWeatherProvider.Mappers;

public static class HourlyWeatherMapper
{
    //map hourlyweather
    public static HourlyWeather MapToHourlyWeather(this WeatherHourlyResponse.HourlyForecastItem item)
    {
        return new HourlyWeather
        {
            Time = item.FxTime,
            Temperature = item.Temp,
            WeatherCode = WeatherTypeHelper.GetWeatherTypeByIcon(int.Parse(item.Icon)),
            Wind = new Wind
            {
                Angle = item.Wind360,
                Direction = WindDirectionExtensions.GetWindDirectionFromAngle(item.Wind360),
                Speed = item.WindSpeed
            },
            Humidity = item.Humidity,
            CloudAmount = item.Cloud
        };
    }
}