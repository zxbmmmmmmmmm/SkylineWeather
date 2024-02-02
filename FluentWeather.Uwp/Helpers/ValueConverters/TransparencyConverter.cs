using System;
using Windows.UI.Xaml.Data;

namespace FluentWeather.Uwp.Helpers.ValueConverters;

public sealed class TransparencyConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        var doubleValue = double.Parse(value.ToString());
        return 1 - (doubleValue / 100);
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        var doubleValue = double.Parse(value.ToString());
        return (1 - doubleValue)*100;
    }
}