using FluentWeather.Abstraction.Models;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

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

    private bool CanMoveNext => SelectedIndex <= DailyForecasts.Count - 2;
    private bool CanMovePrevious => SelectedIndex >= 1;

    public int FirstColumn => DailyForecasts.Count > 0 ? (int)DailyForecasts.First().Time.DayOfWeek : 0;

    public int SelectedIndex
    {
        get => (int)GetValue(SelectedIndexProperty);
        set => SetValue(SelectedIndexProperty, value);
    }

    public static readonly DependencyProperty SelectedIndexProperty =
        DependencyProperty.Register(nameof(SelectedIndex), typeof(int), typeof(DailyViewPage), new PropertyMetadata(0, SelectedIndexChanged));

    private int PivotSelectedIndex 
    {
        get => Math.Clamp(SelectedIndex,0,6);
        set => SelectedIndex = Math.Clamp(value, 0, 6);
    }

    public WeatherDailyBase SelectedDailyForecast => DailyForecasts?[SelectedIndex];

    private static void SelectedIndexChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var page = (DailyViewPage)d;
        page.OnPropertyChanged(nameof(CanMovePrevious));
        page.OnPropertyChanged(nameof(CanMoveNext));
        page.OnPropertyChanged(nameof(SelectedDailyForecast));
        page.OnPropertyChanged(nameof(PivotSelectedIndex));
        
    }
    [RelayCommand]
    private void MoveNext()
    {
        SelectedIndex += 1;
    }
    [RelayCommand]
    private void MovePrevious()
    {
        SelectedIndex -= 1;
    }


    private static void DailyForecastsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var page = (DailyViewPage)d;
        page.OnPropertyChanged(nameof(FirstColumn));
    }

    public List<WeatherDailyBase> DailyForecasts
    {
        get => (List<WeatherDailyBase>)GetValue(DailyForecastsProperty);
        set => SetValue(DailyForecastsProperty, value);
    }

    public static readonly DependencyProperty DailyForecastsProperty =
        DependencyProperty.Register(nameof(DailyForecasts), typeof(List<WeatherBase>), typeof(DailyViewPage), new PropertyMetadata(default, DailyForecastsChanged));

    public List<WeatherDailyBase> DailyForecasts7D
    {
        get => (List<WeatherDailyBase>)GetValue(DailyForecasts7DProperty);
        set => SetValue(DailyForecasts7DProperty, value);
    }

    public static readonly DependencyProperty DailyForecasts7DProperty =
        DependencyProperty.Register(nameof(DailyForecasts7D), typeof(List<WeatherBase>), typeof(DailyViewPage), new PropertyMetadata(default));

    public DailyViewMode Mode
    {
        get => (DailyViewMode)GetValue(ModeProperty);
        set => SetValue(ModeProperty, value);
    }

    public static readonly DependencyProperty ModeProperty =
        DependencyProperty.Register(nameof(Mode), typeof(DailyViewMode), typeof(DailyViewPage), new PropertyMetadata(DailyViewMode.Daily));
}
public enum DailyViewMode
{
    Daily,
    Weekly,
    Monthly
}