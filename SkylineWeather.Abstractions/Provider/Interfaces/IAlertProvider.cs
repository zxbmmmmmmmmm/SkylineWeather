using LanguageExt.Common;
using SkylineWeather.Abstractions.Models.Weather;
using SkylineWeather.Abstractions.Models;
using SkylineWeather.Abstractions.Models.Alert;

namespace SkylineWeather.Abstractions.Provider.Interfaces;

public interface IAlertProvider
{
    public Task<Result<List<Alert>>> GetAlertsAsync(Location location);
}