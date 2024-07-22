using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FluentWeather.DIContainer;
using FluentWeather.Uwp.Helpers.Analytics;
using FluentWeather.Uwp.Shared;
using Microsoft.Extensions.DependencyInjection;
using Windows.UI.Xaml.Controls;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“内容对话框”项模板

namespace FluentWeather.Uwp.Controls.Dialogs;

[ObservableObject]
public sealed partial class AboutDialog : ContentDialog
{
    public AboutDialog()
    {
        this.InitializeComponent();
        Locator.ServiceProvider.GetService<AppAnalyticsService>()?.TrackAboutOpened();
    }
    [RelayCommand]
    public void Close()
    {
        Hide();
    }
    [RelayCommand]
    public void EnableDeveloperMode()
    {
        Locator.ServiceProvider.GetService<AppAnalyticsService>()?.TrackDeveloperModeEnabled();
        Common.Settings.DeveloperMode = true;
    }
}