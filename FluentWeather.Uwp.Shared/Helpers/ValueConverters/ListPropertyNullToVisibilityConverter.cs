using System;
using System.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace FluentWeather.Uwp.Shared.Helpers.ValueConverters;

public class ListPropertyNullToVisibilityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is not IList list) return Visibility.Collapsed;
        if (list.Count == 0) return Visibility.Collapsed;
        var first = list[0];
        var type = first.GetType();
        var prop = type.GetProperty(parameter.ToString());
        if (prop is null) return Visibility.Collapsed;
        var result = prop.GetValue(first);
        return result is null ? Visibility.Collapsed : Visibility.Visible;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException();
    }
}