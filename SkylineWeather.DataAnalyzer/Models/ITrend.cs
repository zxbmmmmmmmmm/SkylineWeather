namespace SkylineWeather.DataAnalyzer.Models;

public interface ITrend
{
    public double Slope { get; set; }
    public double Intercept { get; set; }
    public double CorrelationCoefficient { get; set; }
}