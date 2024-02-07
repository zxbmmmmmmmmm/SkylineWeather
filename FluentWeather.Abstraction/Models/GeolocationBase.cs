using FluentWeather.Abstraction.Interfaces.Geolocation;
using System;

namespace FluentWeather.Abstraction.Models;

public class GeolocationBase:ITimeZone
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
    public string? AdmDistrict { get; set; }
    public string? TimeZone { get ; set; }
    public TimeSpan? UtcOffset { get ; set  ; }
    public bool? IsDaylightSavingTime { get ; set ; }

    public GeolocationBase(string name, double lon, double lat)
    {
        Name = name;
        Longitude = lon;
        Latitude = lat;
    }
    public GeolocationBase()
    {
          
    }
}