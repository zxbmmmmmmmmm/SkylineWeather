using FluentWeather.Abstraction.Models;
using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace FluentWeather.Uwp.Helpers.ValueConverters;

public sealed class AutoSuggestSuggestionParameterConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        // cast value to whatever EventArgs class you are expecting here
        var args = (AutoSuggestBoxSuggestionChosenEventArgs)value;
        // return what you need from the args
        return (GeolocationBase)args.SelectedItem;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException();
    }
}