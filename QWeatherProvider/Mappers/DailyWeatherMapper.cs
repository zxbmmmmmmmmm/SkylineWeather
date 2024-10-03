using QWeatherApi.ApiContracts;
using QWeatherProvider.Helpers;
using SkylineWeather.Abstractions.Models;
using SkylineWeather.Abstractions.Models.Weather;

namespace QWeatherProvider.Mappers;

public static class DailyWeatherMapper
{
    //map daily weather
    public static DailyWeather MapToDailyWeather(this WeatherDailyResponse.DailyForecastItem item)
    {
        return new DailyWeather
        {
            Date = DateOnly.FromDateTime(item.FxDate),
            HighTemperature = item.TempMax,
            LowTemperature = item.TempMin,
            WeatherCode = WeatherTypeHelper.GetWeatherTypeByIcon(int.Parse(item.IconDay)),
            CloudAmount = item.Cloud,
            Humidity = item.Humidity,
            Visibility = item.Vis,
            Wind = new Wind()
            {
                Angle = int.Parse(item.Wind360Day),
                Direction = WindDirectionExtensions.GetWindDirectionFromAngle(int.Parse(item.Wind360Day)),
                Speed = item.WindSpeedDay
            },
            DaytimeWeather = new HalfDayWeather
            {
                IsDay = true,
                WeatherCode = WeatherTypeHelper.GetWeatherTypeByIcon(int.Parse(item.IconDay)),
                Wind = new Wind()
                {
                    Angle = int.Parse(item.Wind360Day),
                    Direction = WindDirectionExtensions.GetWindDirectionFromAngle(int.Parse(item.Wind360Day)),
                    Speed = item.WindSpeedDay
                },
            },
            NightWeather = new HalfDayWeather
            {
                IsDay = false,
                WeatherCode = WeatherTypeHelper.GetWeatherTypeByIcon(int.Parse(item.IconNight)),
                Wind = new Wind()
                {
                    Angle = item.Wind360Night,
                    Direction = WindDirectionExtensions.GetWindDirectionFromAngle(item.Wind360Night),
                    Speed = item.WindSpeedNight
                },
            },
            Sunrise = item.Sunrise is null ? null : TimeOnly.FromDateTime(item.Sunrise.Value),
            Sunset = item.Sunset is null ? null : TimeOnly.FromDateTime(item.Sunset.Value)
        };
    }
}