using System;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media.Imaging;
using FluentWeather.Abstraction.Models;
using FluentWeather.Abstraction.Interfaces.Weather;
using static FluentWeather.Abstraction.Models.WeatherCode;
using System.Dynamic;

namespace FluentWeather.Uwp.Helpers.ValueConverters;

public class WeatherTypeToIconConverter:IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        var weatherType = (WeatherCode)value;
        var name = GetImageNameDay(weatherType);
        name = name is "" ? GetImageName(weatherType): name;
        var uri = new Uri("ms-appx:///Assets/Weather/" + name);
        var img = new BitmapImage(uri);

        return img;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException();
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
            ThunderstormWithHeavyHail or SlightOrModerateThunderstorm or ThunderstormWithSlightHail or HeavyThunderStorm => "Thundery.png",
            _ => "Cloudy.png",
        };
    }
    public static string GetImageNameDay(WeatherCode weatherType)
    {
        return weatherType switch
        {
            Clear => "SunnyDay.png",
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
            Clear => "ClearNight.png",
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

public class WeatherToIconConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is null) return null;
        var weather = (WeatherBase)value;
        var time = ((ITime)weather).Time;
        var baseUri = "ms-appx:///Assets/Weather/";
        var imageName = "";
        if (time.TimeOfDay >= TimeSpan.FromHours(6) && time.TimeOfDay <= TimeSpan.FromHours(18))
        {
            imageName = WeatherTypeToIconConverter.GetImageNameDay(weather.WeatherType);
        }
        else
        {
            imageName = WeatherTypeToIconConverter.GetImageNameNight(weather.WeatherType);
        }
        imageName = imageName is "" ? WeatherTypeToIconConverter.GetImageName(weather.WeatherType) : imageName;

        var uri = new Uri(baseUri + imageName);
        return new BitmapImage(uri);
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException();
    }

}