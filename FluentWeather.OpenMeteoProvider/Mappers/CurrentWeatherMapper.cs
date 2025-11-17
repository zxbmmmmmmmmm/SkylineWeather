using FluentWeather.OpenMeteoProvider.Models;
using System;
using FluentWeather.Abstraction.Helpers;
using OpenMeteoApi.Models;

namespace FluentWeather.OpenMeteoProvider.Mappers;

public static class CurrentWeatherMapper
{
    extension(CurrentWeather item)
    {
        public OpenMeteoWeatherNow MapToOpenMeteoWeatherNow()
        {
            return new OpenMeteoWeatherNow
            {
                //Description = WeatherCodeHelper.GetWeatherDescription(item.WeatherCode!.Value),
                WeatherType = WeatherCodeHelper.GetWeatherType(item.WeatherCode!.Value),
                WindDirection = UnitConverter.GetWindDirectionFromAngle(item.WindDirection10m!.Value),
                WindScale = UnitConverter.GetWindScaleFromKM((int)item.WindSpeed10m!.Value).ToString(),
                WindSpeed = (int)item.WindSpeed10m,
                ApparentTemperature = (int)item.ApparentTemperature!.Value,
                Humidity = item.RelativeHumidity2m!.Value,
                Temperature = (int)Math.Round(item.Temperature2m!.Value),
                Pressure = item.SurfacePressure is null ? null : (int)Math.Round(item.SurfacePressure.Value),
                //Visibility = int.Parse(item.),
                CloudAmount = item.CloudCover

                //WeatherNight = new WeatherBase
                //{
                //    Description = item.TextNight,
                //    WindDirection = item.WindDirNight,
                //    WindScale = item.WindScaleNight,
                //    WindSpeed = int.Parse(item.WindSpeedNight),
                //}
            };
        }
    }
}