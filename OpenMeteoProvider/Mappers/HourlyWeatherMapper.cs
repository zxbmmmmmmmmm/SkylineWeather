using OpenMeteoApi.Models;
using SkylineWeather.Abstractions.Models.Weather;
using SkylineWeather.Abstractions.Models;

namespace OpenMeteoProvider.Mappers;

public static class HourlyWeatherMapper
{
    public static HourlyWeather MapToHourlyWeather(this HourlyForecastItem item)
    {
        return new HourlyWeather
        {
            WeatherCode = (WeatherCode)item.WeatherCode.GetValueOrDefault(),
            Time = DateTimeOffset.Parse(item.Time.GetValueOrDefault().ToString()),//TODO: DateTimeOffset
            Temperature = item.Temperature2m ?? throw new Exception("Temperature can't be null"),
            Wind = new Wind
            {
                Speed = item.WindSpeed10m,
                Direction = WindDirectionExtensions.GetWindDirectionFromAngle(item.WindDirection10m.GetValueOrDefault()),
                Angle = item.WindDirection10m.GetValueOrDefault()
            }
        };
    }
}