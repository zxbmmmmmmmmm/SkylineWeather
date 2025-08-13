using SkylineWeather.Abstractions.Models.AirQuality;

namespace SkylineWeather.DataAnalyzer.Models;

public interface IAqiAnalyzer
{
    Aqi CalculateAqi(AirQuality airQuality);
}