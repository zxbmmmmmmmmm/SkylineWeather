using FluentWeather.Abstraction.Models;
using FluentWeather.Uwp.Shared;
using FluentWeather.Uwp.ViewModels;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using Windows.Storage;

namespace FluentWeather.Uwp.Helpers;

public sealed class CacheHelper
{
    public static async Task CacheAsync(MainPageViewModel viewModel)
    {
        var item = await ApplicationData.Current.LocalCacheFolder.GetOrCreateFileAsync(viewModel.CurrentGeolocation.Location.GetHashCode().ToString());
        var cache = new WeatherCacheBase
        {
            DailyForecasts = viewModel.DailyForecasts,
            AirCondition = viewModel.AirCondition,
            Location = viewModel.CurrentGeolocation,
            HourlyForecasts = viewModel.HourlyForecasts,
            Indices = viewModel.Indices,
            Precipitation = viewModel.Precipitation,
            UpdatedTime = DateTime.Now,
            Warnings = viewModel.Warnings,
            WeatherNow = viewModel.WeatherNow,
            SunRise = viewModel.SunRise,
            SunSet = viewModel.SunSet,
        };
        await FileIO.WriteTextAsync(item, "");
        using var stream = await item.OpenStreamForWriteAsync();

        var options = new JsonSerializerOptions { TypeInfoResolver = SourceGenerationContext.Default, };

        await JsonSerializer.SerializeAsync(stream, cache, options);

    }
    public static async Task<WeatherCacheBase> GetCacheAsync(GeolocationBase location)
    {
        var item = await ApplicationData.Current.LocalCacheFolder.GetOrCreateFileAsync(location.Location.GetHashCode().ToString());

        if (DateTimeOffset.Now - (await item.GetBasicPropertiesAsync()).DateModified > TimeSpan.FromMinutes(15))
            return null;
        try
        {
            //读取文件
            using var stream = await item.OpenStreamForReadAsync();
            if (stream.Length == 0) return null;
            var options = new JsonSerializerOptions { TypeInfoResolver = SourceGenerationContext.Default };
            var result = await JsonSerializer.DeserializeAsync<WeatherCacheBase>(stream, options);
            return result;
        }
        catch
        {
            return null;
        }

    }

    /// <summary>
    /// 清除所有缓存文件
    /// </summary>
    /// <returns></returns>
    public static async Task Clear()
    {
        var files = await ApplicationData.Current.LocalCacheFolder.GetFilesAsync();
        foreach (var item in files)
        {
            await item.DeleteAsync();
        }
    }

    /// <summary>
    /// 删除未使用的缓存文件
    /// </summary>
    /// <returns></returns>
    public static async Task DeleteUnused()
    {
        var files = await ApplicationData.Current.LocalCacheFolder.GetFilesAsync();
        var saved = Common.Settings.SavedCities;
        var current = Common.Settings.DefaultGeolocation;

        foreach (var item in files)
        {
            if (saved.FirstOrDefault(p => p.Location.GetHashCode().ToString() == item.Name) is not null) continue;
            if (current?.Location.GetHashCode().ToString() == item.Name) continue;
            await item.DeleteAsync();
        }
    }
}
[JsonSourceGenerationOptions(WriteIndented = true)]
[JsonSerializable(typeof(WeatherDailyBase))]
[JsonSerializable(typeof(WeatherHourlyBase))]
[JsonSerializable(typeof(WeatherNowBase))]
[JsonSerializable(typeof(WeatherWarningBase))]
[JsonSerializable(typeof(WeatherCacheBase))]
[JsonSerializable(typeof(Location))]
[JsonSerializable(typeof(AirConditionBase))]
[JsonSerializable(typeof(IndicesBase))]
[JsonSerializable(typeof(PrecipitationBase))]
[JsonSerializable(typeof(PrecipitationItemBase))]
[JsonSerializable(typeof(HistoricalDailyWeatherBase))]
[JsonSerializable(typeof(Dictionary<string, HistoricalDailyWeatherBase>))]
internal partial class SourceGenerationContext : JsonSerializerContext
{
}