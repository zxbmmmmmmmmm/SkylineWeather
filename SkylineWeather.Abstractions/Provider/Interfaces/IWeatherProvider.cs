namespace SkylineWeather.Abstractions.Provider.Interfaces;

public interface IWeatherProvider:
    ICurrentWeatherProvider,
    IDailyWeatherProvider,
    IHourlyWeatherProvider;