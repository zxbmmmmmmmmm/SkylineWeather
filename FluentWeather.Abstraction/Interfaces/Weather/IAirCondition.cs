namespace FluentWeather.Abstraction.Interfaces.Weather;

public interface IAirCondition
{
    /// <summary>
    /// 空气质量指数
    /// </summary>
    int Aqi { get; set; }
    /// <summary>
    /// 空气质量指数等级
    /// </summary>
    int? AqiLevel { get; set; }
    /// <summary>
    /// 空气质量指数类别（优/良...）
    /// </summary>
    string? AqiCategory { get; set; }
}