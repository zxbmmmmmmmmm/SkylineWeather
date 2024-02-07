using System;
using Windows.UI;
using Telerik.Charting;
using Telerik.Core;
using Telerik.UI.Xaml.Controls.Chart;
using Windows.UI.Xaml.Media;
using FluentWeather.Abstraction.Models;
using Windows.Devices.Geolocation;
using Newtonsoft.Json.Linq;
using Windows.ApplicationModel.Resources;

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

}

