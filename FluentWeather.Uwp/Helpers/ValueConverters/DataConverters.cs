using System;
using Windows.UI;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using FluentWeather.Abstraction.Models;
using Windows.ApplicationModel.Resources;

namespace FluentWeather.Uwp.Helpers.ValueConverters;

public class SeverityColorToColorConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is null) return null;
        var color = (SeverityColor)value;
        return ConverterMethods.SeverityColorToColor(color);
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException();
    }
}

public class WindScaleToDescriptionConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        return value is not string scale ? null : ConverterMethods.GetWindScaleDescription(scale);
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException();
    }
}

public class ToShortTimeStringConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        return ((DateTime)value).ToShortTimeString();
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException();
    }
}