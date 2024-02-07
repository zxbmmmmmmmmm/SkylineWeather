using FluentWeather.Abstraction.Models;
using FluentWeather.Abstraction.Strings;

namespace FluentWeather.Abstraction.Helpers;

public static class ResourcesHelper
{

    public static string GetWeatherDescription(int code)
    {
        var str = Resources.ResourceManager.GetString("WeatherCode_" + code.ToString());
        str ??= Resources.ResourceManager.GetString("WeatherCode_Unknown");
        return str!;
    }
    public static string GetWindDirectionDescription(WindDirection dir)
    {
        var str = Resources.ResourceManager.GetString("WindDirection_" + dir.ToString());
        return str!;
    }
}