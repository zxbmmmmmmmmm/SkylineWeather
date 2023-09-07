using FluentWeather.QWeatherApi.ApiContracts;
using FluentWeather.QWeatherProvider.Models;
using System;

namespace FluentWeather.QWeatherProvider.Mappers;

public static class HourlyForecastItemMapper
{
    public static QWeatherHourlyForecast MapToQWeatherHourlyForecast(this WeatherHourlyResponse.HourlyForecastItem item)
    {
        return new QWeatherHourlyForecast
        {
            Description = item.Text,
            WindDirection = item.WindDir,
            WindScale = item.WindScale,
            WindSpeed = int.Parse(item.WindSpeed),
            Humidity = int.Parse(item.Humidity),
            Pressure = int.Parse(item.Pressure),
            Temperature = int.Parse(item.Temp),
            Time = DateTime.Parse(item.FxTime),
            PrecipitationProbability = item.Pop is null or "" ? null : int.Parse(item.Pop),
            CloudAmount = item.Cloud is not "" ? int.Parse(item.Cloud) : null
        };
    }
}