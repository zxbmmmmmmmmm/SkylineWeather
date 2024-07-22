using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using FluentWeather.Abstraction.Models;

//https://go.microsoft.com/fwlink/?LinkId=234236 上介绍了“用户控件”项模板

namespace FluentWeather.Uwp.Controls;

public sealed partial class WeatherHourlyForecastItem : UserControl
{
    public WeatherHourlyForecastItem()
    {
        this.InitializeComponent();
        DataContext = this;
        this.DataContextChanged += (s, e) => Bindings.Update();
    }
    public WeatherBase WeatherInfo
    {
        get => (WeatherBase)GetValue(WeatherInfoProperty);
        set => SetValue(WeatherInfoProperty, value);
    }

    public static readonly DependencyProperty WeatherInfoProperty =
        DependencyProperty.Register(nameof(WeatherInfo), typeof(WeatherBase), typeof(WeatherHourlyForecastItem), new PropertyMetadata(default));

}