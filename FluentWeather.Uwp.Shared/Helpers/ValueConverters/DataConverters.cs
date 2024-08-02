using System;
using Windows.UI.Xaml.Data;
using FluentWeather.Abstraction.Interfaces.Weather;
using FluentWeather.Abstraction.Models;

namespace FluentWeather.Uwp.Shared.Helpers.ValueConverters;

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
        var result = (DateTime?)value;
        return value is null ? null : result.Value.ToShortTimeString();
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException();
    }
}

public class WeatherTypeToIconConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        return ((WeatherCode)value).GetIconByWeather();
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException();
    }
}

public class WeatherToIconConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        return ((WeatherBase)value).WeatherType.GetIconByWeather(((ITime)value).Time);
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException();
    }
}



public class TemperatureUnitConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        double? result = null;
        if(value is int num)
        {
            result = num;
        }
        else if(value is string str)
        {
            result = double.Parse(str);
        }
        var round = parameter is true or "true";
        return result.ConvertTemperatureUnit(round);
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException();
    }
}