using SkylineWeather.DataAnalyzer.Models;
using UnitsNet;

namespace SkylineWeather.DataAnalyzer.Analyzers;

public class SingleTemperatureTrendAnalyzer : ITrendAnalyzer<Temperature, TemperatureTrend>
{
    private const double SignificantSlope = 0.5;
    private const double WeakCorrelation = 0.7;
    public static SingleTemperatureTrendAnalyzer Instance { get; } = new();
    public TemperatureTrend GetTrend(IEnumerable<Temperature> data)
    {
        // 使用最小二乘法计算温度趋势
        var arr = data.ToArray();
        if (arr.Length < 2)
        {
            return new TemperatureTrend { Type = TemperatureTrendType.Steady };
        }
        var x = arr.Select((__, i) => (double)i).ToArray();
        var y = arr.Select(t => t.DegreesCelsius).ToArray();
        var n = arr.Length;
        var sumX = x.Sum();
        var sumY = y.Sum();
        var sumX2 = x.Select(xx => xx * xx).Sum();
        var sumY2 = y.Select(yy => yy * yy).Sum();
        var sumXY = x.Zip(y, (xx, yy) => xx * yy).Sum();
        var a = (sumY * sumX2 - sumX * sumXY) / (n * sumX2 - sumX * sumX);
        var b = (n * sumXY - sumX * sumY) / (n * sumX2 - sumX * sumX);
        var r = (n * sumXY - sumX * sumY) / Math.Sqrt((n * sumX2 - sumX * sumX) * (n * sumY2 - sumY * sumY));
        var trend = new TemperatureTrend
        {
            Slope = b,
            Intercept = a,
            CorrelationCoefficient = r
        };

        if (Math.Abs(trend.CorrelationCoefficient) <= WeakCorrelation)
        {
            trend.Type = TemperatureTrendType.Fluctuating;
        }
        else
        {
            trend.Type = trend.Slope switch
            {
                >= SignificantSlope => TemperatureTrendType.Increasing,
                <= -SignificantSlope => TemperatureTrendType.Decreasing,
                _ => TemperatureTrendType.Steady
            };
        }

        return trend;
    }
}

