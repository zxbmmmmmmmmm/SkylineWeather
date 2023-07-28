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
            IsDaylightSavingTime = item.IsDaylightSavingTime is "1",
            Latitude = double.Parse(item.Lat),
            Longitude = double.Parse(item.Lon),
            Name = item.Name,
            TimeZone = item.TimeZone,
            //UtcOffset = TimeSpan.Parse(item.UtcOffset),
        };
    }
}