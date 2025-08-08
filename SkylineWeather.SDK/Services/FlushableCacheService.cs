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
            WriteIndented = true // Ϊ�˱��ڵ��ԣ�����ѡ���Եظ�ʽ��JSON
        };
    }

    public async Task<Result<T>> GetOrCreateAsync<T>(string key, Func<CancellationToken, Task<Result<T>>> factory, TimeSpan? expiration = null, CancellationToken cancellationToken = default)
    {
        // 1. ���ȴ��ڴ滺���ж�ȡ
        if (_memoryCache.TryGetValue(key, out var cachedObject) && cachedObject is CacheItem<T> memCachedItem)
        {
            if (memCachedItem.Expiration > DateTimeOffset.UtcNow)
            {
                return new Result<T>(memCachedItem.Data);
            }
            else
            {
                // �ڴ滺����ڣ��Ƴ���
                _memoryCache.TryRemove(key, out _);
            }
        }

        cancellationToken.ThrowIfCancellationRequested();
        var filePath = GetFilePath(key);

        // 2. ���Դӳ־û����棨�ļ����ж�ȡ
        if (File.Exists(filePath))
        {
            try
            {
                var fileJson = await File.ReadAllTextAsync(filePath, cancellationToken);
                var fileCachedItem = JsonSerializer.Deserialize<CacheItem<T>>(fileJson, _serializerOptions);

                if (fileCachedItem is not null && fileCachedItem.Expiration > DateTimeOffset.UtcNow)
                {
                    // ���ļ�������ص��ڴ���
                    _memoryCache[key] = fileCachedItem;
                    return new Result<T>(fileCachedItem.Data);
                }
            }
            catch (Exception ex) when (ex is not OperationCanceledException)
            {
                _logger.LogWarning("�޷���ȡ�����л������ļ� {Key}: {ExMessage}", key, ex.Message);
            }
        }

        cancellationToken.ThrowIfCancellationRequested();

        // 3. �������δ���У���ͨ��������������������
        var result = await factory(cancellationToken);

        await result.Match(
            async succ =>
            {
                var expirationTime = expiration.HasValue
                    ? DateTimeOffset.UtcNow.Add(expiration.Value)
                    : DateTimeOffset.MaxValue;
                var itemToCache = new CacheItem<T>(expirationTime, succ);

                // �洢���ڴ滺��
                _memoryCache[key] = itemToCache;

                // �첽�洢���־û�����
                try
                {
                    var json = JsonSerializer.Serialize(itemToCache, _serializerOptions);
                    await File.WriteAllTextAsync(filePath, json, cancellationToken);
                }
                catch (Exception ex) when (ex is not OperationCanceledException)
                {
                    _logger.LogError(ex, "�޷�д�뻺���ļ� {Key}", key);
                }
            },
            fail => Task.CompletedTask);

        return result;
    }

    public Task InvalidateAsync(string key, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        // ���ڴ�ͳ־û��洢��ͬʱ�Ƴ�
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
                _logger.LogWarning("�޷�ɾ�������ļ� {Key}: {ExMessage}", key, ex.Message);
            }
        }
        return Task.CompletedTask;
    }

    public async Task FlushAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("���ڽ������ڴ滺����ˢ�µ��־û��洢...");
        foreach (var pair in _memoryCache)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var filePath = GetFilePath(pair.Key);
            try
            {
                // ʹ������ʱ���ͽ������л�
                var json = JsonSerializer.Serialize(pair.Value, pair.Value.GetType(), _serializerOptions);
                await File.WriteAllTextAsync(filePath, json, cancellationToken);
            }
            catch (Exception ex) when (ex is not OperationCanceledException)
            {
                _logger.LogError(ex, "ˢ���ڴ滺����ʧ�� {Key}", pair.Key);
            }
        }
        _logger.LogInformation("�ڴ滺��ˢ����ɡ�");
    }

    private string GetFilePath(string key)
    {
        // ʹ��SHA256��ϣ��Ϊ�ļ����Ա���Ƿ��ַ�
        var hash = SHA256.HashData(Encoding.UTF8.GetBytes(key));
        var fileName = Convert.ToHexString(hash) + ".json";
        return Path.Combine(_cacheDirectory, fileName);
    }
}