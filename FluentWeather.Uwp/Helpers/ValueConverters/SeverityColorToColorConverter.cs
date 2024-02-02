using FluentWeather.Abstraction.Models;
using System;
using Windows.UI;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace FluentWeather.Uwp.Helpers.ValueConverters;

public sealed class SeverityColorToColorConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is null) return null;
        var color = (SeverityColor)value;
        switch (color)
        {
            case SeverityColor.Red:
                return new SolidColorBrush(Colors.Red);
            case SeverityColor.Green:
                return new SolidColorBrush(Colors.Green);
            case SeverityColor.Blue:
                return new SolidColorBrush(Colors.DeepSkyBlue);
            case SeverityColor.Orange: 
                return new SolidColorBrush(Colors.Orange);
            case SeverityColor.White:
                return new SolidColorBrush(Colors.White);
            case SeverityColor.Yellow:
                return new SolidColorBrush(Colors.Gold);
            case SeverityColor.Black:
                return new SolidColorBrush(Colors.Black);
        }
        return new SolidColorBrush(Colors.Red);
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException();
    }
}