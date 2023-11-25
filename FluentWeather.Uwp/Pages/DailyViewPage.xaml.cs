using FluentWeather.Abstraction.Models;
using FluentWeather.Uwp.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using FluentWeather.Abstraction.Interfaces.Weather;
using FluentWeather.Uwp.Controls;
using CommunityToolkit.Mvvm.ComponentModel;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace FluentWeather.Uwp.Pages
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    [ObservableObject]
    public sealed partial class DailyViewPage : Page
    {
        public DailyViewPage()
        {
            this.InitializeComponent();
        }

        [ObservableProperty]
        private WeatherBase _selectedItem;


        public List<WeatherDailyBase> DailyForecasts
        {
            get => (List<WeatherDailyBase>)GetValue(DailyForecastsProperty);
            set => SetValue(DailyForecastsProperty, value);
        }

        public static readonly DependencyProperty DailyForecastsProperty =
            DependencyProperty.Register(nameof(DailyForecasts), typeof(List<WeatherBase>), typeof(DailyViewPage), new PropertyMetadata(null));

        public List<WeatherHourlyBase> HourlyForecasts
        {
            get => (List<WeatherHourlyBase>)GetValue(HourlyForecastsProperty);
            set => SetValue(HourlyForecastsProperty, value);
        }

        public static readonly DependencyProperty HourlyForecastsProperty =
            DependencyProperty.Register(nameof(HourlyForecasts), typeof(List<WeatherHourlyBase>), typeof(DailyViewPage), new PropertyMetadata(null));

    }
}
