using System;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml.Data;

namespace FluentWeather.Uwp.Helpers.ValueConverters;

public sealed class WindScaleToDescriptionConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        return value is not string scale ? null : GetWindDescription(scale);
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException();
    }
    public static string GetWindDescription(string scale)
    {
        if(scale.Contains("-"))
        {
            var s = scale.Split("-");
            scale = s[1];
        }
        return ResourceLoader.GetForCurrentView().GetString("WindScaleDescription_" + scale);
    }
}