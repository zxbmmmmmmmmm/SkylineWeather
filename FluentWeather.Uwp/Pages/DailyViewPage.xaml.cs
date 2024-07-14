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

namespace FluentWeather.Uwp.Pages;

/// <summary>
/// 可用于自身或导航至 Frame 内部的空白页。
/// </summary>
[ObservableObject]
public sealed partial class DailyViewPage : Page
{
    public DailyViewPage()
    {
        this.InitializeComponent();
        PlaceholderBorder.Tapped += OnPlaceholderBorderTapped;
        CloseButton.Click += OnCloseButtonClicked;
    }

    private void OnCloseButtonClicked(object sender, RoutedEventArgs e)
    {
        CloseRequested?.Invoke(this, e);
    }

    private void OnPlaceholderBorderTapped(object sender, TappedRoutedEventArgs e)
    {
        CloseRequested?.Invoke(this, e);
    }

    public event RoutedEventHandler CloseRequested;


    public int SelectedIndex
    {
        get => (int)GetValue(SelectedIndexProperty);
        set => SetValue(SelectedIndexProperty, value);
    }

    public static readonly DependencyProperty SelectedIndexProperty =
        DependencyProperty.Register(nameof(SelectedIndex), typeof(int), typeof(DailyViewPage), new PropertyMetadata(0));


    public List<WeatherDailyBase> DailyForecasts
    {
        get => (List<WeatherDailyBase>)GetValue(DailyForecastsProperty);
        set => SetValue(DailyForecastsProperty, value);
    }

    public static readonly DependencyProperty DailyForecastsProperty =
        DependencyProperty.Register(nameof(DailyForecasts), typeof(List<WeatherBase>), typeof(DailyViewPage), new PropertyMetadata(default));

    public List<WeatherDailyBase> DailyForecasts7D
    {
        get => (List<WeatherDailyBase>)GetValue(DailyForecasts7DProperty);
        set => SetValue(DailyForecasts7DProperty, value);
    }

    public static readonly DependencyProperty DailyForecasts7DProperty =
        DependencyProperty.Register(nameof(DailyForecasts7D), typeof(List<WeatherBase>), typeof(DailyViewPage), new PropertyMetadata(default));

    public WeatherDailyBase SelectedDailyForecast
    {
        get => (WeatherDailyBase)GetValue(SelectedDailyForecastProperty);
        set => SetValue(SelectedDailyForecastProperty, value);
    }

    public static readonly DependencyProperty SelectedDailyForecastProperty =
        DependencyProperty.Register(nameof(SelectedDailyForecast), typeof(WeatherDailyBase), typeof(DailyViewPage), new PropertyMetadata(default));

}