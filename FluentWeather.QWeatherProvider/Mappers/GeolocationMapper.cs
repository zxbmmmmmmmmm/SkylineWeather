using FluentWeather.Abstraction.Models;
using FluentWeather.QGeoApi.ApiContracts;
using System;
using System.Runtime.CompilerServices;
using static FluentWeather.QGeoApi.ApiContracts.QGeolocationResponse;

namespace FluentWeather.QWeatherProvider.Mappers;
public static class GeolocationMapper
{
    public static GeolocationBase MapToGeolocationBase(this QGeolocationItem item)
    {
        return new GeolocationBase
        {
            AdmDistrict = item.AdministrativeDistrict1,
            IsDaylightSavingTime = item.IsDaylightSavingTime is "1",
            Latitude = double.Parse(item.Lat),
            Longitude = double.Parse(item.Lon),
            Name = item.Name,
            TimeZone = item.TimeZone,
            UtcOffset = TimeSpan.Parse(item.UtcOffset.Replace("+","")),
        };
    }
}