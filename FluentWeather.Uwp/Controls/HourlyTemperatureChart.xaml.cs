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
        private int MaxTemperature => WeatherForecasts.Count != 0 ? WeatherForecasts.Max(p => p.Temperature) : 45;

        private int MinTemperature => WeatherForecasts.Count != 0 ? WeatherForecasts.Min(p => p.Temperature) : 0;
        public HourlyTemperatureChart()
        {
            this.InitializeComponent();
        }
        public List<ITemperature> WeatherForecasts
        {
            get => ((List<WeatherBase>)GetValue(WeatherForecastsProperty))?.ConvertAll(p => (ITemperature)p);
            set => SetValue(WeatherForecastsProperty, value?.ConvertAll(p => (WeatherBase)p));
        }


        public static readonly DependencyProperty WeatherForecastsProperty =
            DependencyProperty.Register(nameof(WeatherForecasts), typeof(List<WeatherBase>), typeof(HourlyTemperatureChart), new PropertyMetadata(default, OnPropertyChanged));

        private static void OnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var chart = (HourlyTemperatureChart)d;
            chart.Bindings.Update();
        }
    }
}
