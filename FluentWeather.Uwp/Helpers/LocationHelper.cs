using FluentWeather.Abstraction.Interfaces.GeolocationProvider;
using FluentWeather.Abstraction.Models;
using FluentWeather.DIContainer;
using FluentWeather.Uwp.Controls.Dialogs;
using FluentWeather.Uwp.Shared;
using Microsoft.Extensions.DependencyInjection;
using System.Globalization;
using System.Linq;
using Windows.Devices.Geolocation;

namespace FluentWeather.Uwp.Helpers;

public sealed class LocationHelper
{
    private const double LocationChangedTolerance = 0.001;
    public static async Task<(double lon, double lat)> UpdatePosition()
    {
        try
        {
            var accessStatus = await Geolocator.RequestAccessAsync();
            switch (accessStatus)
            {
                case GeolocationAccessStatus.Allowed:
                    var geolocator = new Geolocator { DesiredAccuracyInMeters = 10 };
                    var pos = await geolocator.GetGeopositionAsync();
                    return (pos.Coordinate.Point.Position.Longitude, pos.Coordinate.Point.Position.Latitude);

                case GeolocationAccessStatus.Unspecified:
                case GeolocationAccessStatus.Denied:
                default:
                    return (-1, -1);
            }
        }
        catch
        {
            return (-1, -1);
        }
    }

    public static async Task<GeolocationBase?> EnsureDefaultGeolocationAsync()
    {
        if (Common.Settings.DefaultGeolocation?.Name is not null)
        {
            return Common.Settings.DefaultGeolocation;
        }

        var (lon, lat) = await UpdatePosition();
        if (!IsValidPosition(lon, lat))
        {
            return await RequestLocationAsync(LocationDialogOptions.HideCancelButton);
        }

        var city = await TryResolveCityAsync(lat, lon);
        if (city is not null)
        {
            SaveDefaultLocation(city);
            return city;
        }

        return await RequestLocationAsync(
            LocationDialogOptions.HideCancelButton | LocationDialogOptions.CustomLocationExpanded,
            lon,
            lat);
    }

    public static async Task<GeolocationBase?> GetGeolocation()
    {
        var defaultLocation = await EnsureDefaultGeolocationAsync();
        if (defaultLocation?.Name is null)
        {
            return null;
        }

        if (!Common.Settings.UpdateLocationOnStartup)
        {
            return defaultLocation;
        }

        var updatedLocation = await RefreshCurrentLocationAsync();
        return updatedLocation ?? defaultLocation;
    }

    public static async Task<GeolocationBase?> RefreshCurrentLocationAsync()
    {
        if (Common.Settings.DefaultGeolocation?.Name is null)
        {
            return await EnsureDefaultGeolocationAsync();
        }

        var (lon, lat) = await UpdatePosition();
        if (!IsValidPosition(lon, lat) || !HasLocationChanged(lat, lon))
        {
            return null;
        }

        SaveLastPosition(lat, lon);
        return await TryResolveCityAsync(lat, lon) ?? CreateCurrentLocation(lat, lon);
    }

    private static bool IsValidPosition(double lon, double lat)
    {
        return lon is not -1 && lat is not -1;
    }

    private static bool HasLocationChanged(double lat, double lon)
    {
        if (Common.Settings.Latitude < 0 || Common.Settings.Longitude < 0)
        {
            return true;
        }

        return Math.Abs(Common.Settings.Latitude - lat) > LocationChangedTolerance ||
               Math.Abs(Common.Settings.Longitude - lon) > LocationChangedTolerance;
    }

    private static async Task<GeolocationBase?> TryResolveCityAsync(double lat, double lon)
    {
        var service = Locator.ServiceProvider.GetService<IGeolocationProvider>();
        if (service is null)
        {
            return null;
        }

        try
        {
            return (await service.GetCitiesGeolocationByLocation(lat, lon)).FirstOrDefault();
        }
        catch
        {
            return null;
        }
    }

    private static async Task<GeolocationBase?> RequestLocationAsync(LocationDialogOptions options, double? lon = null, double? lat = null)
    {
        var dialog = new LocationDialog(options);
        if (lon.HasValue && lat.HasValue)
        {
            dialog.Longitude = lon.Value.ToString(CultureInfo.InvariantCulture);
            dialog.Latitude = lat.Value.ToString(CultureInfo.InvariantCulture);
            dialog.TimeZone = TimeZoneHelper.GetTimeZoneFromLocation(lon.Value);
            dialog.Name = "CurrentLocation".GetLocalized();
        }

        await DialogManager.OpenDialogAsync(dialog);
        if (dialog.Result is null)
        {
            return null;
        }

        SaveDefaultLocation(dialog.Result);
        return dialog.Result;
    }

    private static void SaveDefaultLocation(GeolocationBase location)
    {
        Common.Settings.DefaultGeolocation = location;
        SaveLastPosition(location.Location.Latitude, location.Location.Longitude);
    }

    private static void SaveLastPosition(double lat, double lon)
    {
        Common.Settings.Latitude = lat;
        Common.Settings.Longitude = lon;
    }

    private static GeolocationBase CreateCurrentLocation(double lat, double lon)
    {
        var timeZone = TimeZoneHelper.GetTimeZoneFromLocation(lon);
        return new GeolocationBase
        {
            Name = "CurrentLocation".GetLocalized(),
            Location = new(lat, lon),
            TimeZone = timeZone.Id,
            UtcOffset = timeZone.BaseUtcOffset
        };
    }
}
