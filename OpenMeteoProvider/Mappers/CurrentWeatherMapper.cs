using SkylineWeather.Abstractions.Models;
using SkylineWeather.Abstractions.Models.Weather;
using UnitsNet;

namespace OpenMeteoProvider.Mappers;

public static class CurrentWeatherMapper
{
    public static CurrentWeather MapToCurrentWeather(this OpenMeteoApi.Models.CurrentWeather item)
    {
        return new CurrentWeather
        {
            WeatherCode = (WeatherCode)item.WeatherCode.GetValueOrDefault(),
            Temperature = Temperature.FromDegreesCelsius(item.Temperature2m.GetValueOrDefault()),
            CloudAmount = item.CloudCover.GetValueOrDefault(),
            Wind = new Wind
            {
                Speed = Speed.FromKilometersPerHour(item.WindSpeed10m.GetValueOrDefault()),
                Direction = WindDirectionExtensions.GetWindDirectionFromAngle(item.WindDirection10m.GetValueOrDefault()),
                Angle = Angle.FromDegrees(item.WindDirection10m.GetValueOrDefault())
            }
        };
    }
}