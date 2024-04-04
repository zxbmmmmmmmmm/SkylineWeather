using FluentWeather.Abstraction.Interfaces.Weather;
using FluentWeather.Abstraction.Models;
using System;
using System.Collections;
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

    public List<WeatherDailyBase> WeatherForecasts
    {
        get => (List<WeatherDailyBase>)GetValue(WeatherForecastsProperty);
        set => SetValue(WeatherForecastsProperty, value);
    }
    public static readonly DependencyProperty WeatherForecastsProperty =
        DependencyProperty.Register(nameof(WeatherForecasts), typeof(List<WeatherDailyBase>), typeof(TemperatureChart), new PropertyMetadata(default,OnPropertyChanged));

    private static void OnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var chart = (TemperatureChart)d;
        chart.Bindings.Update();   
    }
    public (List<CategoricalDataPoint> tempMax, List<CategoricalDataPoint> tempMin) GetDataPoints(IList<WeatherDailyBase> weatherDailyForecasts)
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
    /// <summary>
    /// 获取降温数据
    /// </summary>
    /// <param name="weatherForecasts"></param>
    /// <returns></returns>
    public List<WeatherDailyBase> GetCoolingData(IList<WeatherDailyBase> weatherForecasts)
    {
        var list = new List<WeatherDailyBase>();
        for (var i = 0; i < weatherForecasts.Count-1; i++)
        {
            if (weatherForecasts[i+1].MaxTemperature - weatherForecasts[i].MaxTemperature <= -5)
            {
                if(!list.Contains(weatherForecasts[i]))
                    list.Add(weatherForecasts[i]);
                if (!list.Contains(weatherForecasts[i+1]))
                    list.Add(weatherForecasts[i + 1]);
                break;
            }

        }
        return list;
    }
    public List<WeatherDailyBase> GetHeatingData(IList<WeatherDailyBase> weatherForecasts)
    {
        var list = new List<WeatherDailyBase>();
        for (var i = 0; i < weatherForecasts.Count - 1; i++)
        {
            if (weatherForecasts[i + 1].MaxTemperature - weatherForecasts[i].MaxTemperature >= 5)
            {
                if (!list.Contains(weatherForecasts[i]))
                    list.Add(weatherForecasts[i]);
                if (!list.Contains(weatherForecasts[i + 1]))
                    list.Add(weatherForecasts[i + 1]);
                break;
            }
        }
        return list;
    }
}