using FluentWeather.Abstraction.Models;
using FluentWeather.Uwp.Controls.Dialogs;
using FluentWeather.Uwp.Shared;
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
    public sealed partial class HistoricalWeatherGauge : UserControl
    {
        public HistoricalWeatherGauge()
        {
            this.InitializeComponent();
        }

        public HistoricalDailyWeatherBase HistoricalDailyWeather
        {
            get => (HistoricalDailyWeatherBase)GetValue(HistoricalDailyWeatherProperty);
            set => SetValue(HistoricalDailyWeatherProperty, value);
        }

        public static readonly DependencyProperty HistoricalDailyWeatherProperty =
            DependencyProperty.Register(nameof(HistoricalDailyWeather), typeof(HistoricalDailyWeatherBase), typeof(HistoricalWeatherGauge), new PropertyMetadata(default));



        public WeatherDailyBase WeatherToday
        {
            get => (WeatherDailyBase)GetValue(WeatherTodayProperty);
            set => SetValue(WeatherTodayProperty, value);
        }

        public static readonly DependencyProperty WeatherTodayProperty =
            DependencyProperty.Register(nameof(WeatherToday), typeof(WeatherDailyBase), typeof(HistoricalWeatherGauge), new PropertyMetadata(default));

        private async void DownloadDataButton_Click(object sender, RoutedEventArgs e)
        {
            await DialogManager.OpenDialogAsync(new HistoricalWeatherSetupDialog(Common.Settings.DefaultGeolocation));
        }

        public bool IsHighTemperatureBreakRecord => WeatherToday?.MaxTemperature > HistoricalDailyWeather?.HistoricalMaxTemperature;
        public bool IsLowTemperatureBreakRecord => WeatherToday?.MinTemperature < HistoricalDailyWeather?.HistoricalMinTemperature;

        private int GetLength3High(int num1,int num2)
        {
            if (IsHighTemperatureBreakRecord) return 0;
            return num1 - num2;
        }

        private int GetLength3Low(int num1, int num2)
        {
            if (IsLowTemperatureBreakRecord) return 0;
            return num1 - num2;
        }

    }
}
