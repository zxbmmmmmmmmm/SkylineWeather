using QWeatherApi.ApiContracts;
using SkylineWeather.Abstractions.Models;

namespace QWeatherProvider.Mappers;

public static class GeolocationMapper
{
    //map geolocation
    public static Geolocation MapToGeolocation(this QGeolocationResponse.QGeolocationItem item)
    {
        return new Geolocation(item.Name, item.Lat, item.Lon)
        {
            AdmDistrict = item.AdministrativeDistrict1,
            AdmDistrict2 = item.AdministrativeDistrict2,
            Region = item.Country,
        };
    }
}