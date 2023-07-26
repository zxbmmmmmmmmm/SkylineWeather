using FluentWeather.Abstraction.Interfaces.Weather;
using FluentWeather.Abstraction.Models;
using FluentWeather.QWeatherProvider.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Telerik.Charting;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

//https://go.microsoft.com/fwlink/?LinkId=234236 上介绍了“用户控件”项模板

namespace FluentWeather.Uwp.Controls;

public sealed partial class TemperatureChart : UserControl
{
    public TemperatureChart()
    {
        this.InitializeComponent();
        this.DataContextChanged += (s, e) => Bindings.Update();
    }
    private int MaxTemperature => WeatherForecasts.Count != 0 ? WeatherForecasts.Max(p => p.MaxTemperature) : 45;

    private int MinTemperature => WeatherForecasts.Count != 0 ? WeatherForecasts.Min(p => p.MinTemperature) : 0;


    public List<ITemperatureRange> WeatherForecasts
    {
        get => ((List<WeatherBase>)GetValue(WeatherForecastsProperty)).ConvertAll(p => (ITemperatureRange)p);
        set => SetValue(WeatherForecastsProperty, value.ConvertAll(p => (WeatherBase)p));
    }
    public static readonly DependencyProperty WeatherForecastsProperty =
        DependencyProperty.Register(nameof(WeatherForecasts), typeof(List<WeatherBase>), typeof(TemperatureChart), new PropertyMetadata(default));



    public (List<CategoricalDataPoint> tempMax, List<CategoricalDataPoint> tempMin) GetDataPoints(IList<ITemperatureRange> weatherDailyForecasts)
    {
        var tempMax = new List<CategoricalDataPoint>();
        var tempMin = new List<CategoricalDataPoint>();

        foreach (var item in weatherDailyForecasts)
        {
            tempMax.Add(new CategoricalDataPoint { Category = item.GetHashCode(), Value = item.MaxTemperature });
            tempMin.Add(new CategoricalDataPoint { Category = item.GetHashCode(), Value = item.MinTemperature });
        }
        return (tempMax, tempMin);
    }
}