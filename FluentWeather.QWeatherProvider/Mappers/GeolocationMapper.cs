using FluentWeather.Abstraction.Models;
using System;
using System.Runtime.CompilerServices;
using static QWeatherApi.ApiContracts.QGeolocationResponse;

namespace FluentWeather.QWeatherProvider.Mappers;
public static class GeolocationMapper
{
    public static GeolocationBase MapToGeolocationBase(this QGeolocationItem item)
    {
        return new GeolocationBase
        {
            AdmDistrict = item.AdministrativeDistrict1,
            IsDaylightSavingTime = item.IsDaylightSavingTime is "1",
            Location = new(item.Lat, item.Lon),
            Name = item.Name,
            TimeZone = item.TimeZone,
            UtcOffset = TimeSpan.Parse(item.UtcOffset.Replace("+","")),
        };
    }
}