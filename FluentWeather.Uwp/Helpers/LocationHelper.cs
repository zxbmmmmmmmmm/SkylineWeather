using FluentWeather.Abstraction.Interfaces.Helpers;
using FluentWeather.DIContainer;
using FluentWeather.Uwp.Pages;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.UI.Xaml;

namespace FluentWeather.Uwp.Helpers;

public class LocationHelper
{
    public static async Task<(double lon,double lat)> GetLocation()
    {
        var accessStatus = await Geolocator.RequestAccessAsync();
        switch (accessStatus)
        {
            case GeolocationAccessStatus.Allowed:

                Geolocator geolocator = new Geolocator { DesiredAccuracyInMeters = 100 };
                Geoposition pos = await geolocator.GetGeopositionAsync();
                return (pos.Coordinate.Point.Position.Longitude, pos.Coordinate.Point.Position.Latitude);

            case GeolocationAccessStatus.Unspecified:
            case GeolocationAccessStatus.Denied:
            default:
                return (-1, -1);
        }
    }
}