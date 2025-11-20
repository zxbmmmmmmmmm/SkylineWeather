// ReSharper disable InconsistentNaming
namespace FluentWeather.Abstraction.Interfaces.Weather;
/// <summary>
/// 空气污染物
/// </summary>
public interface IAirPollutants
{
    /// <summary>
    /// PM2.5
    /// </summary>
    double PM25 { get; set; }
    /// <summary>
    /// PM10
    /// </summary>
    double PM10 { get; set; }
    /// <summary>
    /// 二氧化氮
    /// </summary>
    double NO2 { get; set; }
    /// <summary>
    /// 二氧化硫
    /// </summary>
    double SO2 { get; set; }
    /// <summary>
    /// 一氧化碳
    /// </summary>
    double CO { get; set; }
    /// <summary>
    /// 臭氧
    /// </summary>
    double O3 { get; set; }
}