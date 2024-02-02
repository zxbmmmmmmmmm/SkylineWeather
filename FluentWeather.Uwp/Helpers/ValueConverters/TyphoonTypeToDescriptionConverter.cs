using FluentWeather.Abstraction.Models;
using System;
using Windows.UI.Xaml.Data;

namespace FluentWeather.Uwp.Helpers.ValueConverters;

public sealed class TyphoonTypeToDescriptionConverter:IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if(value is TyphoonType type)
        {
            switch(type)
            {
                case TyphoonType.TD:
                    return "热带气压";
                case TyphoonType.TS:
                    return "热带风暴";
                case TyphoonType.STS:
                    return "强热带风暴";
                case TyphoonType.TY:
                    return "台风";
                case TyphoonType.STY:
                    return "强台风";
                case TyphoonType.SuperTY:
                    return "超强台风";
            }
        }
        return null;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException();
    }
}