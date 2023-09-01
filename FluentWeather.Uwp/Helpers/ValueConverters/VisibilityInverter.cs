using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace FluentWeather.Uwp.Helpers.ValueConverters;

public class VisibilityInverter:IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is not Visibility visibility)return Visibility.Collapsed; 
        return visibility is Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException();
    }
}