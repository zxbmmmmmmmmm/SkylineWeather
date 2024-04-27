using FluentWeather.Abstraction.Models;
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

public sealed class CacheHelper
{
    public static async Task<WeatherCacheBase> GetWeatherCache(GeolocationBase location)
    {
        var item = await ApplicationData.Current.LocalCacheFolder.GetOrCreateFileAsync("WeatherCache.txt");
        try
        {      
            //读取文件
            var text = await FileIO.ReadTextAsync(item);
            var data = JsonSerializer.Deserialize<List<WeatherCacheBase>>(text);
            data.RemoveAll(p => DateTime.Now - p.UpdatedTime > TimeSpan.FromMinutes(10));//删除过期的数据
            return data.Find(p => p.Location.Name == location.Name&&p.Location.AdmDistrict == location.AdmDistrict);
        }
        catch
        {
            return null;
        }

    }
    public static async void Cache(MainPageViewModel viewModel)
    {
        var item = await ApplicationData.Current.LocalCacheFolder.GetOrCreateFileAsync("WeatherCache.txt");
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

            AirCondition = viewModel.AirCondition,
            Location = viewModel.CurrentGeolocation,
            HourlyForecasts = viewModel.HourlyForecasts,
            Indices = viewModel.Indices,
            Precipitation = viewModel.Precipitation,
            UpdatedTime = DateTime.Now,
            Warnings = viewModel.Warnings,
            WeatherNow = viewModel.WeatherNow,
        };
        if(viewModel.SunRise is not null)
        {
            cache.SunRise = viewModel.SunRise.Value;
            cache.SunSet = viewModel.SunSet!.Value;
        }
        cacheData.RemoveAll(p => (DateTime.Now - p["UpdatedTime"]?.GetValue<DateTime>() > TimeSpan.FromMinutes(10)) || p["Location"]?["Location"].Deserialize<Location>() == viewModel.CurrentGeolocation.Location);//删除过期的数据
        cacheData.Add(JsonSerializer.SerializeToNode(cache));      
        var json = JsonSerializer.Serialize(cacheData);
        await FileIO.WriteTextAsync(item,json);
    }

    public static async void Clear()
    {
        var item = await ApplicationData.Current.LocalCacheFolder.GetOrCreateFileAsync("WeatherCache.txt");
        await FileIO.WriteTextAsync(item, "");
    }
}