using FluentWeather.Abstraction.Helpers;
using FluentWeather.QWeatherApi.ApiContracts;
using FluentWeather.QWeatherProvider.Models;

namespace FluentWeather.QWeatherProvider.Mappers;

public static class WeatherNowMapper
{
    public static QWeatherNow MapToQweatherNow(this WeatherNowResponse.WeatherNowItem item)
    {
        return new QWeatherNow
        {
            WindDirection = UnitConverter.GetWindDirectionFromAngle(int.Parse(item.Wind360)),
            WindDirectionDescription  = item.WindDir,
            WindScale = item.WindScale,
            WindSpeed = int.Parse(item.WindSpeed),
            Humidity = int.Parse(item.Humidity),
            Pressure = int.Parse(item.Pressure),
            Visibility = int.Parse(item.Vis),
            ApparentTemperature = int.Parse(item.FeelsLike),
            Temperature = int.Parse(item.Temp),
            Description = item.Text,
            CloudAmount = item.Cloud is not "" ? int.Parse(item.Cloud) : null,
            DewPointTemperature = item.Dew is not "" ? int.Parse(item.Dew) : null
        }; 
    }
}