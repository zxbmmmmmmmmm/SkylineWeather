using Windows.ApplicationModel.Core;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using FluentWeather.Uwp.Shared;
using FluentWeather.Uwp.ViewModels;
using System.Globalization;
using FluentWeather.Uwp.Shared.Helpers;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace FluentWeather.Uwp.Pages;

/// <summary>
/// 可用于自身或导航至 Frame 内部的空白页。
/// </summary>
public sealed partial class WelcomePage : Page
{
    public WelcomePageViewModel ViewModel { get; } = new();
    public WelcomePage()
    {
        this.InitializeComponent();
        SetTitleBar();
        if(CultureInfo.CurrentCulture.Name is "zh-CN")
        {
            WelcomeSuggestion.Visibility = Visibility.Visible;
        }
    }
    public void SetTitleBar()
    {
        var titleBar = ApplicationView.GetForCurrentView().TitleBar;
        var coreTitleBar = CoreApplication.GetCurrentView().TitleBar;
        Window.Current.SetTitleBar(Title);
        coreTitleBar.ExtendViewIntoTitleBar = true;
        titleBar.ForegroundColor = Windows.UI.Colors.Transparent;
        titleBar.BackgroundColor = Windows.UI.Colors.Transparent;
        titleBar.ButtonBackgroundColor = Windows.UI.Colors.Transparent;
        titleBar.InactiveBackgroundColor = Windows.UI.Colors.Transparent;
        titleBar.ButtonInactiveBackgroundColor = Windows.UI.Colors.Transparent;
        titleBar.ButtonHoverBackgroundColor = Color.FromArgb(40, 128, 128, 128);
        ThemeHelper.SetTitleBarColor(Common.Settings.ApplicationTheme);
    }
}