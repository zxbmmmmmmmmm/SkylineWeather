using FluentWeather.Abstraction.Interfaces.Weather;
using FluentWeather.Abstraction.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI.Xaml.Data;

namespace FluentWeather.Uwp.Helpers.ValueConverters;

public sealed class HourlyForecastSelectConverter:IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is not ObservableCollection<WeatherBase> weatherList) return null;
        if (parameter is not ITime selected) return null;
        var list = new List<ITime>();
        foreach(var item in weatherList)
        {
            list.Add(item as ITime);
        }
        return list.Select(p => p.Time.Date == selected.Time.Date).ToList();
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException();
    }
}