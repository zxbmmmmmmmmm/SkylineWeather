using FluentWeather.Abstraction.Models;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

//https://go.microsoft.com/fwlink/?LinkId=234236 上介绍了“用户控件”项模板

namespace FluentWeather.Uwp.Controls
{
    public sealed partial class HourlyTemperatureChart : UserControl
    {
        private int ChartMaxTemperature => HourlyForecasts is not null && HourlyForecasts.Count != 0 ? HourlyForecasts.Max(p => p.Temperature)+1 : 45;

        private int ChartMinTemperature => HourlyForecasts is not null && HourlyForecasts.Count != 0 ? HourlyForecasts.Min(p => p.Temperature) -1 : 0;
        public HourlyTemperatureChart()
        {
            this.InitializeComponent();
        }
        public List<WeatherHourlyBase> HourlyForecasts
        {
            get => ((List<WeatherHourlyBase>)GetValue(HourlyForecastsProperty));
            set => SetValue(HourlyForecastsProperty, value);
        }


        public static readonly DependencyProperty HourlyForecastsProperty =
            DependencyProperty.Register(nameof(HourlyForecasts), typeof(List<WeatherHourlyBase>), typeof(HourlyTemperatureChart), new PropertyMetadata(default, OnPropertyChanged));
        public bool IsHorizontalAxisVisible
        {
            get => ((bool)GetValue(IsHorizontalAxisVisibleProperty));
            set => SetValue(IsHorizontalAxisVisibleProperty, value);
        }
        public static readonly DependencyProperty IsHorizontalAxisVisibleProperty =
            DependencyProperty.Register(nameof(IsHorizontalAxisVisible), typeof(bool), typeof(HourlyTemperatureChart), new PropertyMetadata(true, OnPropertyChanged));



        public int MajorTickInterval
        {
            get => (int)GetValue(MajorTickIntervalProperty);
            set => SetValue(MajorTickIntervalProperty, value);
        }

        public static readonly DependencyProperty MajorTickIntervalProperty =
            DependencyProperty.Register(nameof(MajorTickInterval), typeof(int), typeof(HourlyTemperatureChart), new PropertyMetadata(0));



        private static void OnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var chart = (HourlyTemperatureChart)d;
            chart.Bindings.Update();
        }
    }

}
