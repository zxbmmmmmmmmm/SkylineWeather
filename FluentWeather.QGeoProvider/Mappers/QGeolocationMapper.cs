using FluentWeather.QGeoApi.ApiContracts;
using FluentWeather.QGeoProvider.Models;
using System;
using System.Runtime.CompilerServices;
using static FluentWeather.QGeoApi.ApiContracts.QGeolocationResponse;

namespace FluentWeather.QGeoProvider.Mappers;

public static class QGeolocationMapper
{
    public static QGeolocation MapToQGeolocation(this QGeolocationItem item)
    {
        return new QGeolocation
        {
            AdmDistrict = item.AdministrativeDistrict1,
            IsDaylightSavingTime = bool.Parse(item.IsDaylightSavingTime) ,
            Latitude = int.Parse(item.Lat),
            Longitude = int.Parse(item.Lon),
            Name = item.Name,
            TimeZone = item.TimeZone,
            UtcOffset = TimeSpan.Parse(item.UtcOffset),
        };
    }
}