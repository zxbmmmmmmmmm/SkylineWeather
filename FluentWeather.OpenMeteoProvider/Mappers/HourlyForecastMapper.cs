using FluentWeather.Abstraction.Helpers;
using FluentWeather.OpenMeteoProvider.Helpers;
using FluentWeather.OpenMeteoProvider.Models;
using OpenMeteoApi.Models;
using System;

namespace FluentWeather.OpenMeteoProvider.Mappers;

public static class HourlyForecastMapper
{
    public static OpenMeteoHourlyForecast MapToOpenMeteoHourlyForecast(this HourlyForecastItem item)
    {
        return new OpenMeteoHourlyForecast
        {
            WindDirection = UnitConverter.GetWindDirectionFromAngle(item.WindDirection10m!.Value),
            Description = WeatherCodeHelper.GetWeatherDescription(item.WeatherCode!.Value),
            WindScale = UnitConverter.GetWindScaleFromKM((int)item.WindSpeed10m!.Value).ToString(),
            WindSpeed = (int)item.WindSpeed10m!.Value,
            Humidity = item.RelativeHumidity2m,
            Pressure = (int?)item.SurfacePressure,
            Temperature = (int)item.Temperature2m!,
            Time = item.Time!.Value,
            PrecipitationProbability = item.PrecipitationProbability,
            CloudAmount = item.CloudCover
        };
    }
}