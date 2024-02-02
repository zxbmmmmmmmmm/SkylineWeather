using FluentWeather.Abstraction.Models;
using System;
using System.Diagnostics;
using Windows.Devices.Geolocation;
using Windows.UI.Xaml.Data;

namespace FluentWeather.Uwp.Helpers.ValueConverters;

public sealed class TyphoonTrackToGeopointConverter:IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if(value is TyphoonTrackBase track)
        {
            return new Geopoint(new BasicGeoposition { Latitude = track.Latitude, Longitude = track.Longitude });
        }
        return null;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException();
    }
}