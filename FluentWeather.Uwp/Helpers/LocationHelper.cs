using FluentWeather.Abstraction.Interfaces.Helpers;
using FluentWeather.DIContainer;
using FluentWeather.Uwp.Pages;
using FluentWeather.Uwp.Shared;
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
                try
                {
                    Common.LogManager.GetLogger("Application").Info("启动定位");
                    Geolocator geolocator = new Geolocator { DesiredAccuracyInMeters = 100 };
                    Geoposition pos = await geolocator.GetGeopositionAsync();
                    return (pos.Coordinate.Point.Position.Longitude, pos.Coordinate.Point.Position.Latitude);
                }
                catch(Exception e)
                {
                    Common.LogManager.GetLogger("Application").Info("定位失败:"+ e.Message);
                    return (-1, -1);
                }

            case GeolocationAccessStatus.Unspecified:
            case GeolocationAccessStatus.Denied:
            default:
                return (-1, -1);
        }
    }
}