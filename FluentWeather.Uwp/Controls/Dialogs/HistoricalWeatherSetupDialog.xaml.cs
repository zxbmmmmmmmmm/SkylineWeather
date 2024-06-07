using FluentWeather.Abstraction.Models;
using FluentWeather.Uwp.Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.Json;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
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
            var folder = await ApplicationData.Current.LocalFolder.GetOrCreateFolderAsync("HistoricalWeather");
            var folder1 = await folder.GetOrCreateFolderAsync(Location.GetHashCode().ToString());
            var dic = new Dictionary<string, Dictionary<string,HistoricalDailyWeatherBase>>();
            foreach (var pair in result)
            {
                var month = pair.Key.AsSpan().Slice(0, 2).ToString();
                var day = pair.Value.Date.Day.ToString();
                if (!dic.ContainsKey(month)) 
                {
                    dic[month] = [];
                }

                dic[month][day] = pair.Value;
            }
            foreach(var item in dic)
            {
                var file = await folder1.GetOrCreateFileAsync(item.Key);
                using var stream = await file.OpenStreamForWriteAsync();
                await JsonSerializer.SerializeAsync(stream, item.Value);
                stream.Close();
            }
        }
    }
}
