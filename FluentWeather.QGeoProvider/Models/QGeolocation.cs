using System;
using FluentWeather.Abstraction.Interfaces.Geolocation;
using FluentWeather.Abstraction.Models;

namespace FluentWeather.QGeoProvider.Models
{
    public class QGeolocation : GeolocationBase
    {
        public QGeolocation(string name,double lon, double lat):base(name,lon,lat)
        {
            
        }
        public QGeolocation()
        {
            
        }
    }
}