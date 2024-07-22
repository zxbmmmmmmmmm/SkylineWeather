using FluentWeather.Abstraction.Models;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

//https://go.microsoft.com/fwlink/?LinkId=234236 上介绍了“用户控件”项模板

namespace FluentWeather.Uwp.Controls
{
    public sealed partial class WarningItem : UserControl
    {
        public WarningItem()
        {
            this.InitializeComponent();
        }


        private Visibility GetActionButtonsVisibility(WeatherWarningBase warning)
        {
            if (warning is null) return Visibility.Collapsed;
            return warning.Title.Contains("台风") ? Visibility.Visible : Visibility.Collapsed;
        }

        public WeatherWarningBase Warning
        {
            get => (WeatherWarningBase)GetValue(WarningProperty);
            set => SetValue(WarningProperty, value);
        }

        public static readonly DependencyProperty WarningProperty =
            DependencyProperty.Register(nameof(Warning), typeof(WeatherWarningBase), typeof(WarningItem), new PropertyMetadata(default));

    }
}
