﻿using System;
using Windows.UI;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using FluentWeather.Abstraction.Models;
using Windows.ApplicationModel.Resources;
using FluentWeather.Uwp.Themes;
using FluentWeather.Abstraction.Interfaces.Weather;

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
        var weather = value as WeatherBase;
        return weather?.WeatherType.GetIconByWeather(((ITime)weather).Time);
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
        return ConverterMethods.TemperatureUnitConvert(result, round);
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException();
    }
}