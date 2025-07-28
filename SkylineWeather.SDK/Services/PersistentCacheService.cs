using LanguageExt.Common;
using Microsoft.Extensions.Logging;
using SkylineWeather.Abstractions.Services;
using System;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SkylineWeather.SDK.Services;

public class PersistentCacheService : ICacheService
{
    private record CacheItem<T>(DateTimeOffset Expiration, T Data);

    private readonly string _cacheDirectory;
    private readonly JsonSerializerOptions _serializerOptions;
    private readonly ILogger _logger;

    public PersistentCacheService(ILogger logger)
    {
        // 获取本地应用数据目录，并为本应用创建一个子目录
        _logger = logger;
        var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        var appName = Assembly.GetEntryAssembly()?.GetName().Name ?? "SkylineWeather";
        _cacheDirectory = Path.Combine(appDataPath, appName, "Cache");

        // 确保缓存目录存在
        Directory.CreateDirectory(_cacheDirectory);

        _serializerOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
        };
    }

    public async Task<Result<T>> GetOrCreateAsync<T>(string key, Func<Task<Result<T>>> factory, TimeSpan? absoluteExpirationRelativeToNow = null)
    {
        var filePath = GetFilePath(key);

        if (File.Exists(filePath))
        {
            try
            {
                var fileJson = await File.ReadAllTextAsync(filePath);
                var cachedItem = JsonSerializer.Deserialize<CacheItem<T>>(fileJson, _serializerOptions);

                if (cachedItem is not null && cachedItem.Expiration > DateTimeOffset.UtcNow)
                {
                    return new Result<T>(cachedItem.Data);
                }
            }
            catch (Exception ex)
            {
                // 记录错误，但作为缓存未命中继续执行
                _logger.LogWarning("Failed to read or deserialize cache file {Key}: {ExMessage}", key, ex.Message);
            }
        }

        var result = await factory();

        await result.Match(
            async succ =>
            {
                var expiration = absoluteExpirationRelativeToNow.HasValue
                    ? DateTimeOffset.UtcNow.Add(absoluteExpirationRelativeToNow.Value)
                    : DateTimeOffset.MaxValue;
                var itemToCache = new CacheItem<T>(expiration, succ);
                var json = JsonSerializer.Serialize(itemToCache, _serializerOptions);
                await File.WriteAllTextAsync(filePath, json);
            },
            fail => Task.CompletedTask);

        return result;
    }

    public Task InvalidateAsync(string key)
    {
        var filePath = GetFilePath(key);
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }
        return Task.CompletedTask;
    }

    private string GetFilePath(string key)
    {
        var hash = SHA256.HashData(Encoding.UTF8.GetBytes(key));
        var fileName = Convert.ToHexString(hash) + ".json";
        return Path.Combine(_cacheDirectory, fileName);
    }
}