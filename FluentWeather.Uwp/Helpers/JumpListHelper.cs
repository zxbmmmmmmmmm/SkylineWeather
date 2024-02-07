using FluentWeather.Abstraction.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;
using Windows.UI.StartScreen;

namespace FluentWeather.Uwp.Helpers;

public static class JumpListHelper
{
    public static async Task SetJumpList(GeolocationBase defaultLocation,IEnumerable<GeolocationBase> locations)
    {
        if (!JumpList.IsSupported()) return;
        var jumpList = await JumpList.LoadCurrentAsync();
        jumpList.Items.Clear();
        jumpList.Items.Add(CreateDefaultItem(defaultLocation));
        foreach(var location in locations)
        {
            jumpList.Items.Add(CreateItem(location));
        }
        await jumpList.SaveAsync();
    }
    private static JumpListItem CreateDefaultItem(GeolocationBase geolocation)
    {
        var item = JumpListItem.CreateWithArguments("City_" + geolocation.Name,geolocation.Name);
        item.GroupName = ResourceLoader.GetForCurrentView().GetString("CurrentLocationText");
        item.Logo = new Uri("ms-appx:///Assets/Icons/CurrentLocation.png");
        return item;
    }
    private static JumpListItem CreateItem(GeolocationBase geolocation)
    {
        var item = JumpListItem.CreateWithArguments("City_" + geolocation.Name, geolocation.Name);
        item.GroupName = ResourceLoader.GetForCurrentView().GetString("SavedLocations");
        item.Logo = new Uri("ms-appx:///Assets/Icons/SavedCity.png");
        return item;
    }
}