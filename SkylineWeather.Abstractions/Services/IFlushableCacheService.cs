namespace SkylineWeather.Abstractions.Services;

public interface IFlushableCacheService : ICacheService
{
    Task FlushAsync(CancellationToken cancellationToken = default);
}