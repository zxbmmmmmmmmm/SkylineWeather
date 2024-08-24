﻿using FluentWeather.OpenMeteoProvider.Models;
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
            WeatherType = WeatherCodeHelper.GetWeatherType(item.WeatherCode.GetValueOrDefault()),
            WindDirection = UnitConverter.GetWindDirectionFromAngle(item.WindDirection10mDominant.GetValueOrDefault()),
            WindSpeed = (int)Math.Round(item.WindSpeed10mMax.GetValueOrDefault()),
            WindScale = UnitConverter.GetWindScaleFromKM((int)Math.Round(item.WindSpeed10mMax.GetValueOrDefault())).ToString(),
            //Humidity = int.Parse(item.),
            MaxTemperature = (int)Math.Round(item.Temperature2mMax.GetValueOrDefault()),
            MinTemperature = (int)Math.Round(item.Temperature2mMin.GetValueOrDefault()),
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