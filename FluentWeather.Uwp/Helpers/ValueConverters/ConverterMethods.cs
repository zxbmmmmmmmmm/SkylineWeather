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
    public static string GetWindDirectionDescription(WindDirection dir)
    {
        return ResourceLoader.GetForCurrentView().GetString("WindDirection_" + dir);
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

}