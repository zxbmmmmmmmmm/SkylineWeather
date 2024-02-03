using FluentWeather.Abstraction.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

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
        private static Brush SeverityColorToColor(SeverityColor? color)
        {
            return color switch
            {
                SeverityColor.Red => new SolidColorBrush(Colors.Red),
                SeverityColor.Green => new SolidColorBrush(Colors.Green),
                SeverityColor.Blue => new SolidColorBrush(Colors.DeepSkyBlue),
                SeverityColor.Orange => new SolidColorBrush(Colors.Orange),
                SeverityColor.White => new SolidColorBrush(Colors.White),
                SeverityColor.Yellow => new SolidColorBrush(Colors.Gold),
                SeverityColor.Black => new SolidColorBrush(Colors.Black),
                _ => new SolidColorBrush(Colors.Red)
            };
        }

    }
}
