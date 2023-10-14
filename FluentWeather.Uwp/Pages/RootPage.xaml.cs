using CommunityToolkit.Mvvm.ComponentModel;
using FluentWeather.Uwp.Shared;
using FluentWeather.Uwp.Themes;
using FluentWeather.Uwp.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace FluentWeather.Uwp.Pages;

/// <summary>
/// 可用于自身或导航至 Frame 内部的空白页。
/// </summary>
[ObservableObject]
public sealed partial class RootPage : Page
{
    public RootPageViewModel ViewModel { get; set; } = new();
    public static RootPage Instance { get; private set; }
    public RootPage()
    {
        this.InitializeComponent();
        Instance = this;
        this.Loaded += OnLoaded;
        SetTitleBar();
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        PaneFrame.Navigate(typeof(CitiesPage), Theme.GetSplitPaneNavigationTransition());
    }

    public void SetTitleBar()
    {
        var titleBar = ApplicationView.GetForCurrentView().TitleBar;
        var coreTitleBar = CoreApplication.GetCurrentView().TitleBar;
        Window.Current.SetTitleBar(Title);
        coreTitleBar.ExtendViewIntoTitleBar = true; titleBar.ForegroundColor = Windows.UI.Colors.Transparent;
        titleBar.BackgroundColor = Windows.UI.Colors.Transparent;
        titleBar.ButtonBackgroundColor = Windows.UI.Colors.Transparent;
        titleBar.InactiveBackgroundColor = Windows.UI.Colors.Transparent;
        titleBar.ButtonInactiveBackgroundColor = Windows.UI.Colors.Transparent;
        titleBar.ButtonHoverBackgroundColor = Color.FromArgb(40,128,128,128);
        ThemeHelper.SetTitleBarColor(Common.Settings.ApplicationTheme);
    }
    [ObservableProperty]
    public bool canGoBack;
    
    private void BackButton_Click(object sender, RoutedEventArgs e)
    {
        if (!CanGoBack) return;
        CanGoBack = PaneFrame.CanGoBack;
        PaneFrame.GoBack();
    }

    private void PaneFrame_Navigated(object sender, NavigationEventArgs e)
    {
        CanGoBack = PaneFrame.CanGoBack;
    }
}