using FluentWeather.Abstraction.Models;
using FluentWeather.Uwp.Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.Json;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
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
            GetHistoricalWeatherButton.IsEnabled = false;
            ProgressBarPanel.Visibility = Visibility.Visible;
            WarningInfoBar.Visibility = Visibility.Collapsed;
            DownloadProgressBar.Value = 0;

            ProgressText.Text = "下载原始数据...";
            await Task.Delay(500);

            var data = await HistoricalWeatherHelper.DownloadHistoricalWeatherAsync(Location);
            DownloadProgressBar.Value = 50;
            ProgressText.Text = "分析中...";
            var result = await HistoricalWeatherHelper.AnalyseHistoricalWeatherAsync(data);
            var folder = await ApplicationData.Current.LocalFolder.GetOrCreateFolderAsync("HistoricalWeather");
            var folder1 = await folder.GetOrCreateFolderAsync(Location.GetHashCode().ToString());
            var dic = new Dictionary<string, Dictionary<string,HistoricalDailyWeatherBase>>();
            await Task.Delay(1000);

            DownloadProgressBar.Value = 75;
            ProgressText.Text = "保存数据...";
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
            await Task.Delay(500);

            DownloadProgressBar.Value = 100;
            ProgressText.Text = "完成";
            GetHistoricalWeatherButton.Visibility = Visibility.Collapsed;
            RestartButton.Visibility = Visibility.Visible;
            GetHistoricalWeatherButton.IsEnabled = true;
        }

        private void HideButton_Click(object sender, RoutedEventArgs e)
        {
            Hide();
        }

        private async void RestartButton_Click(object sender, RoutedEventArgs e)
        {
            await CoreApplication.RequestRestartAsync(string.Empty);
        }
    }
}
