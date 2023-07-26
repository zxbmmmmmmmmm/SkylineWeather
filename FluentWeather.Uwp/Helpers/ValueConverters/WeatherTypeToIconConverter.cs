using System;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media.Imaging;
using FluentWeather.Abstraction.Models;

namespace FluentWeather.Uwp.Helpers.ValueConverters;

public class WeatherTypeToIconConverter:IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        var weatherType = (WeatherType)value;
        var uri = new Uri("ms-appx:///Assets/Weather/Day/" + GetImageName(weatherType));
        return new BitmapImage(uri);
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException();
    }

    public string GetImageName(WeatherType weatherType)
    {
        switch (weatherType)
        {
            case WeatherType.Clear:
                return "Clear.png";
            case WeatherType.Hail:
                return "Hail.png";
            case WeatherType.PartlyCloudy:
                return "PartlyCloudy.png";
            case WeatherType.HeavyRain:
            case WeatherType.LightRain:
                return "Rain.png";
            case WeatherType.Cloudy:
            case WeatherType.VeryCloudy:
                return "Cloudy.png";
            case WeatherType.Fog:
                return "Fog.png";
            case WeatherType.HeavyShowers:
            case WeatherType.LightShowers:
                return "Showers.png";
            case WeatherType.LightSnow:
            case WeatherType.HeavySnow:
            case WeatherType.LightSnowShowers:
            case WeatherType.HeavySnowShowers:
                return "Snow.png";
            case WeatherType.LightSleet:
            case WeatherType.LightSleetShowers:
                return "Sleet.png";
            case WeatherType.ThunderyHeavyRain:
            case WeatherType.ThunderyShowers:
            case WeatherType.ThunderySnowShowers:
                return "Thundery.png";
            default:
                return "PartlyCloudy.png";
        }
    }
}