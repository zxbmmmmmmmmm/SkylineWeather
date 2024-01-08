using System;
using FluentWeather.Abstraction.Interfaces.Geolocation;
using FluentWeather.Abstraction.Models;

namespace FluentWeather.QGeoProvider.Models
{
    public class QGeolocation : GeolocationBase, ITimeZone
    {
        public string? TimeZone { get; set; }
        public TimeSpan? UtcOffset { get; set; }
        public bool? IsDaylightSavingTime { get; set; }
        public QGeolocation(string name,double lon, double lat):base(name,lon,lat)
        {
            
        }
        public QGeolocation()
        {
            
        }
    }
}