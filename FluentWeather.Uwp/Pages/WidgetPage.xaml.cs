using FluentWeather.Uwp.Shared;
using FluentWeather.Uwp.ViewModels;
using Microsoft.Gaming.XboxGameBar;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;


namespace FluentWeather.Uwp.Pages;

public sealed partial class WidgetPage : Page
{
    public MainPageViewModel ViewModel { get; set; } = new();
    private readonly DispatcherTimer _timer = new() { Interval = TimeSpan.FromMinutes(20) };
    private XboxGameBarWidget _widget;

    public WidgetPage()
    {
        this.InitializeComponent();
    }
    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        base.OnNavigatedTo(e);
        ViewModel.CurrentGeolocation = Common.Settings.DefaultGeolocation!;
        _widget = e.Parameter as XboxGameBarWidget;
        _timer.Tick += OnTimerTicked;
        _timer.Start();
    }

    private async void OnTimerTicked(object sender, object e)
    {
        await ViewModel.RefreshCommand.ExecuteAsync(null);
    }

    private async void WeatherButton_Click(object sender, RoutedEventArgs e)
    {
        await _widget.LaunchUriAsync(new Uri("weather:"));
    }
}