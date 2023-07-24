using FluentWeather.Abstraction.Models;

namespace FluentWeather.QWeatherProvider.Helpers;

public static class WeatherTypeConverter
{
    public static WeatherType GetWeatherTypeByDescription(string description)
    {
        if (description.Contains("冰雹"))
        {
            return WeatherType.Hail;
        }
        if (description.Contains("雷阵雨"))
        {
            return WeatherType.ThunderyShowers;
        }
        if (description.Contains("小雨"))
        {
            return WeatherType.LightRain;
        }
        if (description.Contains("大雨"))
        {
            return WeatherType.HeavyRain;
        }
        if (description.Contains("雷"))
        {
            return WeatherType.ThunderyHeavyRain;
        }
        if (description.Contains("雨"))
        {
            return WeatherType.LightRain;
        }
        if (description.Contains("雾"))
        {
            return WeatherType.Fog;
        }
        if (description.Contains("霾"))
        {
            return WeatherType.Fog;
        }
        if (description.Contains("阴"))
        {
            return WeatherType.Cloudy;
        }
        if (description.Contains("多云"))
        {
            return WeatherType.PartlyCloudy;
        }
        if (description.Contains("晴"))
        {
            return WeatherType.Clear;
        }

        return WeatherType.Unknown;
    }
}