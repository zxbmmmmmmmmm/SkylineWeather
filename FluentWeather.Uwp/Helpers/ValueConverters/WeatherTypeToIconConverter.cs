using System;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media.Imaging;
using FluentWeather.Abstraction.Models;
using FluentWeather.Abstraction.Interfaces.Weather;
using static FluentWeather.Abstraction.Models.WeatherType;
using System.Dynamic;

namespace FluentWeather.Uwp.Helpers.ValueConverters;

public class WeatherTypeToIconConverter:IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        var weatherType = (WeatherType)value;
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

    public static string GetImageName(WeatherType weatherType)
    {
        return weatherType switch
        {
            Hail => "BlowingHail.png",
            HeavyRain => "HeavyRain.png",
            ModerateRain => "LightRain.png",
            LightRain => "LightRain.png",
            ModerateShowers => "LightRain.png",
            Cloudy => "Cloudy.png",
            VeryCloudy => "VeryCloudy.png",
            LightSnow => "LightSnow.png",
            HeavySnow => "HeavySnow.png",
            Fog => "Fog.png",
            FreezingRain => "FreezingRain.png",           
            LightSleet or LightSleetShowers => "RainSnow.png",
            ThunderyHeavyRain or ThunderyShowers or ThunderySnowShowers => "Thundery.png",
            _ => "Cloudy.png",
        };
    }
    public static string GetImageNameDay(WeatherType weatherType)
    {
        return weatherType switch
        {
            Clear => "SunnyDay.png",
            HazeSmoke => "HazeSmokeDay.png",
            MostlyClear => "MostlySunnyDay.png",
            Hail => "HailDay.png",
            MostlyCloudy => "MostCloudyDay.png",
            PartlyCloudy => "PartlyCloudyDay.png",
            HeavyShowers or LightShowers => "RainShowersDay.png",
            LightSnowShowers or HeavySnowShowers => "SnowShowersDay.png",
            _ => "",
        };
    }
    public static string GetImageNameNight(WeatherType weatherType)
    {
        return weatherType switch
        {
            Clear => "ClearNight.png",
            HazeSmoke => "HazeSmokeNight.png",
            MostlyCloudy => "MostlyCloudyNight.png",
            MostlyClear => "MostlyClearNight.png",
            Hail => "HailNight.png",
            PartlyCloudy => "PartlyCloudyNight.png",
            HeavyShowers or LightShowers => "RainShowersNight.png",
            LightSnowShowers or HeavySnowShowers => "SnowShowersNight.png",
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