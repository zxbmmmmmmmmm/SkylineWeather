using SkylineWeather.Abstractions.Models;
using SkylineWeather.Abstractions.Models.Weather;

namespace OpenMeteoProvider.Mappers;

public static class CurrentWeatherMapper
{
    public static CurrentWeather MapToCurrentWeather(this OpenMeteoApi.Models.CurrentWeather item)
    {
        return new CurrentWeather
        {
            WeatherCode = (WeatherCode)item.WeatherCode.GetValueOrDefault(),
            Temperature = item.Temperature2m.GetValueOrDefault(),
            CloudAmount = item.CloudCover.GetValueOrDefault(),
            Wind = new Wind
            {
                Speed = item.WindSpeed10m,
                Direction = WindDirectionExtensions.GetWindDirectionFromAngle(item.WindDirection10m.GetValueOrDefault()),
                Angle = item.WindDirection10m.GetValueOrDefault()
            }
        };
    }
}