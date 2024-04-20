using BingMapsRESTToolkit;
using FluentWeather.Abstraction.Models;
using FluentWeather.BingGeolocationProvider.Helpers;
using FluentWeather.Uwp.Shared;
using System;
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
}