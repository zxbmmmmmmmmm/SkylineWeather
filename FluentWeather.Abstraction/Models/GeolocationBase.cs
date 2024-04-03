using FluentWeather.Abstraction.Interfaces.Geolocation;
using System;

namespace FluentWeather.Abstraction.Models;

public class GeolocationBase:ITimeZone
{
    /// <summary>
    /// 地区名
    /// </summary>
    public string Name { get; set; }
    
    public Location Location { get; set; }

    /// <summary>
    /// 行政区
    /// </summary>
    public string? AdmDistrict { get; set; }

    public string? AdmDistrict2 { get; set; }


    /// <summary>
    /// 国家/地区
    /// </summary>
    public string? Country { get; set; }

    public string? TimeZone { get ; set; }
    public TimeSpan? UtcOffset { get ; set  ; }
    public bool? IsDaylightSavingTime { get ; set ; }

    public override string ToString()
    {
        return $"{Name} {Location}";
    }
    public GeolocationBase(string name, double lon, double lat)
    {
        Name = name;
        Location = new Location(lat, lon);
    }
    public GeolocationBase()
    {
          
    }
}