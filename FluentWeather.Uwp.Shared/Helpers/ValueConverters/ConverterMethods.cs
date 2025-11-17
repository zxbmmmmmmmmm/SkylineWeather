using System;
using Windows.ApplicationModel.Resources;
using Windows.UI;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using FluentWeather.Abstraction.Helpers;
using FluentWeather.Abstraction.Models;
using static FluentWeather.Abstraction.Models.WeatherCode;
using Windows.UI.Xaml;

namespace FluentWeather.Uwp.Shared.Helpers.ValueConverters;

public static class ConverterMethods
{
    private static readonly bool IsFahrenheit = Common.Settings.TemperatureUnit is TemperatureUnit.Fahrenheit;
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
    public static Brush AqiLevelToColor(int? level)
    {
        return level switch
        {
            0 => new SolidColorBrush(Colors.LawnGreen),
            1 => new SolidColorBrush(Colors.Yellow),
            2 => new SolidColorBrush(Colors.Orange),
            3 => new SolidColorBrush(Colors.Red),
            4 => new SolidColorBrush(Colors.Purple),
            5 => new SolidColorBrush(Colors.DarkRed),
            _ => new SolidColorBrush(Colors.Transparent)
        };
    }
    extension(double? temp)
    {
        /// <summary>
        /// 根据应用设置自动转换温度
        /// 此转换仅在UI层进行
        /// </summary>
        /// <param name="temp">摄氏温度</param>
        /// <param name="disableRound">关闭取整</param>
        /// <returns></returns>
        public double? ConvertTemperatureUnit(bool disableRound = false)
        {
            if (temp is null) return null;
            var result = IsFahrenheit ? temp.Value.ToFahrenheit() : temp.Value;
            return disableRound ? result : Math.Round(result);
        }
    }

    extension(int temp)
    {
        /// <summary>
        /// 根据应用设置自动转换温度
        /// 此转换仅在UI层进行
        /// </summary>
        /// <param name="temp">摄氏温度</param>
        /// <param name="disableRound">关闭取整</param>
        /// <returns></returns>
        public int ConvertTemperatureUnit()
        {
            var result = IsFahrenheit ? temp.ToFahrenheit() : temp;
            return result;
        }
    }

    extension(double temp)
    {
        public int ConvertTemperatureUnit()
        {
            var result = IsFahrenheit ? temp.ToFahrenheit() : temp;
            return (int)Math.Round(result);
        }
    }

    extension(WeatherCode code)
    {
        public ImageSource GetIconByWeather()
        {
            return new BitmapImage(GetIconUriByWeather(code));
        }
        public Uri GetIconUriByWeather()
        {
            var name = GetImageNameDay(code);
            name = name is "" ? GetImageName(code) : name;
            return new Uri("ms-appx:///Assets/Weather/" + name);
        }
    }

    extension(string description)
    {
        /// <summary>
        /// 缩短风向描述
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public string GetShortWindDirectionDescription()
        {
            var index = description.IndexOf("偏", StringComparison.Ordinal);
            if (index is -1 or 0) return description;
            return description.AsSpan(0, index).ToString() + "风";
        }
    }

    extension(WeatherCode weather)
    {
        public Uri GetIconUriByWeather(DateTime time)
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

        public ImageSource GetIconByWeather(DateTime time)
        {
            return new BitmapImage(GetIconUriByWeather(weather, time));
        }
    }

    extension(WeatherCode weatherType)
    {
        public string GetImageName()
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

        public string GetImageNameDay()
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
        public string GetImageNameNight()
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

    extension(int num1)
    {
        public int Subtract(int num2)
        {
            return num1 - num2;
        }

        public int Max(int num2)
        {
            return Math.Max(num1, num2);
        }
        public int Min(int num2)
        {
            return Math.Min(num1, num2);
        }
    }

    extension(bool value)
    {
        public Visibility BoolToVisibility()
        {
            return value ? Visibility.Visible : Visibility.Collapsed;
        }
        public Visibility BoolToVisibilityInverted()
        {
            return value ? Visibility.Collapsed : Visibility.Visible;
        }
    }
}

