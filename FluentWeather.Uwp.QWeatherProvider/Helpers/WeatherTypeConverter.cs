using FluentWeather.Abstraction.Models;
using System;
using static FluentWeather.Abstraction.Models.WeatherCode;
namespace FluentWeather.Uwp.QWeatherProvider.Helpers;


public static class WeatherTypeConverter
{
    public static WeatherCode GetWeatherTypeByDescription(string description)
    {
        description = description.ToLower();
        if (description.Contains("晴") || description.Contains("clear"))
            return Clear;
        if (description.Contains("阴") || description.Contains("overcast"))
            return Overcast;
        if (description.Contains("小雨") || description.Contains("light rain"))
            return SlightRain;
        if (description.Contains("中雨") || description.Contains("moderate rain"))
            return ModerateRain;
        if (description.Contains("大雨") || description.Contains("heavy rain"))
            return HeavyRain;
        if (description.Contains("局部多云") || description.Contains("partly cloudy"))
            return PartlyCloudy;
        if (description.Contains("大部多云") || description.Contains("cloudy"))
            return PartlyCloudy;
        if (description.Contains("少云") || description.Contains("mainly clear"))
            return MainlyClear;

        if (description.Contains("冰雹") || description.Contains("hail"))
            return SlightHail;
        if (description.Contains("雷阵雨") || description.Contains("thunder"))
            return HeavyThunderStorm;
        if (description.Contains("小雪") || description.Contains("light snow"))
            return SlightSnowFall;
        if (description.Contains("大雪") || description.Contains("heavy snow"))
            return HeavySnowFall;
        if (description.Contains("冻雨") || description.Contains("freezing rain"))
            return LightFreezingRain;

        if (description.Contains("雪") || description.Contains("snow"))
            return ModerateSnowFall;
        if (description.Contains("雷") || description.Contains("thunder"))
            return SlightOrModerateThunderstorm;
        if (description.Contains("雨") || description.Contains("rain"))
            return ModerateRain;
        if (description.Contains("雾") || description.Contains("fog"))
            return Fog;
        if (description.Contains("霾") || description.Contains("haze"))
            return Haze;
        if (description.Contains("多云") || description.Contains("cloud"))
            return PartlyCloudy;



        return Unknown;
    }

    public static WeatherCode GetWeatherTypeByIcon(int code)
    {
        return code switch
        {
            // Rain
            300 => SlightRainShowers,
            301 => ModerateRainShowers,
            //  302-304 thunder shower
            302 => SlightOrModerateThunderstorm,
            303 => SlightOrModerateThunderstorm,
            304 => SlightOrModerateThunderstorm,

            305 => SlightRain,
            306 => ModerateRain,
            307 => HeavyRain,

            308 => ViolentRainShowers,
            309 => LightDrizzle,
            // 310-312  rain storm 
            310 => ModerateRain,
            311 => HeavyRain,
            312 => HeavyRain,

            313 => HeavyFreezingRain,
            // 314-318  rain storm
            314 => SlightRain,
            315 => ModerateRain,
            316 => HeavyRain,
            317 => HeavyRain,
            318 => HeavyRain,

            350 => SlightRainShowers,
            351 => ModerateRainShowers,
            399 => ModerateRain,


            400 => SlightSnowFall,
            401 => ModerateSnowFall,
            402 => HeavySnowFall,
            403 => HeavySnowShowers,
            404 => SnowGrains,
            405 => SnowGrains,
            406 => SnowGrains,
            407 => SlightSnowShowers,
            408 => SlightSnowFall,
            409 => ModerateSnowFall,
            410 => HeavySnowFall,

            456 => SnowGrains,
            457 => SlightSnowShowers,
            499 => SlightSnowFall,


            500 => Mist,
            501 => Fog,
            502 => Haze,
            503 => Dust,
            504 => Dust,

            507 => Dust,
            508 => Dust,
            509 => Fog,
            510 => Fog,

            511 => Haze,
            512 => Haze,
            513 => Haze,
            514 => Haze,
            515 => Haze,

            // Cloud
            100 => Clear,
            101 => PartlyCloudy,
            102 => MainlyClear,
            103 => PartlyCloudy,
            104 => Overcast,
            150 => Clear,
            151 => PartlyCloudy,
            152 => MainlyClear,
            153 => PartlyCloudy,



            900 => Clear,
            901 => Overcast,
            999 => Clear,
            _ => Unknown
        };
    }
}