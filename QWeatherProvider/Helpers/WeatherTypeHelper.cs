using SkylineWeather.Abstractions.Models;
using static SkylineWeather.Abstractions.Models.WeatherCode;

namespace QWeatherProvider.Helpers;

public class WeatherTypeHelper
{
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