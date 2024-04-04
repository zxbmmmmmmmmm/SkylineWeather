using FluentWeather.Abstraction.Helpers;
using FluentWeather.Uwp.QWeatherProvider.Models;
using QWeatherApi.ApiContracts;

namespace FluentWeather.Uwp.QWeatherProvider.Mappers;

public static class WeatherNowMapper
{
    public static QWeatherNow MapToQweatherNow(this WeatherNowResponse.WeatherNowItem item)
    {
        return new QWeatherNow
        {
            WindDirection = UnitConverter.GetWindDirectionFromAngle(item.Wind360),
            WindDirectionDescription  = item.WindDir,
            WindScale = item.WindScale,
            WindSpeed = item.WindSpeed,
            Humidity = item.Humidity,
            Pressure = item.Pressure,
            Visibility = item.Vis,
            ApparentTemperature = item.FeelsLike,
            Temperature = item.Temp,
            Description = item.Text,
            CloudAmount = item.Cloud,
            DewPointTemperature = item.Dew
        }; 
    }
}