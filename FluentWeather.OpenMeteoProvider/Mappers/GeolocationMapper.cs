using FluentWeather.Abstraction.Models;
using OpenMeteoApi.Models.Geocoding;

namespace FluentWeather.OpenMeteoProvider.Mappers;

public static class GeolocationMapper
{
    extension(Geolocation geolocation)
    {
        public GeolocationBase MapToGeolocationBase()
        {
            return new GeolocationBase()
            {
                Location = new Location(geolocation.Latitude, geolocation.Longitude),
                AdmDistrict = geolocation.Admin1,
                AdmDistrict2 = geolocation.Admin2,
                Country = geolocation.Country,
                Name = geolocation.Name,
                TimeZone = geolocation.Timezone,
            };
        }
    }
}