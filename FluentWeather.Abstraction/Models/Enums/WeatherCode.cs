namespace FluentWeather.Abstraction.Models;

/// <summary>
/// WMO Weather interpretation codes
/// </summary>
public enum WeatherCode
{
    #region 00-09 No precipitation, fog, dust storm, sandstorm, drifting or blowing snow at the station at the time of observation or, except for 09 during the preceding hour
    Clear = 0,

    MainlyClear = 1,
    PartlyCloudy = 2,
    Overcast = 3,

    Haze = 5,
    #endregion
    Mist = 10,
    #region 40-49 Fog
    Fog = 45,
    DepositingRimeFog = 48,
    #endregion
    #region 50-59 Drizzle
    LightDrizzle = 51,
    ModerateDrizzle = 53,
    DenseDrizzle = 55,

    LightFreezingDrizzle = 56,
    DenseFreezingDrizzle = 57,
    #endregion
    #region 60-69 Rain
    SlightRain = 61,
    ModerateRain = 63,
    HeavyRain = 65,

    LightFreezingRain = 66,
    HeavyFreezingRain = 67,
    #endregion
    #region 70-79 Solid precipitation not in showers
    SlightSnowFall = 71,
    ModerateSnowFall = 73,
    HeavySnowFall = 75,

    SnowGrains = 77,
    #endregion
    #region 80-99 Showery precipitation, or precipitation with current or recent thunderstorm
    SlightRainShowers = 80,
    ModerateRainShowers = 81,
    ViolentRainShowers = 82,

    SlightSleet = 83,
    ModerateOrHeavySleet = 84,

    SlightSnowShowers = 85,
    HeavySnowShowers = 86,

    SlightHail = 89,
    ModerateOrHeavyHail = 90,

    SlightOrModerateThunderstorm = 95,
    ThunderstormWithSlightHail = 96,
    HeavyThunderStorm = 97,
    ThunderstormWithHeavyHail = 99,
    #endregion
    Unknown = -1,
}