namespace SkylineWeather.DataAnalyzer.Models;

public record Trend
{
    public double Slope { get; set; }
    public double Intercept { get; set; }
    public double CorrelationCoefficient { get; set; }
}