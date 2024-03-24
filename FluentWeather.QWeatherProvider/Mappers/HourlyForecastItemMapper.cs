using FluentWeather.Abstraction.Helpers;
using FluentWeather.QWeatherProvider.Models;
using System;
using QWeatherApi.ApiContracts;

namespace FluentWeather.QWeatherProvider.Mappers;

public static class HourlyForecastItemMapper
{
    public static QWeatherHourlyForecast MapToQWeatherHourlyForecast(this WeatherHourlyResponse.HourlyForecastItem item)
    {
        return new QWeatherHourlyForecast
        {
            WindDirection = UnitConverter.GetWindDirectionFromAngle(item.Wind360),
            Description = item.Text,
            WindDirectionDescription = item.WindDir,
            WindScale = item.WindScale,
            WindSpeed = item.WindSpeed,
            Humidity = item.Humidity,
            Pressure = item.Pressure,
            Temperature = item.Temp,
            Time = item.FxTime,
            PrecipitationProbability = item.Pop,
            CloudAmount = item.Cloud
        };
    }
}