using LanguageExt.Common;
using Microsoft.Extensions.Logging;
using SkylineWeather.Abstractions.Services;
using System;
using System.Collections.Concurrent;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace SkylineWeather.SDK.Services;

public class FlushableCacheService : IFlushableCacheService
{
    private record CacheItem<T>(DateTimeOffset Expiration, T Data);

    private readonly string _cacheDirectory;
    private readonly JsonSerializerOptions _serializerOptions;
    private readonly ILogger _logger;
    private readonly ConcurrentDictionary<string, object> _memoryCache = new();

    public FlushableCacheService(ILogger logger, string containerName)
    {
        _logger = logger;
        var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        var appName = Assembly.GetEntryAssembly()?.GetName().Name ?? "SkylineWeather";
        _cacheDirectory = Path.Combine(appDataPath, appName, "Cache", containerName);

        Directory.CreateDirectory(_cacheDirectory);

        _serializerOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            WriteIndented = true // 为了便于调试，可以选择性地格式化JSON
        };
    }

    public async Task<Result<T>> GetOrCreateAsync<T>(string key, Func<CancellationToken, Task<Result<T>>> factory, TimeSpan? expiration = null, CancellationToken cancellationToken = default)
    {
        // 1. 优先从内存缓存中读取
        if (_memoryCache.TryGetValue(key, out var cachedObject) && cachedObject is CacheItem<T> memCachedItem)
        {
            if (memCachedItem.Expiration > DateTimeOffset.UtcNow)
            {
                return new Result<T>(memCachedItem.Data);
            }
            else
            {
                // 内存缓存过期，移除它
                _memoryCache.TryRemove(key, out _);
            }
        }

        cancellationToken.ThrowIfCancellationRequested();
        var filePath = GetFilePath(key);

        // 2. 尝试从持久化缓存（文件）中读取
        if (File.Exists(filePath))
        {
            try
            {
                var fileJson = await File.ReadAllTextAsync(filePath, cancellationToken);
                var fileCachedItem = JsonSerializer.Deserialize<CacheItem<T>>(fileJson, _serializerOptions);

                if (fileCachedItem is not null && fileCachedItem.Expiration > DateTimeOffset.UtcNow)
                {
                    // 将文件缓存加载到内存中
                    _memoryCache[key] = fileCachedItem;
                    return new Result<T>(fileCachedItem.Data);
                }
            }
            catch (Exception ex) when (ex is not OperationCanceledException)
            {
                _logger.LogWarning("无法读取或反序列化缓存文件 {Key}: {ExMessage}", key, ex.Message);
            }
        }

        cancellationToken.ThrowIfCancellationRequested();

        // 3. 如果缓存未命中，则通过工厂方法创建新数据
        var result = await factory(cancellationToken);

        await result.Match(
            async succ =>
            {
                var expirationTime = expiration.HasValue
                    ? DateTimeOffset.UtcNow.Add(expiration.Value)
                    : DateTimeOffset.MaxValue;
                var itemToCache = new CacheItem<T>(expirationTime, succ);

                // 存储到内存缓存
                _memoryCache[key] = itemToCache;

                // 异步存储到持久化缓存
                try
                {
                    var json = JsonSerializer.Serialize(itemToCache, _serializerOptions);
                    await File.WriteAllTextAsync(filePath, json, cancellationToken);
                }
                catch (Exception ex) when (ex is not OperationCanceledException)
                {
                    _logger.LogError(ex, "无法写入缓存文件 {Key}", key);
                }
            },
            fail => Task.CompletedTask);

        return result;
    }

    public Task InvalidateAsync(string key, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        // 从内存和持久化存储中同时移除
        _memoryCache.TryRemove(key, out _);

        var filePath = GetFilePath(key);
        if (File.Exists(filePath))
        {
            try
            {
                File.Delete(filePath);
            }
            catch (Exception ex)
            {
                _logger.LogWarning("无法删除缓存文件 {Key}: {ExMessage}", key, ex.Message);
            }
        }
        return Task.CompletedTask;
    }

    public async Task FlushAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("正在将所有内存缓存项刷新到持久化存储...");
        foreach (var pair in _memoryCache)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var filePath = GetFilePath(pair.Key);
            try
            {
                // 使用运行时类型进行序列化
                var json = JsonSerializer.Serialize(pair.Value, pair.Value.GetType(), _serializerOptions);
                await File.WriteAllTextAsync(filePath, json, cancellationToken);
            }
            catch (Exception ex) when (ex is not OperationCanceledException)
            {
                _logger.LogError(ex, "刷新内存缓存项失败 {Key}", pair.Key);
            }
        }
        _logger.LogInformation("内存缓存刷新完成。");
    }

    private string GetFilePath(string key)
    {
        // 使用SHA256哈希作为文件名以避免非法字符
        var hash = SHA256.HashData(Encoding.UTF8.GetBytes(key));
        var fileName = Convert.ToHexString(hash) + ".json";
        return Path.Combine(_cacheDirectory, fileName);
    }
}