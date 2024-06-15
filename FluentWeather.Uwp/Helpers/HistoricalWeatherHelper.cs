using FluentWeather.Abstraction.Interfaces.WeatherProvider;
using FluentWeather.Abstraction.Models;
using FluentWeather.DIContainer;
using FluentWeather.OpenMeteoProvider;
using Microsoft.Extensions.DependencyInjection;
using OpenMeteoApi;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Windows.Storage;

namespace FluentWeather.Uwp.Helpers;

public class HistoricalWeatherHelper
{
    public static async Task<List<WeatherDailyBase>> DownloadHistoricalWeatherAsync(Location location)
    {
        var folder = await ApplicationData.Current.LocalFolder.GetOrCreateFolderAsync("HistoricalWeather");
        var folder1 = await folder.GetOrCreateFolderAsync(location.GetHashCode().ToString());
        //if (await folder1.TryGetItemAsync("data") is not null)
        //{
        //    var cache = await folder1.GetFileAsync("data");
        //    return await JsonSerializer.DeserializeAsync<List<WeatherDailyBase>>(await cache.OpenStreamForReadAsync(), new JsonSerializerOptions { TypeInfoResolver = SourceGenerationContext.Default });
        //}

        var service = Locator.ServiceProvider.GetService<IHistoricalWeatherProvider>();
        service ??= new OpenMeteoProvider.OpenMeteoProvider();
        var data = await service.GetHistoricalDailyWeather(location.Longitude, location.Latitude,DateTime.Parse("1940-01-01"), DateTime.Parse($"{DateTime.Now.Year}-01-01"));

        //保存原始数据
        var file =await folder1.GetOrCreateFileAsync("data");
        using var stream = await file.OpenStreamForWriteAsync();
        await JsonSerializer.SerializeAsync(stream, data, new JsonSerializerOptions { TypeInfoResolver = SourceGenerationContext.Default });
        await stream.FlushAsync();

        return data;
    }

    public static async Task<Dictionary<string, HistoricalDailyWeatherBase>> AnalyseHistoricalWeatherAsync(List<WeatherDailyBase> dailyWeather)
    {
        return await Task.Run(() =>
        {
            var dic = new Dictionary<string, LinkedList<WeatherDailyBase>>();
            foreach (var item in dailyWeather)
            {
                var date = item.Time.ToString("MM-dd");
                if (!dic.ContainsKey(date))
                {
                    dic[date] = [];
                }
                dic[date].AddLast(item);
            }
            var result = new Dictionary<string, HistoricalDailyWeatherBase>();

            foreach (var pair in dic)
            {
                var maxTemp = int.MinValue;
                var maxTempDate = DateTime.MinValue;
                var minTemp = int.MaxValue;
                var minTempDate = DateTime.MinValue;

                double? totalPrecip = 0.0;
                double? maxPrecip = 0.0;
                DateTime? maxPrecipDate = DateTime.MinValue;

                var totalWindSpeed = 0;
                var totalMaxTemp = 0;
                var totalMinTemp = 0;
                double? totalPrecipHours = 0.0;
                var weatherCodeDic = new Dictionary<WeatherCode, int>();
                var windDirectionDic = new Dictionary<WindDirection, int>();
                var count = 0;
                foreach (var item in pair.Value)
                {
                    if (item.MaxTemperature >= maxTemp)
                    {
                        maxTemp = item.MaxTemperature;
                        maxTempDate = item.Time;
                    }
                    if (item.MinTemperature <= minTemp)
                    {
                        minTemp = item.MinTemperature;
                        minTempDate = item.Time;
                    }
                    if (item.Precipitation >= maxPrecip)
                    {
                        maxPrecip = item.Precipitation;
                        maxPrecipDate = item.Time;
                    }
                    totalWindSpeed += item.WindSpeed;
                    windDirectionDic[item.WindDirection] = windDirectionDic.GetOrCreate(item.WindDirection) + 1;

                    totalMaxTemp += item.MaxTemperature;
                    totalMinTemp += item.MinTemperature;
                    totalPrecipHours += item.PrecipitationHours;
                    totalPrecip += item.Precipitation;
                    weatherCodeDic[item.WeatherType] = weatherCodeDic.GetOrCreate(item.WeatherType) + 1;
                    count++;
                }
                var historicalWeather = new HistoricalDailyWeatherBase
                {
                    Date = pair.Value.First.Value.Time.Date,
                    WindDirection = windDirectionDic.OrderBy(p => p.Value).First().Key,
                    AverageWindSpeed = totalWindSpeed / count,
                    AverageMaxTemperature = totalMaxTemp / count,
                    AverageMinTemperature = totalMinTemp / count,
                    MaxPrecipitation = maxPrecip,
                    MaxPrecipitationDate = maxPrecipDate,
                    AveragePrecipitation = totalPrecip / count,
                    AveragePrecipitationHours = totalPrecipHours / count,
                    HistoricalMaxTemperature = maxTemp,
                    HistoricalMaxTemperatureDate = maxTempDate,
                    HistoricalMinTemperature = minTemp,
                    HistoricalMinTemperatureDate = minTempDate,
                    Weather = weatherCodeDic.OrderBy(p => p.Value).First().Key,
                };
                result[pair.Key] = historicalWeather;
            }

            return result;
        });
    }


    public static void SaveHistoricalWeather(IList<HistoricalDailyWeatherBase> weatherList)
    {

    }

    public static void ClearHistoricalWeather()
    {

    }
    public static async Task<HistoricalDailyWeatherBase> GetHistoricalWeatherAsync(Location location,DateTime date)
    {
        var folder = await ApplicationData.Current.LocalFolder.GetOrCreateFolderAsync("HistoricalWeather");
        var folderItem = await folder.TryGetItemAsync(location.GetHashCode().ToString());
        if (folderItem == null) return null;
        var folder1 = (StorageFolder)folderItem;
        var id = date.Month.ToString().PadLeft(2, '0');
        var file = await folder1.GetFileAsync(id);
        using var stream = await file.OpenStreamForReadAsync();
        var dic = JsonSerializer.Deserialize<Dictionary<string, HistoricalDailyWeatherBase>>(stream, new JsonSerializerOptions { TypeInfoResolver = SourceGenerationContext.Default });
        return dic[date.Day.ToString()];

    }
}