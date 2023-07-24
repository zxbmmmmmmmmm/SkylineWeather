namespace FluentWeather.Abstraction.Models;

public abstract class WeatherBase
{
    public virtual WeatherType WeatherType { get; set; }
    public string Description { get; set; }
}
public enum WeatherType
{
    /// <summary>
    /// 阴
    /// </summary>
    Cloudy,
    /// <summary>
    /// 雾
    /// </summary>
    Fog,
    /// <summary>
    /// 大雨
    /// </summary>
    HeavyRain,
    /// <summary>
    /// 暴雨
    /// </summary>
    HeavyShowers,
    /// <summary>
    /// 大雪
    /// </summary>
    HeavySnow,
    /// <summary>
    /// 暴雪
    /// </summary>
    HeavySnowShowers,
    /// <summary>
    /// 小雨
    /// </summary>
    LightRain,
    /// <summary>
    /// 小阵雨
    /// </summary>
    LightShowers,
    /// <summary>
    /// 雨夹雪
    /// </summary>
    LightSleet,
    /// <summary>
    /// 阵雨夹雪
    /// </summary>
    LightSleetShowers,
    /// <summary>
    /// 小雪
    /// </summary>
    LightSnow,
    /// <summary>
    /// 阵小雪
    /// </summary>
    LightSnowShowers,
    /// <summary>
    /// 多云
    /// </summary>
    PartlyCloudy,
    /// <summary>
    /// 晴朗
    /// </summary>
    Clear,
    /// <summary>
    /// 雷暴
    /// </summary>
    ThunderyHeavyRain,
    /// <summary>
    /// 雷阵雨
    /// </summary>
    ThunderyShowers,
    /// <summary>
    /// 雷阵雪
    /// </summary>
    ThunderySnowShowers,
    /// <summary>
    /// 阴
    /// </summary>
    VeryCloudy,
    /// <summary>
    /// 冰雹
    /// </summary>
    Hail,
    /// <summary>
    /// 未知
    /// </summary>
    Unknown,
}