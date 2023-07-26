using FluentWeather.Abstraction.Models;
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

//https://go.microsoft.com/fwlink/?LinkId=234236 上介绍了“用户控件”项模板

namespace FluentWeather.Uwp.Controls;

public sealed partial class WeatherForecastItem : UserControl
{
    public WeatherForecastItem()
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
        DependencyProperty.Register(nameof(WeatherInfo), typeof(WeatherBase), typeof(WeatherForecastItem), new PropertyMetadata(default));
}