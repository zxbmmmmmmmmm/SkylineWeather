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
    public static async Task GetLocation()
    {
        var accessStatus = await Geolocator.RequestAccessAsync();
        switch (accessStatus)
        {
            case GeolocationAccessStatus.Allowed:

                Geolocator geolocator = new Geolocator { DesiredAccuracyInMeters = 100 };
                Geoposition pos = await geolocator.GetGeopositionAsync();
                var settingsHelper = Locator.ServiceProvider.GetService<ISettingsHelper>();
                settingsHelper.WriteLocalSetting(AppSettings.Longitude.ToString(),pos.Coordinate.Longitude);
                settingsHelper.WriteLocalSetting(AppSettings.Latitude.ToString(), pos.Coordinate.Latitude);
                break;
        }
    }
}