using FluentWeather.Abstraction.Interfaces.Weather;
using FluentWeather.Abstraction.Models;
using Newtonsoft.Json.Linq;
using System;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using static FluentWeather.Abstraction.Models.WeatherCode;

namespace FluentWeather.Uwp.Themes;

public sealed partial class Generic:ResourceDictionary
{
    public Generic()
    {
        this.InitializeComponent();
    }
    public static Visibility GetPrecipVisibility(int? precip)
    {
        return (precip >= 10) ? Visibility.Visible : Visibility.Collapsed;
    }

}