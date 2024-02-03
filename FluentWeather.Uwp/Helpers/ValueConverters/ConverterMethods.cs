using System;
using Windows.UI;
using Telerik.Charting;
using Telerik.Core;
using Telerik.UI.Xaml.Controls.Chart;
using Windows.UI.Xaml.Media;
using FluentWeather.Abstraction.Models;
using Windows.Devices.Geolocation;
using Newtonsoft.Json.Linq;

namespace FluentWeather.Uwp.Helpers.ValueConverters;

public static class ConverterMethods
{
    public static Brush DataPointToBrush(IElementPresenter presenter)
    {
        var series = presenter as CategoricalStrokedSeries;
        return series?.Stroke;
    }


}