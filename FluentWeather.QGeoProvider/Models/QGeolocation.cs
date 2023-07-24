using System;
using FluentWeather.Abstraction.Interfaces.Geolocation;
using FluentWeather.Abstraction.Models;

namespace FluentWeather.QGeoProvider.Models
{
    public class QGeolocation : GeolocationBase, ITimeZone
    {
        public string TimeZone { get; set; }
        public DateTime UtcOffset { get; set; }
        public bool IsDaylightSavingTime { get; set; }
    }
}