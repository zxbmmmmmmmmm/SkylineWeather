using FluentWeather.Abstraction.Models;
using System;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml.Data;
using static FluentWeather.Abstraction.Models.WindDirection;

namespace FluentWeather.Uwp.Helpers.ValueConverters;

public sealed class WindDirectionToDescriptionConverter:IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        var dir = (WindDirection)value;
        return ResourceLoader.GetForCurrentView().GetString("WindDirection_" + dir);
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException();
    }
}