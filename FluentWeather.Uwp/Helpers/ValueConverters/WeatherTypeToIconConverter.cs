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
public class WeatherFontIconConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        var weatherType = (WeatherType)value;
        return GetGlyph(weatherType);
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException();
    }

    public string GetGlyph(WeatherType weatherType)
    {
        switch (weatherType)
        {
            case WeatherType.Clear:
                return "\uE9BD";
            case WeatherType.Hail:
                return "\uEA02";
            case WeatherType.PartlyCloudy:
                return "\uE9C0";
            case WeatherType.HeavyRain:
                return "\uEA0D";
            case WeatherType.LightRain:
                return "\uE9E2";
            case WeatherType.Cloudy:
                return "\uE9BE";
            case WeatherType.VeryCloudy:
                return "\uE9BF";
            case WeatherType.Fog:
                return "Fog.png";
            case WeatherType.HeavyShowers:
            case WeatherType.LightShowers:
                return "\uEA0D";
            case WeatherType.LightSnow:
                return "\uEA02";
            case WeatherType.HeavySnow:
            case WeatherType.LightSnowShowers:
            case WeatherType.HeavySnowShowers:
                return "\uEA05";
            case WeatherType.LightSleet:
            case WeatherType.LightSleetShowers:
                return "\uEA12";
            case WeatherType.ThunderyHeavyRain:
            case WeatherType.ThunderyShowers:
            case WeatherType.ThunderySnowShowers:
                return "\uEA07";
            default:
                return "\uE9C0";
        }
    }
}
