using FluentWeather.Abstraction.Helpers;
using FluentWeather.Uwp.QWeatherProvider.Models;
using QWeatherApi.ApiContracts;

namespace FluentWeather.Uwp.QWeatherProvider.Mappers;

public static class DailyForecastItemMapper
{
    public static QWeatherDailyForecast MapToQWeatherDailyForecast(this WeatherDailyResponse.DailyForecastItem item)
    {
        return new QWeatherDailyForecast
        {
            WindDirection = UnitConverter.GetWindDirectionFromAngle(int.Parse(item.Wind360Day)),
            Description = item.TextDay,
            WindDirectionDescription = item.WindDirDay,
            WindScale = item.WindScaleDay,
            WindSpeed = item.WindSpeedDay,
            Humidity = item.Humidity,
            MaxTemperature = item.TempMax,
            MinTemperature = item.TempMin,
            Pressure = item.Pressure,
            Time = item.FxDate,
            Visibility = item.Vis,
            SunRise = item.Sunrise,
            SunSet = item.Sunset,
            CloudAmount = item.Cloud,

            WeatherNight = new QWeatherNight
            {
                Description = item.TextNight,
                WindDirection = UnitConverter.GetWindDirectionFromAngle(item.Wind360Night),
                WindDirectionDescription = item.WindDirNight,
                WindScale = item.WindScaleNight,
                WindSpeed = item.WindSpeedNight,
            }
        };
    }
}