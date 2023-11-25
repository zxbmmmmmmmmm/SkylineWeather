using FluentWeather.Abstraction.Interfaces.Weather;
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

namespace FluentWeather.Uwp.Controls
{
    public sealed partial class HourlyTemperatureChart : UserControl
    {
        private int MaxTemperature => HourlyForecasts is not null && HourlyForecasts.Count != 0 ? HourlyForecasts.Max(p => p.Temperature) : 45;

        private int MinTemperature => HourlyForecasts is not null && HourlyForecasts.Count != 0 ? HourlyForecasts.Min(p => p.Temperature) : 0;
        public HourlyTemperatureChart()
        {
            this.InitializeComponent();
        }
        public List<WeatherHourlyBase> HourlyForecasts
        {
            get => ((List<WeatherHourlyBase>)GetValue(WeatherForecastsProperty));
            set => SetValue(WeatherForecastsProperty, value);
        }


        public static readonly DependencyProperty WeatherForecastsProperty =
            DependencyProperty.Register(nameof(HourlyForecasts), typeof(List<WeatherHourlyBase>), typeof(HourlyTemperatureChart), new PropertyMetadata(default, OnPropertyChanged));

        private static void OnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var chart = (HourlyTemperatureChart)d;
            chart.Bindings.Update();
        }
    }
}
