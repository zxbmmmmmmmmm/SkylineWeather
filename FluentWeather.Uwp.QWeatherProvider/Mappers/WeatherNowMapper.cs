using FluentWeather.Abstraction.Helpers;
using FluentWeather.Abstraction.Models;
using FluentWeather.Uwp.QWeatherProvider.Helpers;
using QWeatherApi.ApiContracts;

namespace FluentWeather.Uwp.QWeatherProvider.Mappers;

public static class WeatherNowMapper
{
    extension(WeatherNowResponse.WeatherNowItem item)
    {
        public WeatherNowBase MapToQweatherNow()
        {
            return new WeatherNowBase
            {
                WeatherType = WeatherTypeConverter.GetWeatherTypeByIcon(int.Parse(item.Icon)),
                WindDirection = UnitConverter.GetWindDirectionFromAngle(item.Wind360),
                WindDirectionDescription = item.WindDir,
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
}