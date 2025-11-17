using FluentWeather.Abstraction.Helpers;
using FluentWeather.Abstraction.Models;
using OpenMeteoApi.Models;
using System;

namespace FluentWeather.OpenMeteoProvider.Mappers;

public static class HourlyForecastMapper
{
    extension(HourlyForecastItem item)
    {
        public WeatherHourlyBase MapToHourlyForecast()
        {
            return new WeatherHourlyBase
            {
                WeatherType = WeatherCodeHelper.GetWeatherType(item.WeatherCode!.Value),
                WindDirection = UnitConverter.GetWindDirectionFromAngle(item.WindDirection10m!.Value),
                //Description = WeatherCodeHelper.GetWeatherDescription(item.WeatherCode!.Value),
                WindScale = UnitConverter.GetWindScaleFromKM((int)item.WindSpeed10m!.Value).ToString(),
                WindSpeed = (int)Math.Round(item.WindSpeed10m!.Value),
                Humidity = item.RelativeHumidity2m,
                Pressure = item.SurfacePressure is null ? null : (int)Math.Round(item.SurfacePressure.Value),
                Temperature = (int)Math.Round(item.Temperature2m!.Value),
                Time = item.Time!.Value,
                PrecipitationProbability = item.PrecipitationProbability,
                CloudAmount = item.CloudCover,
                Visibility = item.Visibility is null ? null : (int)Math.Round(item.Visibility.Value / 1000),
            };
        }
    }
}