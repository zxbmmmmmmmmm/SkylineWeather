using FluentWeather.Abstraction.Models;
using FluentWeather.Abstraction.Strings;
using System;
using System.Globalization;
using System.Reflection;
using System.Resources;
using static FluentWeather.Abstraction.Models.WeatherType;
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