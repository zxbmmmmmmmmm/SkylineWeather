using CommunityToolkit.Mvvm.ComponentModel;
using FluentWeather.Abstraction.Models;
using FluentWeather.DIContainer;
using FluentWeather.Uwp.Helpers;
using FluentWeather.Uwp.Helpers.Analytics;
using Microsoft.Extensions.DependencyInjection;
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
namespace FluentWeather.Uwp.Controls.Dialogs;

[ObservableObject]
public sealed partial class HistoricalWeatherSetupDialog : ContentDialog
{


    public GeolocationBase Location
    {
        get => (GeolocationBase)GetValue(LocationProperty);
        set => SetValue(LocationProperty, value);
    }

    public static readonly DependencyProperty LocationProperty =
        DependencyProperty.Register(nameof(LocationProperty), typeof(int), typeof(HistoricalWeatherSetupDialog), new PropertyMetadata(default));


    public HistoricalWeatherSetupDialog(GeolocationBase location)
    {
        this.InitializeComponent();
        Location = location;
    }

    private async void GetHistoricalWeatherButton_Click(object sender, RoutedEventArgs e)
    {
        GetHistoricalWeatherButton.IsEnabled = false;
        DownloadProgressBar.ShowError = false;
        ProgressBarPanel.Visibility = Visibility.Visible;
        WarningInfoBar.Visibility = Visibility.Collapsed;
        DownloadProgressBar.Value = 0;
        ProgressText.Text = "HistoricalWeatherSetupProgress_Downloading".GetLocalized();
        try
        {
            await Task.Delay(500);

            var data = await HistoricalWeatherHelper.DownloadHistoricalWeatherAsync(Location.Location);

            DownloadProgressBar.Value = 50;
            ProgressText.Text = "HistoricalWeatherSetupProgress_Analysing".GetLocalized();
            await Task.Delay(1000);

            var result = await HistoricalWeatherHelper.AnalyseHistoricalWeatherAsync(data);
            var folder = await ApplicationData.Current.LocalFolder.GetOrCreateFolderAsync("HistoricalWeather");
            var folder1 = await folder.GetOrCreateFolderAsync(Location.GetHashCode().ToString());
            var dic = new Dictionary<string, Dictionary<string, HistoricalDailyWeatherBase>>();
            DownloadProgressBar.Value = 75;
            ProgressText.Text = "HistoricalWeatherSetupProgress_Saving".GetLocalized();

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
            foreach (var item in dic)
            {
                var file = await folder1.GetOrCreateFileAsync(item.Key);
                using var stream = await file.OpenStreamForWriteAsync();
                await JsonSerializer.SerializeAsync(stream, item.Value);
                stream.Close();
            }

            await Task.Delay(500);
            ProgressText.Text = "HistoricalWeatherSetupProgress_Done".GetLocalized();
            Locator.ServiceProvider.GetService<AppAnalyticsService>()?.TrackHistoricalWeatherDataDownloaded(Location.Name);

            GetHistoricalWeatherButton.Visibility = Visibility.Collapsed;
            RestartButton.Visibility = Visibility.Visible;
        }
        catch (Exception ex) 
        {
            DownloadProgressBar.ShowError = true;
            ProgressText.Text = ex.Message;
            throw;
        }
        finally
        {
            DownloadProgressBar.Value = 100;
            GetHistoricalWeatherButton.IsEnabled = true;
        }

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