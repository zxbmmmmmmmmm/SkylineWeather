using FluentWeather.Abstraction.Interfaces.Weather;
using FluentWeather.Abstraction.Models;
using FluentWeather.Uwp.Helpers.ValueConverters;
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

    public static ImageSource GetIconByWeather(WeatherCode code)
    {
        var name = GetImageNameDay(code);
        name = name is "" ? GetImageName(code) : name;
        return new BitmapImage(new Uri("ms-appx:///Assets/Weather/" + name));
    }
    public static ImageSource GetIconByWeather(WeatherCode weather,DateTime time)
    {
        var imageName = "";
        if (time.TimeOfDay >= TimeSpan.FromHours(6) && time.TimeOfDay <= TimeSpan.FromHours(18))
        {
            imageName = GetImageNameDay(weather);
        }
        else
        {
            imageName = GetImageNameNight(weather);
        }
        imageName = imageName is "" ? GetImageName(weather) : imageName;

        var uri = new Uri("ms-appx:///Assets/Weather/" + imageName);
        return new BitmapImage(uri);
    }

    public static string GetImageName(WeatherCode weatherType)
    {
        return weatherType switch
        {
            SlightHail or ModerateOrHeavyHail => "BlowingHail.png",
            HeavyRain => "HeavyRain.png",
            ModerateRain => "LightRain.png",
            SlightRain => "LightRain.png",
            ModerateRainShowers => "LightRain.png",
            PartlyCloudy => "VeryCloudy.png",
            Overcast => "Cloudy.png",
            SlightSnowFall or ModerateSnowFall => "LightSnow.png",
            HeavySnowFall => "HeavySnow.png",
            Fog => "Fog.png",
            LightFreezingRain or HeavyFreezingRain => "FreezingRain.png",
            SlightSleet or ModerateOrHeavySleet => "RainSnow.png",
            ThunderstormWithHeavyHail or SlightOrModerateThunderstorm or ThunderstormWithSlightHail or HeavyThunderStorm => "Thunder.png",
            _ => "Cloudy.png",
        };
    }

    public static string GetImageNameDay(WeatherCode weatherType)
    {
        return weatherType switch
        {
            WeatherCode.Clear => "SunnyDay.png",
            Haze or Mist => "HazeSmokeDay.png",
            MainlyClear => "MostlySunnyDay.png",
            SlightHail or ModerateOrHeavyHail => "HailDay.png",
            PartlyCloudy => "PartlyCloudyDay.png",
            SlightRainShowers or ModerateRainShowers or ViolentRainShowers => "RainShowersDay.png",
            SlightSnowShowers or HeavySnowShowers => "SnowShowersDay.png",
            _ => "",
        };
    }
    public static string GetImageNameNight(WeatherCode weatherType)
    {
        return weatherType switch
        {
            WeatherCode.Clear => "ClearNight.png",
            Haze => "HazeSmokeNight.png",
            MainlyClear => "MostlyClearNight.png",
            SlightHail or ModerateOrHeavyHail => "HailNight.png",
            PartlyCloudy => "PartlyCloudyNight.png",
            SlightRainShowers or ModerateRainShowers or ViolentRainShowers => "RainShowersNight.png",
            SlightSnowShowers or HeavySnowShowers => "SnowShowersNight.png",
            _ => "",
        };
    }
}