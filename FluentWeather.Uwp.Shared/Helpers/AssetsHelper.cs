using FluentWeather.Abstraction.Models;
using static FluentWeather.Abstraction.Models.WeatherCode;

namespace FluentWeather.Uwp.Shared.Helpers;

public static class AssetsHelper
{
    public static string GetWeatherIconName(WeatherCode weatherType)
    {
        return weatherType switch
        {
            SlightHail or ModerateOrHeavyHail => "BlowingHail.png",
            HeavyRain => "HeavyRain.png",
            ModerateRain => "LightRain.png",
            SlightRain => "LightRain.png",
            ModerateRainShowers => "LightRain.png",
            PartlyCloudy => "Cloudy.png",
            Overcast => "VeryCloudy.png",
            SlightSnowFall => "LightSnow.png",
            HeavySnowFall => "HeavySnow.png",
            Fog => "Fog.png",
            LightFreezingRain or HeavyFreezingRain => "FreezingRain.png",
            SlightSleet or ModerateOrHeavySleet => "RainSnow.png",
            ThunderstormWithHeavyHail or SlightOrModerateThunderstorm or ThunderstormWithSlightHail or HeavyThunderStorm => "Thundery.png",
            Clear => "SunnyDay.png",
            Haze => "HazeSmokeDay.png",
            MainlyClear => "MostlySunnyDay.png",
            ViolentRainShowers or ViolentRainShowers or SlightRainShowers => "RainShowersDay.png",
            SlightSnowShowers or HeavySnowShowers => "SnowShowersDay.png",
            _ => "PartlyCloudy.png",
        };
    }
    public static string GetBackgroundImageName(WeatherCode weather)
    {
        var code = (int)weather;
        if (code is 0)
            return "Clear";
        if (code is 1 or 2)
            return "PartlyCloudy";
        if (code is 3)
            return "Overcast";
        if (50 <= code && code <= 69 || (80 <= code && code <= 82))
            return "Rain";
        if (40 <= code && code <= 49)
            return "Fog";
        if (70 <= code && code <= 79)
            return "Snow";

        return "All";
    }
}