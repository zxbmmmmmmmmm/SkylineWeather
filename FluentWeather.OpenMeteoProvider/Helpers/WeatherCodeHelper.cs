using FluentWeather.Abstraction.Models;
using static FluentWeather.Abstraction.Models.WeatherType;
namespace FluentWeather.OpenMeteoProvider.Helpers;

public static class WeatherCodeHelper
{
    public static WeatherType GetWeatherType(int weatherCode)
    {
        switch (weatherCode)
        {
            case 0:
                return Clear;
            case 1:
                return MostlyClear;
            case 2:
                return PartlyCloudy;
            case 3:
                return VeryCloudy;
            case 45:
                return Fog;
            case 48:
                return Fog;
            case 51:
                return LightRain;
            case 53:
                return LightRain;
            case 55:
                return LightRain;
            case 56:
                return LightRain;
            case 57:
                return LightRain;
            case 61:
                return LightRain;
            case 63:
                return ModerateRain;
            case 65:
                return HeavyRain;
            case 66:
                return FreezingRain;
            case 67:
                return FreezingRain;
            case 71:
                return LightSnow;
            case 73:
                return ModerateSnow;
            case 75:
                return HeavySnow;
            case 77:
                return LightSnow;
            case 80:
                return LightShowers ;
            case 81:
                return ModerateShowers;
            case 82:
                return HeavyShowers;
            case 85:
                return LightSnowShowers;
            case 86:
                return HeavySnowShowers;
            case 95:
                return Thunderstorm;
            case 96:
                return Thunderstorm;
            case 99:
                return Thunderstorm;
            default:
                return Unknown;
        }

    }
    /// <summary>
    /// Converts a given weather code to it's string representation
    /// </summary>
    /// <param name="code"></param>
    /// <returns><see cref="string"/> WeatherCode string representation</returns>
    public static string GetWeatherDescription(int code)
    {
        switch (code)
        {
            case 0:
                return "Clear sky";
            case 1:
                return "Mainly clear";
            case 2:
                return "Partly cloudy";
            case 3:
                return "Overcast";
            case 45:
                return "Fog";
            case 48:
                return "Depositing rime Fog";
            case 51:
                return "Light drizzle";
            case 53:
                return "Moderate drizzle";
            case 55:
                return "Dense drizzle";
            case 56:
                return "Light freezing drizzle";
            case 57:
                return "Dense freezing drizzle";
            case 61:
                return "Slight rain";
            case 63:
                return "Moderate rain";
            case 65:
                return "Heavy rain";
            case 66:
                return "Light freezing rain";
            case 67:
                return "Heavy freezing rain";
            case 71:
                return "Slight snow fall";
            case 73:
                return "Moderate snow fall";
            case 75:
                return "Heavy snow fall";
            case 77:
                return "Snow grains";
            case 80:
                return "Slight rain showers";
            case 81:
                return "Moderate rain showers";
            case 82:
                return "Violent rain showers";
            case 85:
                return "Slight snow showers";
            case 86:
                return "Heavy snow showers";
            case 95:
                return "Thunderstorm";
            case 96:
                return "Thunderstorm with light hail";
            case 99:
                return "Thunderstorm with heavy hail";
            default:
                return "Invalid weather code";
        }
    }

}