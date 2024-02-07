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
    //public static WeatherType GetWeatherType(int weatherCode)
    //{
    //    switch (weatherCode)
    //    {
    //        case 0:
    //            return Clear;
    //        case 1:
    //            return MostlyClear;
    //        case 2:
    //            return PartlyCloudy;
    //        case 3:
    //            return VeryCloudy;
    //        case 45:
    //            return Fog;
    //        case 48:
    //            return Fog;
    //        case 51:
    //            return LightRain;
    //        case 53:
    //            return LightRain;
    //        case 55:
    //            return LightRain;
    //        case 56:
    //            return LightRain;
    //        case 57:
    //            return LightRain;
    //        case 61:
    //            return LightRain;
    //        case 63:
    //            return ModerateRain;
    //        case 65:
    //            return HeavyRain;
    //        case 66:
    //            return FreezingRain;
    //        case 67:
    //            return FreezingRain;
    //        case 71:
    //            return LightSnow;
    //        case 73:
    //            return ModerateSnow;
    //        case 75:
    //            return HeavySnow;
    //        case 77:
    //            return LightSnow;
    //        case 80:
    //            return LightShowers ;
    //        case 81:
    //            return ModerateShowers;
    //        case 82:
    //            return HeavyShowers;
    //        case 85:
    //            return LightSnowShowers;
    //        case 86:
    //            return HeavySnowShowers;
    //        case 95:
    //            return Thunderstorm;
    //        case 96:
    //            return Thunderstorm;
    //        case 99:
    //            return Thunderstorm;
    //        default:
    //            return Unknown;
    //    }

    //}

    public static WeatherCode GetWeatherType(int weatherCode)
    {
        if (Enum.IsDefined(typeof(WeatherCode), weatherCode))
        {
            return (WeatherCode)weatherCode;
        }
        return WeatherCode.Unknown;
    }
    /// <summary>
    /// Converts a given weather code to it's string representation
    /// </summary>
    /// <param name="code"></param>
    /// <returns><see cref="string"/> WeatherCode string representation</returns>
    public static string GetWeatherDescription(int code)
    {

        var str = WeatherCodeDescription.ResourceManager.GetString("WeatherCode_" + code.ToString());
        str ??= WeatherCodeDescription.ResourceManager.GetString("WeatherCode_Unknown");
        return str!;
    }

}