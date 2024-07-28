using FluentWeather.Abstraction.Models;
using System.Threading.Tasks;
using System;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Storage;
using static FluentWeather.Abstraction.Models.WeatherCode;

namespace FluentWeather.Uwp.Shared.Helpers;

public static class AssetsHelper
{
    public static string GetWeatherIconName(this WeatherCode weatherType)
    {
        return weatherType switch
        {
            SlightHail or ModerateOrHeavyHail => "BlowingHail.png",
            HeavyRain => "HeavyRain.png",
            ModerateRain => "LightRain.png",
            SlightRain => "LightRain.png",
            ModerateRainShowers => "LightRain.png",
            PartlyCloudy => "Cloudy.png",
            Overcast => "VeryCloudy.png",
            SlightSnowFall => "LightSnow.png",
            HeavySnowFall => "HeavySnow.png",
            Fog => "Fog.png",
            LightFreezingRain or HeavyFreezingRain => "FreezingRain.png",
            SlightSleet or ModerateOrHeavySleet => "RainSnow.png",
            ThunderstormWithHeavyHail or SlightOrModerateThunderstorm or ThunderstormWithSlightHail or HeavyThunderStorm => "Thunder.png",
            Clear => "SunnyDay.png",
            Haze => "HazeSmokeDay.png",
            MainlyClear => "MostlySunnyDay.png",
            ViolentRainShowers or ViolentRainShowers or SlightRainShowers => "RainShowersDay.png",
            SlightSnowShowers or HeavySnowShowers => "SnowShowersDay.png",
            _ => "PartlyCloudy.png",
        };
    }

    public static string GetBase64String(this WeatherCode weatherType)
    {
        return weatherType switch
        {
            SlightHail or ModerateOrHeavyHail => AssetBase64Resized32.BlowingHail,
            HeavyRain => AssetBase64Resized32.HeavyRain,
            ModerateRain => AssetBase64Resized32.LightRain,
            SlightRain => AssetBase64Resized32.LightRain,
            ModerateRainShowers => AssetBase64Resized32.LightRain,
            PartlyCloudy => AssetBase64Resized32.Cloudy,
            Overcast => AssetBase64Resized32.VeryCloudy,
            SlightSnowFall => AssetBase64Resized32.LightSnow,
            HeavySnowFall => AssetBase64Resized32.HeavySnow,
            Fog => AssetBase64Resized32.Fog,
            LightFreezingRain or HeavyFreezingRain => AssetBase64Resized32.FreezingRain,
            SlightSleet or ModerateOrHeavySleet => AssetBase64Resized32.RainSnow,
            ThunderstormWithHeavyHail or SlightOrModerateThunderstorm or ThunderstormWithSlightHail or HeavyThunderStorm => AssetBase64Resized32.Thunder,
            Clear => AssetBase64Resized32.SunnyDay,
            Haze => AssetBase64Resized32.HazeSmoke,
            MainlyClear => AssetBase64Resized32.MostlySunnyDay,
            ViolentRainShowers or ViolentRainShowers or SlightRainShowers => AssetBase64Resized32.RainShowersDay,
            SlightSnowShowers or HeavySnowShowers => AssetBase64Resized32.SnowShowersDay,
            _ => AssetBase64Resized32.PartlyCloudyDay,
        };
    }
    public static async Task<Uri> ToBase64Url(this Uri sourceUri)
    {
        var file = await StorageFile.GetFileFromApplicationUriAsync(sourceUri);
        var buffer = await FileIO.ReadBufferAsync(file);
        var bytes = buffer.ToArray();
        var result = Convert.ToBase64String(bytes);
        return new Uri("data:image/png;base64," + result);
    }
    public static string GetBackgroundImageName(this WeatherCode weather)
    {
        var code = (int)weather;
        if (code is 0)
            return "Clear";
        if (code is 1 or 2)
            return "PartlyCloudy";
        if (code is 3)
            return "Overcast";
        if (50 <= code && code <= 69 || (80 <= code && code <= 82))
            return "Rain";
        if (40 <= code && code <= 49)
            return "Fog";
        if (70 <= code && code <= 79)
            return "Snow";

        return "All";
    }
}