using OpenMeteoApi.Models;
using SkylineWeather.Abstractions.Models;
using SkylineWeather.Abstractions.Models.Weather;

namespace OpenMeteoProvider.Mappers;

public static class DailyWeatherMapper
{
    public static DailyWeather MapToDailyWeather(this DailyForecastItem item)
    {
        return new DailyWeather
        {
            WeatherCode = (WeatherCode)item.WeatherCode.GetValueOrDefault(),
            Date = DateOnly.FromDateTime(item.Time.GetValueOrDefault()),
            Sunrise = item.Sunrise is null ? null : TimeOnly.Parse(item.Sunrise),
            Sunset = item.Sunset is null ? null : TimeOnly.Parse(item.Sunset),
            HighTemperature = item.Temperature2mMax ?? throw new Exception("Temperature can't be null"),
            LowTemperature = item.Temperature2mMin ?? throw new Exception("Temperature can't be null"),
            Wind = new Wind
            {
                Speed = item.WindSpeed10mMax,
                Direction = WindDirectionExtensions.GetWindDirectionFromAngle(item.WindDirection10mDominant.GetValueOrDefault()),
                Angle = item.WindDirection10mDominant.GetValueOrDefault()
            }
        };
    }
}