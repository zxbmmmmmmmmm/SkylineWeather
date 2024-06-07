using FluentWeather.Abstraction.Models;
using FluentWeather.Uwp.Helpers;
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

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“内容对话框”项模板

namespace FluentWeather.Uwp.Controls.Dialogs
{
    public sealed partial class HistoricalWeatherSetupDialog : ContentDialog
    {


        public Location Location
        {
            get => (Location)GetValue(LocationProperty);
            set => SetValue(LocationProperty, value);
        }

        public static readonly DependencyProperty LocationProperty =
            DependencyProperty.Register(nameof(LocationProperty), typeof(int), typeof(HistoricalWeatherSetupDialog), new PropertyMetadata(default));


        public HistoricalWeatherSetupDialog(Location location)
        {
            this.InitializeComponent();
            Location = location;
        }

        private async void GetHistoricalWeatherButton_Click(object sender, RoutedEventArgs e)
        {
            var data = await HistoricalWeatherHelper.DownloadHistoricalWeatherAsync(Location);
            var result = await HistoricalWeatherHelper.AnalyseHistoricalWeatherAsync(data);

        }
    }
}
