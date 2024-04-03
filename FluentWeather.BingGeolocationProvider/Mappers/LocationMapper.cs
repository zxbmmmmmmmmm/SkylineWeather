using BingMapsRESTToolkit;
using FluentWeather.Abstraction.Models;

namespace FluentWeather.BingGeolocationProvider.Mappers;

public static class LocationMapper
{
    public static GeolocationBase MapToGeolocation(this BingMapsRESTToolkit.Location location)
    {
        return new GeolocationBase
        {
            Name = location.Address.Locality,
            AdmDistrict = location.Address.AdminDistrict,
            Location = new Abstraction.Models.Location(location.Point.Coordinates[0], location.Point.Coordinates[1]),
        };
    }
}