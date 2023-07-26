using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using FluentWeather.Abstraction.Models;

//https://go.microsoft.com/fwlink/?LinkId=234236 上介绍了“用户控件”项模板

namespace FluentWeather.Uwp.Controls;

public sealed partial class WeatherHourlyForecastItem : UserControl
{
    public WeatherHourlyForecastItem()
    {
        this.InitializeComponent();
        DataContext = this;
    }
    public WeatherBase WeatherInfo
    {
        get => (WeatherBase)GetValue(WeatherInfoProperty);
        set => SetValue(WeatherInfoProperty, value);
    }

    public static readonly DependencyProperty WeatherInfoProperty =
        DependencyProperty.Register(nameof(WeatherInfo), typeof(WeatherBase), typeof(WeatherHourlyForecastItem), new PropertyMetadata(default));

}