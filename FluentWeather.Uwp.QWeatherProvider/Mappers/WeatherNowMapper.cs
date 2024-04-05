using FluentWeather.Abstraction.Helpers;
using FluentWeather.Abstraction.Models;
using FluentWeather.Uwp.QWeatherProvider.Helpers;
using FluentWeather.Uwp.QWeatherProvider.Models;
using QWeatherApi.ApiContracts;

namespace FluentWeather.Uwp.QWeatherProvider.Mappers;

public static class WeatherNowMapper
{
    public static WeatherNowBase MapToQweatherNow(this WeatherNowResponse.WeatherNowItem item)
    {
        return new WeatherNowBase
        {
            WeatherType = WeatherTypeConverter.GetWeatherTypeByDescription(item.Text),
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