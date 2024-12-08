namespace SkylineWeather.DataAnalyzer.Models;

public record TemperatureTrend : ITrend
{
    public TemperatureTrendType Type { get; set; }
    public double Slope { get; set; }
    public double Intercept { get; set; }
    public double CorrelationCoefficient { get; set; }
}
public enum TemperatureTrendType
{
    Increasing,
    Decreasing, 
    Steady,
    Fluctuating,
}