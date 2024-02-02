using System;
using System.Collections.Generic;
using Windows.UI.Xaml.Data;

namespace FluentWeather.Uwp.Helpers.ValueConverters;

public sealed class TypeConverter:IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
      
        if(value is List<object> list)
        {
            return list.ConvertAll(p => System.Convert.ChangeType(p, parameter.GetType()));
        }
        return System.Convert.ChangeType(value, parameter.GetType());
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException();
    }
}