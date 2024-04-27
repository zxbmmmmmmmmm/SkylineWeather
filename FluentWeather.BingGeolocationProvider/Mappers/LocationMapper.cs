using BingMapsRESTToolkit;
using FluentWeather.Abstraction.Interfaces.Geolocation;
using FluentWeather.Abstraction.Models;
using FluentWeather.BingGeolocationProvider.Helpers;
using FluentWeather.Uwp.Shared;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace FluentWeather.BingGeolocationProvider.Mappers;

public static class LocationMapper
{
    public static GeolocationBase MapToGeolocation(this BingMapsRESTToolkit.Location location)
    {
        var result = new GeolocationBase
        {
            Country = location.Address.CountryRegion.Replace("中华人民共和国","中国"),
            AdmDistrict = location.Address.AdminDistrict,
            AdmDistrict2 = location.Address.AdminDistrict2,
            Location = new Abstraction.Models.Location(location.Point.Coordinates[0], location.Point.Coordinates[1]),
        };
        result.TimeZone = GetTimeZoneFromLocation(result.Location.Longitude).Id;
        result.UtcOffset = GetTimeZoneFromLocation(result.Location.Longitude).BaseUtcOffset;
        var name = location.Name;

        if(Common.Settings.Language.ToLower().Contains("zh"))
        {
            if (location.Address.AdminDistrict is not null && location.Address.AdminDistrict != name)
            {
                name = name.ReplaceOnce(location.Address.AdminDistrict, "");
            }
            if (location.Address.Locality is not null && location.Address.Locality != name)
            {
                name = name.ReplaceOnce(location.Address.Locality, "");
            }
            if (location.Address.AdminDistrict2 is not null && location.Address.AdminDistrict2 != name)
            {
                name = name.ReplaceOnce(location.Address.AdminDistrict2, "");
            }
            if (name.Last() is '区' or '市')
            {
                var span = name.AsSpan();
                span = span.Slice(0, span.Length - 1);
                name = span.ToString();
            }
        }

        result.Name = name;
        return result;
    }

    public static TimeZoneInfo GetTimeZoneFromLocation(double longitude)
    {
        var timeZone = 0;

        var quotient = (int)(longitude / 15);
        var remainder = Math.Abs(longitude % 15);
        if (remainder <= 7.5)
        {
            timeZone = quotient;
        }
        else
        {
            timeZone = quotient + (longitude > 0 ? 1 : -1);
        }

        return TimeZones.FirstOrDefault(p => p.BaseUtcOffset == TimeSpan.FromHours(1) * timeZone);
    }

    public static ReadOnlyCollection<TimeZoneInfo> TimeZones = TimeZoneInfo.GetSystemTimeZones();
}