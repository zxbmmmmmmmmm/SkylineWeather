namespace FluentWeather.Abstraction.Models;

public class GeolocationBase
{
    /// <summary>
    /// 地区名
    /// </summary>
    public string Name { get; set; }
    /// <summary>
    /// 经度
    /// </summary>
    public double Longitude{ get; set; }
    /// <summary>
    /// 纬度
    /// </summary>
    public double Latitude { get; set; }
    /// <summary>
    /// 行政区
    /// </summary>
    public string AdmDistrict { get; set; }
}