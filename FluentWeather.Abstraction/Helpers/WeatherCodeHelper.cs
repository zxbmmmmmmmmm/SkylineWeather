using FluentWeather.Abstraction.Models;
using System;

namespace FluentWeather.Abstraction.Helpers;

public static class WeatherCodeHelper
{
    public static WeatherCode GetWeatherType(int weatherCode)
    {
        if (Enum.IsDefined(typeof(WeatherCode), weatherCode))
        {
            return (WeatherCode)weatherCode;
        }
        return WeatherCode.Unknown;
    }

}