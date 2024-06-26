﻿using Windows.UI;
using Telerik.Core;
using Telerik.UI.Xaml.Controls.Chart;
using Windows.UI.Xaml.Media;
using FluentWeather.Abstraction.Models;
using Windows.ApplicationModel.Resources;
using FluentWeather.Abstraction.Helpers;
using FluentWeather.Uwp.Shared;

namespace FluentWeather.Uwp.Helpers.ValueConverters;

public static class ConverterMethods
{
    public static Brush DataPointToBrush(IElementPresenter presenter)
    {
        var series = presenter as CategoricalStrokedSeries;
        return series?.Stroke;
    }
    public static string GetWindScaleDescription(string scale)
    {
        if (scale.Contains("-"))
        {
            var s = scale.Split("-");
            scale = s[1];
        }
        return ResourceLoader.GetForCurrentView().GetString("WindScaleDescription_" + scale);
    }
    public static Brush SeverityColorToColor(SeverityColor? color)
    {
        return color switch
        {
            SeverityColor.Red => new SolidColorBrush(Colors.Red),
            SeverityColor.Green => new SolidColorBrush(Colors.Green),
            SeverityColor.Blue => new SolidColorBrush(Colors.DeepSkyBlue),
            SeverityColor.Orange => new SolidColorBrush(Colors.Orange),
            SeverityColor.White => new SolidColorBrush(Colors.White),
            SeverityColor.Yellow => new SolidColorBrush(Colors.Gold),
            SeverityColor.Black => new SolidColorBrush(Colors.Black),
            _ => new SolidColorBrush(Colors.Red)
        };
    }

    /// <summary>
    /// 根据应用设置自动转换温度
    /// 此转换仅在UI层进行
    /// </summary>
    /// <param name="temp">摄氏温度</param>
    /// <param name="disableRound">关闭取整</param>
    /// <returns></returns>
    public static double? TemperatureUnitConvert(double? temp,bool disableRound = false)
    {
        if (temp is null) return null;
        var result = Common.Settings.TemperatureUnit is TemperatureUnit.Fahrenheit ? temp.Value.ToFahrenheit():temp.Value;
        return disableRound ? result : Math.Round(result);
    }

    public static double ToDouble(int value)
    {
        return value;
    }
}

