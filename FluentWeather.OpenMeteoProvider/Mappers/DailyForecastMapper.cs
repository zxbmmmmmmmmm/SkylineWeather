using FluentWeather.Abstraction.Models;
using FluentWeather.OpenMeteoProvider.Models;
using System;
using OpenMeteoApi.Models;
using FluentWeather.Abstraction.Helpers;

namespace FluentWeather.OpenMeteoProvider.Mappers;

public static class DailyForecastMapper
{
    public static OpenMeteoDailyForecast MapToOpenMeteoDailyForecast(this DailyForecastItem item)
    {
        return new OpenMeteoDailyForecast
        {
            //Description = WeatherCodeHelper.GetWeatherDescription(item.WeatherCode!.Value),
            WeatherType = WeatherCodeHelper.GetWeatherType(item.WeatherCode!.Value),
            WindDirection = UnitConverter.GetWindDirectionFromAngle(item.WindDirection10mDominant!.Value),
            WindSpeed = (int)item.WindSpeed10mMax!,
            WindScale = UnitConverter.GetWindScaleFromKM((int)item.WindSpeed10mMax!.Value).ToString(),
            //Humidity = int.Parse(item.),
            MaxTemperature = (int)item.Temperature2mMax!,
            MinTemperature = (int)item.Temperature2mMin!,
            //Pressure = int.Parse(item),
            Time = item.Time!.Value,
            //Visibility = int.Parse(item.),
            SunRise = DateTime.Parse(item.Sunrise),
            SunSet = DateTime.Parse(item.Sunset),
            //CloudAmount = item

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