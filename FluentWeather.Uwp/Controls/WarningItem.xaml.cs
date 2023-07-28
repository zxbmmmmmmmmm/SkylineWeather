using FluentWeather.Abstraction.Models;
using System;
using System.Collections.Generic;
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

//https://go.microsoft.com/fwlink/?LinkId=234236 上介绍了“用户控件”项模板

namespace FluentWeather.Uwp.Controls
{
    public sealed partial class WarningItem : UserControl
    {
        public WarningItem()
        {
            this.InitializeComponent();
        }




        public WeatherWarningBase Warning
        {
            get => (WeatherWarningBase)GetValue(WarningProperty);
            set => SetValue(WarningProperty, value);
        }

        // Using a DependencyProperty as the backing store for Warning.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty WarningProperty =
            DependencyProperty.Register(nameof(Warning), typeof(WeatherWarningBase), typeof(WarningItem), new PropertyMetadata(default));



    }
}
