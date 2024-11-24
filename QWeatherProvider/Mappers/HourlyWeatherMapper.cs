using QWeatherApi.ApiContracts;
using QWeatherProvider.Helpers;
using SkylineWeather.Abstractions.Models;
using SkylineWeather.Abstractions.Models.Weather;
using UnitsNet;

namespace QWeatherProvider.Mappers;

public static class HourlyWeatherMapper
{    public static HourlyWeather MapToHourlyWeather(this WeatherHourlyResponse.HourlyForecastItem item)
    {
        return new HourlyWeather
        {
            Time = item.FxTime,
            Temperature = Temperature.FromDegreesCelsius(item.Temp),
            WeatherCode = WeatherTypeHelper.GetWeatherTypeByIcon(int.Parse(item.Icon)),
            Wind = new Wind
            {
                Angle = Angle.FromDegrees(item.Wind360),
                Direction = WindDirectionExtensions.GetWindDirectionFromAngle(item.Wind360),
                Speed = Speed.FromKilometersPerHour(item.WindSpeed)
            },
            Humidity = item.Humidity,
            CloudAmount = item.Cloud
        };
    }
}