using System;
using FluentWeather.Abstraction.Models;
using static QWeatherApi.ApiContracts.QGeolocationResponse;

namespace FluentWeather.Uwp.QWeatherProvider.Mappers;

public static class GeolocationMapper
{
    extension(QGeolocationItem item)
    {
        public GeolocationBase MapToGeolocationBase()
        {
            return new GeolocationBase
            {
                AdmDistrict = item.AdministrativeDistrict1,
                AdmDistrict2 = item.AdministrativeDistrict2,
                Country = item.Country,
                IsDaylightSavingTime = item.IsDaylightSavingTime is "1",
                Location = new(item.Lat, item.Lon),
                Name = item.Name,
                TimeZone = item.TimeZone,
                UtcOffset = TimeSpan.Parse(item.UtcOffset.Replace("+", "")),
            };
        }
    }
}