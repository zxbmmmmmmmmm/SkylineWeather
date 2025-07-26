namespace SkylineWeather.DataAnalyzer.Models;

public record TemperatureTrend : Trend
{
    public TemperatureTrendType Type { get; set; }
}
public enum TemperatureTrendType
{
    Increasing,
    Decreasing,
    Steady,
    Fluctuating,
}