using OpenMeteoApi.Models;
using SkylineWeather.Abstractions.Models.Weather;
using SkylineWeather.Abstractions.Models;
using UnitsNet;

namespace OpenMeteoProvider.Mappers;

public static class HourlyWeatherMapper
{
    public static HourlyWeather MapToHourlyWeather(this HourlyForecastItem item)
    {
        return new HourlyWeather
        {
            WeatherCode = (WeatherCode)item.WeatherCode.GetValueOrDefault(),
            Time = DateTimeOffset.Parse(item.Time.GetValueOrDefault().ToString()),//TODO: Check if this is the correct way to parse the date
            Temperature = Temperature.FromDegreesCelsius(item.Temperature2m ?? throw new Exception("Temperature can't be null")),
            Wind = new Wind
            {
                Speed = Speed.FromKilometersPerHour(item.WindSpeed10m.GetValueOrDefault()),
                Direction = WindDirectionExtensions.GetWindDirectionFromAngle(item.WindDirection10m.GetValueOrDefault()),
                Angle = Angle.FromDegrees(item.WindDirection10m.GetValueOrDefault())
            }
        };
    }
}