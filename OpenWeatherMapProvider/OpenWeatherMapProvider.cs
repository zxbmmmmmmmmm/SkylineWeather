using LanguageExt.Common;
using OpenWeatherMap.Cache;
using SkylineWeather.Abstractions.Models;
using SkylineWeather.Abstractions.Models.Weather;
using SkylineWeather.Abstractions.Provider;
using SkylineWeather.Abstractions.Provider.Interfaces;

namespace OpenWeatherMapProvider;

public class OpenWeatherMapProvider : ProviderBase,ICurrentWeatherProvider
{
    public override string Name => "OpenWeatherMap";
    public override string Id => "open-weather-map";
    private OpenWeatherMapCache Client { get; init; }
    public OpenWeatherMapProvider()
    {
        Client = new OpenWeatherMapCache("", 9_500, Enums.FetchMode.AlwaysUseLastFetchedValue);
    }

    public async Task<Result<CurrentWeather>> GetCurrentWeatherAsync(Location location, CancellationToken cancellationToken = default)
    {
        var query = new OpenWeatherMap.Cache.Models.Location(location.Latitude, location.Longitude);
        var result = await Client.GetReadingsAsync(query, cancellationToken);
        throw new NotImplementedException();
    }
}