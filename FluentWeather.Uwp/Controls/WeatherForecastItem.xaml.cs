using FluentWeather.Abstraction.Models;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

//https://go.microsoft.com/fwlink/?LinkId=234236 上介绍了“用户控件”项模板

namespace FluentWeather.Uwp.Controls;

public sealed partial class WeatherForecastItem : UserControl
{
    public WeatherForecastItem()
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
        DependencyProperty.Register(nameof(WeatherInfo), typeof(WeatherBase), typeof(WeatherForecastItem), new PropertyMetadata(default));
}