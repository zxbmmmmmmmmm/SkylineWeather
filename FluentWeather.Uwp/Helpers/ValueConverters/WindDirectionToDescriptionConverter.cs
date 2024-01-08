using FluentWeather.Abstraction.Models;
using System;
using Windows.UI.Xaml.Data;
using static FluentWeather.Abstraction.Models.WindDirection;

namespace FluentWeather.Uwp.Helpers.ValueConverters;

public class WindDirectionToDescriptionConverter:IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        var dir = (WindDirection)value;
        return dir switch
        {
            North => "北风",
            South => "南风",
            East => "东风",
            West => "西风",
            NorthWest => "西北风",
            NorthEast => "东北风",
            SouthWest => "西南风",
            SouthEast => "西北风",
            NorthNorthEast => "东北偏北风",
            EastNorthEast => "东北偏东风",
            EastSouthEast => "东南偏东风",
            SouthSouthEast => "东南偏南风",
            SouthSouthWest => "西南偏南风",
            WestSouthWest => "西南偏西风",
            WestNorthWest => "西北偏西风",
            NorthNorthWest => "西北偏北风",
            Rotational => "旋转风向",
            None => "无持续风向",
            _ => throw new NotImplementedException(),
        };
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException();
    }
}