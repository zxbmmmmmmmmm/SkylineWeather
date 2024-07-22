using FluentWeather.DIContainer;
using Microsoft.Extensions.DependencyInjection;
using Windows.Devices.Geolocation;
using FluentWeather.Abstraction.Interfaces.GeolocationProvider;
using FluentWeather.Abstraction.Models;
using FluentWeather.Uwp.Controls.Dialogs;
using FluentWeather.Uwp.Shared;

namespace FluentWeather.Uwp.Helpers;

public sealed class LocationHelper
{
    public static async Task<(double lon,double lat)> UpdatePosition()
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
    public static async Task<GeolocationBase> GetGeolocation()
    {
        //尝试获取位置
        //获取失败且默认位置未设置:弹出对话框，将默认位置作为当前位置
        //获取失败但默认位置已设置：将默认位置作为当前位置
        //获取成功:设置位置，并将当前位置设置为默认位置

        var service = Locator.ServiceProvider.GetService<IGeolocationProvider>();
        if (Common.Settings.DefaultGeolocation?.Name is null)//默认位置未设置
        {
            var (lon, lat) = await LocationHelper.UpdatePosition();
            if (lon is -1 || lat is -1)//获取位置失败
            {
                var dialog = new LocationDialog(LocationDialogOptions.HideCancelButton);
                await DialogManager.OpenDialogAsync(dialog);
                Common.Settings.DefaultGeolocation = dialog.Result;
                return dialog.Result;
            }
            var city = await service.GetCitiesGeolocationByLocation(lat, lon);
            if (city.Count is 0)//根据经纬度获取城市失败
            {
                var dialog = new LocationDialog(LocationDialogOptions.HideCancelButton);
                await DialogManager.OpenDialogAsync(dialog);
                Common.Settings.DefaultGeolocation = dialog.Result;
                return dialog.Result;
            }
            return city.First();
        }

        if (!Common.Settings.UpdateLocationOnStartup)//不更新位置
            return Common.Settings.DefaultGeolocation;

        //默认位置已设置但需要更新位置
        var (lo, la) = await LocationHelper.UpdatePosition();
        if (lo is -1 || la is -1)//检查失败
        {
            return Common.Settings.DefaultGeolocation;
        }
        var c = await service.GetCitiesGeolocationByLocation(la, lo);
        return c.Count is 0 ? Common.Settings.DefaultGeolocation : c.First();//若定位失败仍然使用默认位置
    }

}