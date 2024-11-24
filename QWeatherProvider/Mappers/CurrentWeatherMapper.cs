using QWeatherApi.ApiContracts;
using QWeatherProvider.Helpers;
using SkylineWeather.Abstractions.Models;
using SkylineWeather.Abstractions.Models.Weather;
using UnitsNet;
using static QWeatherApi.ApiContracts.WeatherNowResponse;

namespace QWeatherProvider.Mappers;

public static class CurrentWeatherMapper
{
    public static CurrentWeather MapToCurrentWeather(this WeatherNowResponse.WeatherNowItem current)
    {
        return new CurrentWeather
        {
            Temperature = Temperature.FromDegreesCelsius(current.Temp),
            WeatherCode = WeatherTypeHelper.GetWeatherTypeByIcon(int.Parse(current.Icon)),
            Wind = new Wind
            {
                Speed = Speed.FromKilometersPerHour(current.WindSpeed),
                Direction = WindDirectionExtensions.GetWindDirectionFromAngle(current.Wind360)
            },
            Humidity = current.Humidity,
            CloudAmount = current.Cloud,
            Visibility = current.Vis,      
        };     
    }
}