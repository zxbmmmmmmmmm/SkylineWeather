using FluentWeather.Abstraction.Models;
using FluentWeather.QWeatherProvider.Models;
using FluentWeather.Uwp.Shared;
using FluentWeather.Uwp.ViewModels;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using Windows.Storage;

namespace FluentWeather.Uwp.Helpers;

public class CacheHelper
{
    public static async Task<WeatherCacheBase> GetWeatherCache(GeolocationBase location)
    {
        var item = (await ApplicationData.Current.LocalCacheFolder.TryGetItemAsync("WeatherCache.txt") )as IStorageFile;
        if (item is null)
        {
            item = await CreateCacheFile();
            return null;
        }
        try
        {      
            //读取文件
            var text = await FileIO.ReadTextAsync(item);
            var data = JsonSerializer.Deserialize<List<WeatherCacheBase>>(text);
            data.RemoveAll(p => DateTime.Now - p.UpdatedTime > TimeSpan.FromMinutes(10));//删除过期的数据
            return data.Find(p => p.Location.Name == location.Name);
        }
        catch
        {
            return null;
        }

    }
    public static async void Cache(MainPageViewModel viewModel)
    {
        var item = (await ApplicationData.Current.LocalCacheFolder.TryGetItemAsync("WeatherCache.txt")) as IStorageFile;
        var text = await FileIO.ReadTextAsync(item);
        List<JsonNode> cacheData;
        try
        {
            cacheData = JsonSerializer.Deserialize<List<JsonNode>>(text);
        }
        catch
        {
            cacheData = new();
        }
        var cache = new WeatherCacheBase
        {
            DailyForecasts = viewModel.DailyForecasts,
            SunRise = viewModel.SunRise,
            SunSet = viewModel.SunSet,
            AirCondition = viewModel.AirCondition,
            Location = viewModel.CurrentLocation,
            HourlyForecasts = viewModel.HourlyForecasts,
            Indices = viewModel.Indices,
            Precipitation = viewModel.Precipitation,
            UpdatedTime = DateTime.Now,
            Warnings = viewModel.Warnings,
            WeatherDescription = viewModel.WeatherDescription,
            WeatherNow = viewModel.WeatherNow,
        };
        cacheData.RemoveAll(p => DateTime.Now - p["UpdatedTime"]?.GetValue<DateTime>() > TimeSpan.FromMinutes(10));//删除过期的数据
        cacheData.Add(JsonSerializer.SerializeToNode(cache));      
        var json = JsonSerializer.Serialize(cacheData);
        await FileIO.WriteTextAsync(item,json);
    }
    public static async Task<IStorageFile> CreateCacheFile()
    {
        return await ApplicationData.Current.LocalCacheFolder.CreateFileAsync("WeatherCache.txt");
    }
}