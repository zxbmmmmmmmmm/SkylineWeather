using System;
using Windows.ApplicationModel.Resources;
using Windows.UI;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using FluentWeather.Abstraction.Helpers;
using FluentWeather.Abstraction.Models;
using static FluentWeather.Abstraction.Models.WeatherCode;

namespace FluentWeather.Uwp.Shared.Helpers.ValueConverters;

public static class ConverterMethods
{

    public static string GetWindScaleDescription(string scale)
    {
        if (scale.Contains("-"))
        {
            var s = scale.Split("-");
            scale = s[1];
        }
        return ResourceLoader.GetForCurrentView().GetString("WindScaleDescription_" + scale);
    }
    public static Brush SeverityColorToColor(SeverityColor? color)
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

    /// <summary>
    /// 根据应用设置自动转换温度
    /// 此转换仅在UI层进行
    /// </summary>
    /// <param name="temp">摄氏温度</param>
    /// <param name="disableRound">关闭取整</param>
    /// <returns></returns>
    public static double? ConvertTemperatureUnit(this double? temp,bool disableRound = false)
    {
        if (temp is null) return null;
        var result = Common.Settings.TemperatureUnit is TemperatureUnit.Fahrenheit ? temp.Value.ToFahrenheit():temp.Value;
        return disableRound ? result : Math.Round(result);
    }

    /// <summary>
    /// 根据应用设置自动转换温度
    /// 此转换仅在UI层进行
    /// </summary>
    /// <param name="temp">摄氏温度</param>
    /// <param name="disableRound">关闭取整</param>
    /// <returns></returns>
    public static int ConvertTemperatureUnit(this int temp)
    {
        var result = Common.Settings.TemperatureUnit is TemperatureUnit.Fahrenheit ? temp.ToFahrenheit() : temp;
        return result;
    }
    public static int ConvertTemperatureUnit(this double temp)
    {
        var result = Common.Settings.TemperatureUnit is TemperatureUnit.Fahrenheit ? temp.ToFahrenheit() : temp;
        return (int)Math.Round(result);
    }
    public static ImageSource GetIconByWeather(this WeatherCode code)
    {
        return new BitmapImage(GetIconUriByWeather(code));
    }

    public static Uri GetIconUriByWeather(this WeatherCode code)
    {
        var name = GetImageNameDay(code);
        name = name is "" ? GetImageName(code) : name;
        return new Uri("ms-appx:///Assets/Weather/" + name);
    }
    public static Uri GetIconUriByWeather(this WeatherCode weather, DateTime time)
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

        return new Uri("ms-appx:///Assets/Weather/" + imageName);
    }

    public static ImageSource GetIconByWeather(this WeatherCode weather, DateTime time)
    {
        return new BitmapImage(GetIconUriByWeather(weather, time));
    }

    public static string GetImageName(this WeatherCode weatherType)
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
            DenseFreezingDrizzle or LightFreezingDrizzle or LightFreezingRain or HeavyFreezingRain => "FreezingRain.png",
            SlightSleet or ModerateOrHeavySleet => "RainSnow.png",
            ThunderstormWithHeavyHail or SlightOrModerateThunderstorm or ThunderstormWithSlightHail or HeavyThunderStorm => "Thunder.png",
            _ => "Cloudy.png",
        };
    }

    public static string GetImageNameDay(this WeatherCode weatherType)
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
    public static string GetImageNameNight(this WeatherCode weatherType)
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

    public static int Subtract(this int num1,int num2)
    {
        return num1 - num2;
    }

    public static int Max(this int num1, int num2)
    {
        return Math.Max(num1, num2);
    }
    public static int Min(this int num1, int num2)
    {
        return Math.Min(num1, num2);
    }
}

