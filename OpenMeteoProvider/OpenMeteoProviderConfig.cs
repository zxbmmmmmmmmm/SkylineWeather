namespace OpenMeteoProvider;

public sealed class OpenMeteoProviderConfig
{
    public Dictionary<string, string>? ForecastParameters { get; set; }
    public Dictionary<string, string>? AirQualityParameters { get; set; }
    public Dictionary<string, string>? GeocodingParameters { get; set; }
    public Dictionary<string, string>? HistoricalWeatherParameters { get; set; }
    public static OpenMeteoProviderConfig Default => new();
    
}